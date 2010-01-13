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
using WiiDeviceLibrary.Bluetooth.MsHid;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    public class BluesoleilDeviceProvider : IDeviceProvider
    {
        private BluesoleilService bluesoleil;
        private Thread discoveringThread;

        private IDictionary<BluetoothAddress, BluesoleilDeviceInfo> lookupDeviceInfo = new Dictionary<BluetoothAddress, BluesoleilDeviceInfo>();
        private IDictionary<BluetoothAddress, ReportDevice> lookupDevice = new Dictionary<BluetoothAddress, ReportDevice>();
        private IDictionary<BluetoothAddress, BluetoothConnection> lookupConnection = new Dictionary<BluetoothAddress, BluetoothConnection>();

        private ICollection<IDeviceInfo> foundDevices = new List<IDeviceInfo>();
        public ICollection<IDeviceInfo> FoundDevices
        {
            get { return foundDevices; }
        }

        private ICollection<IDevice> connectedDevices = new List<IDevice>();
        public ICollection<IDevice> ConnectedDevices
        {
            get { return connectedDevices; }
        }

        public bool IsDiscovering
        {
            get { return discoveringThread != null; }
        }

        public BluesoleilDeviceProvider()
        {
        }

        public void StartDiscovering()
        {
            if (discoveringThread != null)
                throw new InvalidOperationException("The " + GetType().Name + " is already discovering.");
            discoveringThread = new Thread(Discovering);
            discoveringThread.Name = "Discovering Thread";
            discoveringThread.Start();
        }

        public void StopDiscovering()
        {
            if (discoveringThread != null)
            {
                Thread stoppingThread = discoveringThread;
                discoveringThread = null;
                stoppingThread.Join();
            }
        }

        private TimeSpan pollingTime = TimeSpan.FromSeconds(1);
        protected void EnsureBluesoleilStarted(BluesoleilService bluesoleil)
        {
            if (!bluesoleil.IsStarted)
            {
                bluesoleil.StartBluetooth();
                while (discoveringThread == Thread.CurrentThread && !bluesoleil.IsStarted)
                {
                    Thread.Sleep(1000);
                    bluesoleil.StartBluetooth();
                }
            }
        }

        protected void Discovering()
        {
            while (discoveringThread == Thread.CurrentThread)
            {
                MsHid.NativeMethods.WSAStartup();

                bluesoleil = BluesoleilService.Instance;
                bluesoleil.Initialize();
                bluesoleil.ConnectionClosed += OnConnectionClosed;

                while (discoveringThread == Thread.CurrentThread)
                {
                    EnsureBluesoleilStarted(bluesoleil);

                    BluetoothDevice[] devices;
                    try
                    {
                        // Scan for bluetooth-devices (like devices).
                        devices = bluesoleil.InquireDevices(pollingTime);
                    }
                    catch (BluesoleilFailException)
                    {
                        // Happens sometimes randomly, but also happens sometimes when the bluetooth-dongle is unplugged.
                        continue;
                    }
                    catch (BluesoleilNotReadyException)
                    {
                        // Happens when bluetooth is stopped or when the bluetooth-dongle is unplugged.
                        Thread.Sleep(5000);
                        continue;
                    }

                    List<BluetoothAddress> notFoundAddresses = new List<BluetoothAddress>(lookupDeviceInfo.Keys);
                    foreach (BluetoothDevice device in devices)
                    {
                        if (!IsWiiDevice(device))
                            continue;
                        BluetoothAddress address = new BluetoothAddress(device.Address);
                        if (lookupDeviceInfo.ContainsKey(address))
                        {
                            notFoundAddresses.Remove(address);
                            break;
                        }

                        BluetoothService[] services = null;
                        try
                        {
                            // Scan for bluetooth-devices (like devices).
                            services = bluesoleil.BrowseServices(device);
                            Thread.Sleep(1000);
                        }
                        catch (BluesoleilFailException)
                        {
                            // Happens sometimes randomly, but also happens sometimes when the bluetooth-dongle is unplugged.
                            continue;
                        }
                        catch (BluesoleilNotReadyException)
                        {
                            // Happens when bluetooth is stopped or when the bluetooth-dongle is unplugged.
                            continue;
                        }

                        if (services.Length != 3)
                            continue;

                        if (!lookupDeviceInfo.ContainsKey(address))
                        {
                            BluesoleilDeviceInfo foundDevice = new BluesoleilDeviceInfo(device, services[1]);
                            OnDeviceFound(foundDevice);
                        }
                    }

                    // Remove the lost devices from the list and notify DeviceLost event.
                    foreach (BluetoothAddress notFoundAddress in notFoundAddresses)
                    {
                        BluesoleilDeviceInfo notFoundDeviceInfo = lookupDeviceInfo[notFoundAddress];
                        OnDeviceLost(notFoundDeviceInfo);
                    }

                    Thread.Sleep(1000);
                }
                bluesoleil.ConnectionClosed -= OnConnectionClosed;
                bluesoleil.Dispose();
            }
        }

        private bool IsWiiDevice(BluetoothDevice device)
        {
            if (device.Name == "Nintendo RVL-CNT-01")
                return true;
            else if (device.Name == "Nintendo RVL-WBC-01")
                return true;
            return false;
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            BluesoleilDeviceInfo bluetoothDeviceInfo = (BluesoleilDeviceInfo)deviceInfo;
            Thread.Sleep(100);

            BluetoothConnection connection = BluesoleilService.Instance.ConnectService(bluetoothDeviceInfo.Service);

            ReportDevice device = null;
            foreach (KeyValuePair<string, SafeFileHandle> pair in MsHidDeviceProviderHelper.GetWiiDeviceHandles())
            {
                string devicePath = pair.Key;
                SafeFileHandle fileHandle = pair.Value;
                Stream communicationStream = new MsHidStream(fileHandle);

                // determine the device type
                if (bluetoothDeviceInfo.Name == "Nintendo RVL-WBC-01")
                    device = new ReportBalanceBoard(deviceInfo, communicationStream);
                else if (bluetoothDeviceInfo.Name == "Nintendo RVL-CNT-01")
                    device = new ReportWiimote(deviceInfo, communicationStream);
                else
                    throw new ArgumentException("The specified deviceInfo with name '" + bluetoothDeviceInfo.Name + "' is not supported.", "deviceInfo");

                if (MsHidDeviceProviderHelper.TryConnect(device, communicationStream, devicePath, fileHandle))
                    break;
                device = null;
            }
            if (device == null)
            {
                bluesoleil.DisconnectService(connection);
                throw new DeviceConnectException("The connected bluetooth device was not found in the HID-list.");
            }

            device.Disconnected += new EventHandler(device_Disconnected);
            lookupConnection.Add(bluetoothDeviceInfo.Address, connection);
            OnDeviceConnected(device);
            return device;
        }

        void device_Disconnected(object sender, EventArgs e)
        {
            ReportDevice device = (ReportDevice)sender;
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)device.DeviceInfo;
            BluetoothConnection connection;
            if (lookupConnection.TryGetValue(deviceInfo.Address, out connection))
            {
                lookupConnection.Remove(deviceInfo.Address);
                OnDeviceDisconnected(device);
                try
                {
                    bluesoleil.DisconnectService(connection);
                }
                catch (BluesoleilNonExistingConnectionException)
                {
                }

                MsHidDeviceProviderHelper.SetDevicePathConnected(deviceInfo.DevicePath, false);
            }
        }

        private void OnConnectionClosed(object sender, BluetoothConnectionEventArgs e)
        {
            BluetoothAddress address = new BluetoothAddress(e.BluetoothConnection.Service.Device.Address);
            ReportDevice device;
            if (lookupDevice.TryGetValue(address, out device))
            {
                device.Disconnect();
            }
        }

        private void OnDeviceConnected(ReportDevice device)
        {
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)device.DeviceInfo;

            OnDeviceLost(deviceInfo);
            lookupDevice.Add(deviceInfo.Address, device);
            connectedDevices.Add(device);
            OnDeviceConnected(new DeviceEventArgs(device));
        }

        private void OnDeviceDisconnected(ReportDevice device)
        {
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)device.DeviceInfo;

            lookupDevice.Remove(deviceInfo.Address);
            connectedDevices.Remove(device);
            OnDeviceDisconnected(new DeviceEventArgs(device));
        }

        private void OnDeviceFound(BluesoleilDeviceInfo deviceInfo)
        {
            lookupDeviceInfo.Add(deviceInfo.Address, deviceInfo);
            foundDevices.Add(deviceInfo);

            OnDeviceFound(new DeviceInfoEventArgs(deviceInfo));
        }

        private void OnDeviceLost(BluesoleilDeviceInfo deviceInfo)
        {
            lookupDeviceInfo.Remove(deviceInfo.Address);
            foundDevices.Remove(deviceInfo);

            OnDeviceLost(new DeviceInfoEventArgs(deviceInfo));
        }

        #region Events
        #region DeviceConnected Event
        protected virtual void OnDeviceConnected(DeviceEventArgs e)
        {
            if (DeviceConnected == null)
                return;
            DeviceConnected(this, e);
        }
        public event EventHandler<DeviceEventArgs> DeviceConnected;
        #endregion
        #region DeviceDisconnected Event
        protected virtual void OnDeviceDisconnected(DeviceEventArgs e)
        {
            if (DeviceDisconnected == null)
                return;
            DeviceDisconnected(this, e);
        }
        public event EventHandler<DeviceEventArgs> DeviceDisconnected;
        #endregion
        #region DeviceFound Event
        protected virtual void OnDeviceFound(DeviceInfoEventArgs e)
        {
            if (DeviceFound == null)
                return;
            DeviceFound(this, e);
        }
        public event EventHandler<DeviceInfoEventArgs> DeviceFound;
        #endregion
        #region DeviceLost Event
        protected virtual void OnDeviceLost(DeviceInfoEventArgs e)
        {
            if (DeviceLost == null)
                return;
            DeviceLost(this, e);
        }
        public event EventHandler<DeviceInfoEventArgs> DeviceLost;
        #endregion
        #endregion

    }
}
