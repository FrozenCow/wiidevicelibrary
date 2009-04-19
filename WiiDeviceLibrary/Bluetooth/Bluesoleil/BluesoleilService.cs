//    Copyright 2009 Wii Device Library authors
//
//    This file is part of Wii Device Library.
//
//    Wii Device Library is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Wii Device Library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Wii Device Library.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    public class BluesoleilService: IDisposable
    {
        #region Fields
        Thread createdThread;
        #endregion

        #region Singleton
        private static NativeMethods.PCALLBACK_CONNECTION_STATUS CONNECTION_STATUS_DELEGATE;
        private static NativeMethods.PCALLBACK_BLUETOOTH_STATUS BLUETOOTH_STATUS_DELEGATE;
        private static BluesoleilService _Instance;
        public static BluesoleilService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BluesoleilService();
                return _Instance;
            }
        }
        #endregion

        #region Properties
        private IDictionary<int, BluetoothConnection> lookupConnections = new Dictionary<int, BluetoothConnection>();
        private ICollection<BluetoothConnection> readonlyConnections;
        private IList<BluetoothConnection> connections = new List<BluetoothConnection>();
        public ICollection<BluetoothConnection> Connections
        {
            get { return readonlyConnections; }
        }

        private bool _IsInitialized;
        public bool IsInitialized
        {
            get { return _IsInitialized; }
        }
	
        public bool IsStarted
        {
            get { return NativeMethods.BT_IsBlueSoleilStarted(10); }
        }
        #endregion

        #region Constructors
        internal BluesoleilService()
        {
            readonlyConnections = new ReadOnlyCollection<BluetoothConnection>(connections);
        }
        #endregion

        #region Initialization
        public void Initialize()
        {
            if (IsInitialized)
                throw new InvalidOperationException("BluesoleilService is already initialized.");
            createdThread = Thread.CurrentThread;

            if (!NativeMethods.BT_InitializeLibrary())
                throw new BluesoleilException("The library could not be initialized.");

            CONNECTION_STATUS_DELEGATE = new NativeMethods.PCALLBACK_CONNECTION_STATUS(static_CONNECTION_STATUS_CALLBACK);
            BLUETOOTH_STATUS_DELEGATE = new NativeMethods.PCALLBACK_BLUETOOTH_STATUS(static_BLUETOOTH_STATUS_CALLBACK);
            NativeMethods.BT_RegisterCallback(NativeMethods.EVENT.EVENT_CONNECTION_STATUS, CONNECTION_STATUS_DELEGATE);
            NativeMethods.BT_RegisterCallback(NativeMethods.EVENT.EVENT_BLUETOOTH_STATUS, BLUETOOTH_STATUS_DELEGATE);
            _IsInitialized = true;
        }
        #endregion

        #region Helper Methods
        private void ValidateThread()
        {
            //if (Thread.CurrentThread != createdThread)
            //    throw new InvalidOperationException("The thread calling is another thread than the one that created Bluesoleil.");
        }
        private void ValidateStatus(NativeMethods.BTSTATUS status)
        {
                switch (status)
                {
                    case NativeMethods.BTSTATUS.BTSTATUS_SUCCESS:
                        break;
                    case NativeMethods.BTSTATUS.BTSTATUS_FAIL:
                        throw new BluesoleilFailException();
                    case NativeMethods.BTSTATUS.BTSTATUS_BT_NOT_READY:
                        throw new BluesoleilNotReadyException();
                    case NativeMethods.BTSTATUS.BTSTATUS_BT_BUSY:
                        throw new BluesoleilBluetoothBusyException();
                    case NativeMethods.BTSTATUS.BTSTATUS_SYSTEM_ERROR:
                        throw new BluesoleilSystemException();
                    default:
                        throw new BluesoleilException(status.ToString());
                }
        }
        private BluetoothDevice CreateBluetoothDevice(NativeMethods.BLUETOOTH_DEVICE_INFO deviceInfo)
        {
            return new BluetoothDevice(this, deviceInfo);
        }
        #endregion

        #region Callback Methods
        private static void static_BLUETOOTH_STATUS_CALLBACK(NativeMethods.BLUETOOTH_STATUS ucStatus)
        {
            Instance.BLUETOOTH_STATUS_CALLBACK(ucStatus);
        }

        private void BLUETOOTH_STATUS_CALLBACK(NativeMethods.BLUETOOTH_STATUS ucStatus)
        {
            // _IsStarted = ucStatus == NativeMethods.BLUETOOTH_STATUS.STATUS_BLUETOOTH_STARTED;
        }

        private static void static_CONNECTION_STATUS_CALLBACK(short wServiceClass, IntPtr lpBdAddr, NativeMethods.CONNECTION_STATUS ucStatus, int dwConnetionHandle)
        {
            Instance.CONNECTION_STATUS_CALLBACK(wServiceClass, lpBdAddr, ucStatus, dwConnetionHandle);
        }

        private void CONNECTION_STATUS_CALLBACK(short wServiceClass, IntPtr lpBdAddr, NativeMethods.CONNECTION_STATUS ucStatus, int dwConnetionHandle)
        {
            BluetoothConnection connection;
            if (!lookupConnections.TryGetValue(dwConnetionHandle, out connection))
                return;
            BluetoothConnectionStatus newStatus = BluetoothConnectionStatus.Connected;
            if (ucStatus == NativeMethods.CONNECTION_STATUS.STATUS_INCOMING_DISCONNECT || ucStatus == NativeMethods.CONNECTION_STATUS.STATUS_OUTGOING_DISCONNECT)
                newStatus = BluetoothConnectionStatus.Disconnected;
            connection.Status = newStatus;

            lookupConnections.Remove(connection.ConnectionHandle);
            connections.Remove(connection);
            
            if (newStatus == BluetoothConnectionStatus.Disconnected)
                OnConnectionClosed(new BluetoothConnectionEventArgs(connection));
        }

        //private void ERROR_CALLBACK(uint errorCode)
        //{
        //    Console.WriteLine("ErrorCode: " + errorCode);
        //}
        #endregion

        #region Public Methods
        public void StartBluetooth()
        {
            NativeMethods.BTSTATUS status = NativeMethods.BT_StartBluetooth();
            ValidateStatus(status);
        }

        public void StopBluetooth()
        {
            NativeMethods.BTSTATUS status = NativeMethods.BT_StartBluetooth();
            ValidateStatus(status);
        }

        public BluetoothDevice[] InquireDevices(TimeSpan timeOut)
        {
            ValidateThread();
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
            {
                Thread.Sleep(timeOut);
                NativeMethods.BT_CancelInquiry();
            }));
            NativeMethods.BLUETOOTH_DEVICE_INFO[] deviceInfos;
            NativeMethods.BTSTATUS status = NativeMethods.BT_InquireDevices(NativeMethods.INQUIRY.INQUIRY_GENERAL_REFRESH, 0, out deviceInfos);
            ValidateStatus(status);

            BluetoothDevice[] devices = new BluetoothDevice[deviceInfos.Length];
            for (int i = 0; i < devices.Length; i++)
                devices[i] = CreateBluetoothDevice(deviceInfos[i]);
            return devices;
        }

        public BluetoothService[] BrowseServices(BluetoothDevice device)
        {
            ValidateThread();
            NativeMethods.GENERAL_SERVICE_INFO[] serviceInfos;
            NativeMethods.BTSTATUS status = NativeMethods.BT_BrowseServices(device.DeviceInfo, true, out serviceInfos);
            ValidateStatus(status);
            BluetoothService[] services = new BluetoothService[serviceInfos.Length];
            for (int i = 0; i < serviceInfos.Length; i++)
            {
                services[i] = new BluetoothService(this, device, serviceInfos[i]);
            }
            return services;
        }

        public BluetoothConnection ConnectService(BluetoothService service)
        {
            ValidateThread();
            int connectionHandle = 0;
            NativeMethods.BTSTATUS status = NativeMethods.BT_ConnectService(service.Device.DeviceInfo, service.ServiceInfo, ref connectionHandle);
            ValidateStatus(status);
            BluetoothConnection connection = new BluetoothConnection(this, service, connectionHandle);

            lookupConnections.Add(connectionHandle, connection);
            connections.Add(connection);

            OnConnectionOpened(new BluetoothConnectionEventArgs(connection));
            return connection;
        }

        public void DisconnectService(BluetoothConnection connection)
        {
            int connectionHandle = connection.ConnectionHandle;
            NativeMethods.BTSTATUS status = NativeMethods.BT_DisconnectService(connectionHandle);
            ValidateStatus(status);
            lookupConnections.Remove(connectionHandle);
            connections.Remove(connection);
            OnConnectionClosed(new BluetoothConnectionEventArgs(connection));
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (IsInitialized)
            {
                NativeMethods.BT_UnregisterCallback(NativeMethods.EVENT.EVENT_CONNECTION_STATUS);
                NativeMethods.BT_UnregisterCallback(NativeMethods.EVENT.EVENT_BLUETOOTH_STATUS);
                NativeMethods.BT_UninitializeLibrary();
            }
            _IsInitialized = false;
        }
        #endregion

        #region Public Events
        #region BluetoothStarted Event
        protected virtual void OnBluetoothStarted(EventArgs e)
        {
            if (BluetoothStarted == null)
                return;
            BluetoothStarted(this, e);
        }
        public event EventHandler<EventArgs> BluetoothStarted;
        #endregion

        #region BluetoothStopped Event
        protected virtual void OnBluetoothStopped(EventArgs e)
        {
            if (BluetoothStopped == null)
                return;
            BluetoothStopped(this, e);
        }
        public event EventHandler<EventArgs> BluetoothStopped;
        #endregion
	
        #region ConnectionOpened Event
        protected virtual void OnConnectionOpened(BluetoothConnectionEventArgs e)
        {
            if (ConnectionOpened == null)
                return;
            ConnectionOpened(this, e);
        }
        public event EventHandler<BluetoothConnectionEventArgs> ConnectionOpened;
        #endregion
	
        #region ConnectionClosed Event
        protected virtual void OnConnectionClosed(BluetoothConnectionEventArgs e)
        {
            if (ConnectionClosed == null)
                return;
            ConnectionClosed(this, e);
        }
        public event EventHandler<BluetoothConnectionEventArgs> ConnectionClosed;
        #endregion
        #endregion
    }
}
