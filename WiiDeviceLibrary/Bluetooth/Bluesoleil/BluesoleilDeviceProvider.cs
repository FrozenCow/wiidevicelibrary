//    Copyright 2008 Wii Device Library authors
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
        private IDictionary<BluetoothAddress, ReportWiimote> lookupDevice = new Dictionary<BluetoothAddress, ReportWiimote>();
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
                        // Scan for bluetooth-devices (like wiimotes).
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
                        BluetoothAddress address = new BluetoothAddress(device.Address);
                        if (lookupDeviceInfo.ContainsKey(address))
                        {
                            notFoundAddresses.Remove(address);
                            break;
                        }

                        BluetoothService[] services = null;
                        try
                        {
                            // Scan for bluetooth-devices (like wiimotes).
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
                            BluesoleilDeviceInfo foundWiimote = new BluesoleilDeviceInfo(device, services[1]);
                            OnWiimoteFound(foundWiimote);
                        }
                    }

                    // Remove the lost wiimotes from the list and notify WiimoteLost event.
                    foreach (BluetoothAddress notFoundAddress in notFoundAddresses)
                    {
                        BluesoleilDeviceInfo notFoundWiimoteInfo = lookupDeviceInfo[notFoundAddress];
                        OnWiimoteLost(notFoundWiimoteInfo);
                    }

                    Thread.Sleep(1000);
                }
                bluesoleil.ConnectionClosed -= OnConnectionClosed;
                bluesoleil.Dispose();
            }
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            BluesoleilDeviceInfo bsDeviceInfo = (BluesoleilDeviceInfo)deviceInfo;
            BluetoothConnection connection = BluesoleilService.Instance.ConnectService(bsDeviceInfo.Service);

            ReportWiimote wiimote = null;
            foreach (KeyValuePair<string, SafeFileHandle> pair in MsHidDeviceProviderHelper.GetWiiDeviceHandles())
            {
                string devicePath = pair.Key;
                SafeFileHandle fileHandle = pair.Value;
                Stream communicationStream = new MsHidStream(fileHandle);
                wiimote = new ReportWiimote(deviceInfo, communicationStream);
                if (MsHidDeviceProviderHelper.TryConnect(wiimote, communicationStream, devicePath, fileHandle))
                    break;
                wiimote = null;
            }
            if (wiimote == null)
            {
                bluesoleil.DisconnectService(connection);
                throw new DeviceConnectException("The connected bluetooth device was not found in the HID-list.");
            }

            wiimote.Disconnected += new EventHandler(wiimote_Disconnected);
            lookupConnection.Add(bsDeviceInfo.BluetoothAddress, connection);
            OnWiimoteConnected(wiimote);
            return wiimote;
        }

        void wiimote_Disconnected(object sender, EventArgs e)
        {
            ReportWiimote wiimote = (ReportWiimote)sender;
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)wiimote.DeviceInfo;
            BluetoothConnection connection;
            if (lookupConnection.TryGetValue(deviceInfo.BluetoothAddress, out connection))
            {
                lookupConnection.Remove(deviceInfo.BluetoothAddress);
                OnWiimoteDisconnected(wiimote);
                try
                {
                    bluesoleil.DisconnectService(connection);
                }
                catch (BluesoleilException)
                {
                }

                MsHidDeviceProviderHelper.SetDevicePathConnected(deviceInfo.DevicePath, false);
            }
        }

        private void OnConnectionClosed(object sender, BluetoothConnectionEventArgs e)
        {
            BluetoothAddress address = new BluetoothAddress(e.BluetoothConnection.Service.Device.Address);
            ReportWiimote wiimote;
            if (lookupDevice.TryGetValue(address, out wiimote))
            {
                wiimote.Disconnect();
            }
        }

        private void OnWiimoteConnected(ReportWiimote device)
        {
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)device.DeviceInfo;

            OnWiimoteLost(deviceInfo);
            lookupDevice.Add(deviceInfo.BluetoothAddress, device);
            connectedDevices.Add(device);
            OnWiimoteConnected(new DeviceEventArgs(device));
        }

        private void OnWiimoteDisconnected(ReportWiimote device)
        {
            BluesoleilDeviceInfo deviceInfo = (BluesoleilDeviceInfo)device.DeviceInfo;

            lookupDevice.Remove(deviceInfo.BluetoothAddress);
            connectedDevices.Remove(device);
            OnWiimoteDisconnected(new DeviceEventArgs(device));
        }

        private void OnWiimoteFound(BluesoleilDeviceInfo deviceInfo)
        {
            lookupDeviceInfo.Add(deviceInfo.BluetoothAddress, deviceInfo);
            foundDevices.Add(deviceInfo);

            OnWiimoteFound(new DeviceInfoEventArgs(deviceInfo));
        }

        private void OnWiimoteLost(BluesoleilDeviceInfo deviceInfo)
        {
            lookupDeviceInfo.Remove(deviceInfo.BluetoothAddress);
            foundDevices.Remove(deviceInfo);

            OnWiimoteLost(new DeviceInfoEventArgs(deviceInfo));
        }

        #region Events
        #region WiimoteConnected Event
        protected virtual void OnWiimoteConnected(DeviceEventArgs e)
        {
            if (DeviceConnected == null)
                return;
            DeviceConnected(this, e);
        }
        public event EventHandler<DeviceEventArgs> DeviceConnected;
        #endregion
        #region WiimoteDisconnected Event
        protected virtual void OnWiimoteDisconnected(DeviceEventArgs e)
        {
            if (DeviceDisconnected == null)
                return;
            DeviceDisconnected(this, e);
        }
        public event EventHandler<DeviceEventArgs> DeviceDisconnected;
        #endregion
        #region WiimoteFound Event
        protected virtual void OnWiimoteFound(DeviceInfoEventArgs e)
        {
            if (DeviceFound == null)
                return;
            DeviceFound(this, e);
        }
        public event EventHandler<DeviceInfoEventArgs> DeviceFound;
        #endregion
        #region WiimoteLost Event
        protected virtual void OnWiimoteLost(DeviceInfoEventArgs e)
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
