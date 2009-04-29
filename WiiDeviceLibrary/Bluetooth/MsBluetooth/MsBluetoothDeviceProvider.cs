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
using System.Diagnostics;
using WiiDeviceLibrary.Bluetooth.MsHid;
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth.MsBluetooth
{
    public class MsBluetoothDeviceProvider : IDeviceProvider
    {
        private Thread discoveringThread;

        private IDictionary<BluetoothAddress, MsBluetoothDeviceInfo> lookupFoundDevices = new Dictionary<BluetoothAddress, MsBluetoothDeviceInfo>();
        private ICollection<IDeviceInfo> foundDevices = new List<IDeviceInfo>();
        public ICollection<IDeviceInfo> FoundDevices
        {
            get { return foundDevices; }
        }

        private IDictionary<MsBluetoothDeviceInfo, ReportDevice> lookupConnectedDevices = new Dictionary<MsBluetoothDeviceInfo, ReportDevice>();
        private ICollection<IDevice> connectedDevices = new List<IDevice>();
        public ICollection<IDevice> ConnectedDevices
        {
            get { return connectedDevices; }
        }

        public bool IsDiscovering
        {
            get { return discoveringThread != null; }
        }

        public MsBluetoothDeviceProvider()
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

        private static bool IsWiiDevice(NativeMethods.BluetoothDeviceInfo deviceInfo)
        {
            if (deviceInfo.name == "Nintendo RVL-CNT-01")
                return true;
            else if (deviceInfo.name == "Nintendo RVL-WBC-01")
                return true;
            return false;
        }

        internal static bool WaitTillConnected(NativeMethods.BluetoothAddress bluetoothAddress, TimeSpan timeout)
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime + timeout;
            bool connected = false;
            while (!connected)
            {
                foreach (NativeMethods.BluetoothDeviceInfo deviceInfo in NativeMethods.GetDeviceInfos(false, true, false, false, false, 1))
                {
                    if (!deviceInfo.connected)
                        continue;
                    if (!deviceInfo.address.Equals(bluetoothAddress))
                        continue;
                    connected = true;
                    break;
                }
                if (DateTime.Now > endTime)
                    return false;
                Thread.Sleep(1);
            }
            return true;
        }

        protected void Discovering()
        {
            MsHid.NativeMethods.WSAStartup();
            while (discoveringThread == Thread.CurrentThread)
            {
                List<BluetoothAddress> notFoundAddresses = new List<BluetoothAddress>(lookupFoundDevices.Keys);
                foreach (ReportDevice device in lookupConnectedDevices.Values)
                {
                    notFoundAddresses.Remove(((MsBluetoothDeviceInfo)device.DeviceInfo).Address);
                }

                IEnumerable<NativeMethods.BluetoothDeviceInfo> devices;
                try
                {
                    devices = NativeMethods.GetDeviceInfos(true, true, true, true, true, 2);
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    if (e.ErrorCode == -2147467259)
                    {
                        // TODO: This should be checked at the constructor or StartDiscovering, if possible...
                        throw new InvalidOperationException("No bluetooth adapter was found.");
                    }
                    else
                        throw;
                }
                foreach (NativeMethods.BluetoothDeviceInfo deviceInfo in devices)
                {
                    if (!IsWiiDevice(deviceInfo))
                        continue;
                    if (deviceInfo.connected)
                        continue;
                    if (deviceInfo.remembered)
                    {
                        NativeMethods.RemoveDevice(deviceInfo.address);
                        continue;
                    }

                    BluetoothAddress address = new BluetoothAddress(deviceInfo.address.address);
                    notFoundAddresses.Remove(address);
                    MsBluetoothDeviceInfo bluetoothDeviceInfo;
                    if (lookupFoundDevices.TryGetValue(address, out bluetoothDeviceInfo))
                    {
                        bluetoothDeviceInfo.Device = deviceInfo;
                    }
                    else
                    {
                        bluetoothDeviceInfo = new MsBluetoothDeviceInfo(address, deviceInfo);
                        FoundDevices.Add(bluetoothDeviceInfo);
                        lookupFoundDevices.Add(address, bluetoothDeviceInfo);
                        OnDeviceFound(new DeviceInfoEventArgs(bluetoothDeviceInfo));
                    }
                }

                // Remove the lost devices from the list and notify DeviceLost event.
                foreach (BluetoothAddress notFoundAddress in notFoundAddresses)
                {
                    IDeviceInfo notFoundDeviceInfo = lookupFoundDevices[notFoundAddress];
                    lookupFoundDevices.Remove(notFoundAddress);
                    foundDevices.Remove(notFoundDeviceInfo);
                    OnDeviceLost(new DeviceInfoEventArgs(notFoundDeviceInfo));
                }
                Thread.Sleep(1000);
            }
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            int result;

            MsBluetoothDeviceInfo bluetoothDeviceInfo = deviceInfo as MsBluetoothDeviceInfo;
            if (bluetoothDeviceInfo == null)
                throw new ArgumentException("The specified IDeviceInfo does not belong to this DeviceProvider.", "deviceInfo");

            NativeMethods.BluetoothDeviceInfo bluetoothDevice = bluetoothDeviceInfo.Device;

            result = NativeMethods.BluetoothUpdateDeviceRecord(ref bluetoothDevice);
            NativeMethods.HandleError(result);

            if (bluetoothDevice.connected)
                throw new NotImplementedException("The device is already connected.");
            if (bluetoothDevice.remembered)
            {
                // Remove non-connected devices from MsBluetooth's device list.
                // This has to be done because:
                //     MsBluetooth can't connect to Hid devices without also pairing to them.
                // If you think that sounds crazy, you're on the right track.
                NativeMethods.RemoveDevice(bluetoothDevice.address);
            }

            Guid hidGuid = BluetoothServices.HumanInterfaceDeviceServiceClass_UUID;
            result = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref bluetoothDevice, ref hidGuid, 0x0001);
            NativeMethods.HandleError(result);

            if (WaitTillConnected(bluetoothDevice.address, TimeSpan.FromSeconds(30)))
            {
                Thread.Sleep(2000);

                ReportDevice device = null;
                foreach (KeyValuePair<string, SafeFileHandle> pair in MsHidDeviceProviderHelper.GetWiiDeviceHandles())
                {
                    string devicePath = pair.Key;
                    SafeFileHandle fileHandle = pair.Value;
                    Stream communicationStream = new MsHidSetOutputReportStream(fileHandle);

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
                if (device != null)
                {
                    lookupFoundDevices.Remove(bluetoothDeviceInfo.Address);
                    foundDevices.Remove(bluetoothDeviceInfo);
                    OnDeviceLost(new DeviceInfoEventArgs(bluetoothDeviceInfo));

                    device.Disconnected += device_Disconnected;
                    ConnectedDevices.Add(device);
                    lookupConnectedDevices.Add(bluetoothDeviceInfo, device);
                    OnDeviceConnected(new DeviceEventArgs(device));
                    return device;
                }
                else
                    throw new DeviceConnectException("No working HID device found.");
            }
            else
            {
                throw new TimeoutException("Timeout while trying to connect to the bluetooth device.");
            }
        }

        private void device_Disconnected(object sender, EventArgs e)
        {
            IDevice device = (IDevice)sender;
            MsBluetoothDeviceInfo deviceInfo = (MsBluetoothDeviceInfo)device.DeviceInfo;
            device.Disconnected -= device_Disconnected;
            connectedDevices.Remove(device);
            lookupConnectedDevices.Remove(deviceInfo);

            // The actual disconnecting on bluetooth-level.
            NativeMethods.BluetoothDeviceInfo bluetoothDeviceInfo = deviceInfo.Device;
            Guid hidGuid = BluetoothServices.HumanInterfaceDeviceServiceClass_UUID;
            int result = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref bluetoothDeviceInfo, ref hidGuid, 0x0000);
            if (result != 0)
                Debug.WriteLine(string.Format("BluetoothSetServiceState returned 0x{0:x}.", result));
            // Also remove the device from the bluetooth-devices list, so
            // that doesn't have to be done when connecting again.
            NativeMethods.RemoveDevice(deviceInfo.Device.address);

            MsHidDeviceProviderHelper.SetDevicePathConnected(deviceInfo.DevicePath, false);

            OnDeviceDisconnected(new DeviceEventArgs(device));
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
