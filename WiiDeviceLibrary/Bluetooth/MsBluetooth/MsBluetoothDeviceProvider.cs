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

        private IDictionary<BluetoothAddress, MsBluetoothDeviceInfo> lookupFoundWiimotes = new Dictionary<BluetoothAddress, MsBluetoothDeviceInfo>();
        private ICollection<IDeviceInfo> foundDevices = new List<IDeviceInfo>();
        public ICollection<IDeviceInfo> FoundDevices
        {
            get { return foundDevices; }
        }

        private IDictionary<MsBluetoothDeviceInfo, ReportWiimote> lookupConnectedWiimotes = new Dictionary<MsBluetoothDeviceInfo, ReportWiimote>();
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

        private static bool IsWiimoteDevice(NativeMethods.BluetoothDeviceInfo deviceInfo)
        {
            if (deviceInfo.name != "Nintendo RVL-CNT-01")
                return false;
            return true;
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
                List<BluetoothAddress> notFoundAddresses = new List<BluetoothAddress>(lookupFoundWiimotes.Keys);
                foreach (ReportWiimote wiimote in lookupConnectedWiimotes.Values)
                {
                    notFoundAddresses.Remove(((MsBluetoothDeviceInfo)wiimote.DeviceInfo).BluetoothAddress);
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
                    if (!IsWiimoteDevice(deviceInfo))
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
                    MsBluetoothDeviceInfo wiimoteInfo;
                    if (lookupFoundWiimotes.TryGetValue(address, out wiimoteInfo))
                    {
                        wiimoteInfo.Device = deviceInfo;
                    }
                    else
                    {
                        wiimoteInfo = new MsBluetoothDeviceInfo(address, deviceInfo);
                        FoundDevices.Add(wiimoteInfo);
                        lookupFoundWiimotes.Add(address, wiimoteInfo);
                        OnWiimoteFound(new DeviceInfoEventArgs(wiimoteInfo));
                    }
                }

                // Remove the lost wiimotes from the list and notify WiimoteLost event.
                foreach (BluetoothAddress notFoundAddress in notFoundAddresses)
                {
                    IDeviceInfo notFoundWiimoteInfo = lookupFoundWiimotes[notFoundAddress];
                    lookupFoundWiimotes.Remove(notFoundAddress);
                    foundDevices.Remove(notFoundWiimoteInfo);
                    OnWiimoteLost(new DeviceInfoEventArgs(notFoundWiimoteInfo));
                }
                Thread.Sleep(1000);
            }
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            int result;

            MsBluetoothDeviceInfo wiimoteInfo = deviceInfo as MsBluetoothDeviceInfo;
            if (wiimoteInfo == null)
                throw new ArgumentException("The specified IWiimoteInfo does not belong to this WiimoteProvider.", "deviceInfo");

            NativeMethods.BluetoothDeviceInfo wiimoteDevice = wiimoteInfo.Device;

            result = NativeMethods.BluetoothUpdateDeviceRecord(ref wiimoteDevice);
            NativeMethods.HandleError(result);

            if (wiimoteDevice.connected)
                throw new NotImplementedException("The device is already connected.");
            if (wiimoteDevice.remembered)
            {
                // Remove non-connected wiimotes from MsBluetooth's device list.
                // This has to be done because:
                //     MsBluetooth can't connect to Hid devices without also pairing to them.
                // If you think that sounds crazy, you're on the right track.
                NativeMethods.RemoveDevice(wiimoteDevice.address);
            }

            Guid hidGuid = BluetoothServices.HumanInterfaceDeviceServiceClass_UUID;
            result = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref wiimoteDevice, ref hidGuid, 0x0001);
            NativeMethods.HandleError(result);

            if (WaitTillConnected(wiimoteDevice.address, TimeSpan.FromSeconds(30)))
            {
                Thread.Sleep(2000);

                ReportWiimote wiimote = null;
                foreach (KeyValuePair<string, SafeFileHandle> pair in MsHidDeviceProviderHelper.GetWiiDeviceHandles())
                {
                    string devicePath = pair.Key;
                    SafeFileHandle fileHandle = pair.Value;
                    Stream communicationStream = new MsHidSetOutputReportStream(fileHandle);
                    wiimote = new ReportWiimote(deviceInfo, communicationStream);
                    if (MsHidDeviceProviderHelper.TryConnect(wiimote, communicationStream, devicePath, fileHandle))
                        break;
                    wiimote = null;
                }
                if (wiimote != null)
                {
                    lookupFoundWiimotes.Remove(wiimoteInfo.BluetoothAddress);
                    foundDevices.Remove(wiimoteInfo);
                    OnWiimoteLost(new DeviceInfoEventArgs(wiimoteInfo));

                    wiimote.Disconnected += wiimote_Disconnected;
                    ConnectedDevices.Add(wiimote);
                    lookupConnectedWiimotes.Add(wiimoteInfo, wiimote);
                    OnWiimoteConnected(new DeviceEventArgs(wiimote));
                    return wiimote;
                }
                else
                    throw new DeviceConnectException("No working HID device found.");
            }
            else
            {
                throw new TimeoutException("Timeout while trying to connect to the bluetooth device.");
            }
        }

        private void wiimote_Disconnected(object sender, EventArgs e)
        {
            IDevice device = (IDevice)sender;
            MsBluetoothDeviceInfo deviceInfo = (MsBluetoothDeviceInfo)device.DeviceInfo;
            device.Disconnected -= wiimote_Disconnected;
            connectedDevices.Remove(device);
            lookupConnectedWiimotes.Remove(deviceInfo);

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

            OnWiimoteDisconnected(new DeviceEventArgs(device));
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
