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
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public class MsHidDeviceProvider : IDeviceProvider
    {
        // VID = Nintendo, PID = Wiimote
        private const int VID = 0x057e;
        private const int PID = 0x0306;

        private IDictionary<string, IDeviceInfo> _FoundDevices = new Dictionary<string, IDeviceInfo>();
        public ICollection<IDeviceInfo> FoundDevices
        {
            get { return _FoundDevices.Values; }
        }

        private ICollection<IDevice> _ConnectedDevices = new List<IDevice>();
        public ICollection<IDevice> ConnectedDevices
        {
            get { return _ConnectedDevices; }
        }

        private bool _UseSetOutputReport = false;
        public bool UseSetOutputReport
        {
            get { return _UseSetOutputReport; }
            set { _UseSetOutputReport = value; }
        }

        private bool _IsDiscovering = false;
        public bool IsDiscovering
        {
            get { return _IsDiscovering; }
        }

        public void StartDiscovering()
        {
            Update();
        }

        public void StopDiscovering()
        {

        }

        public static IEnumerable<IDeviceInfo> GetAllDevices()
        {
            foreach (string devicePath in MsHidHelper.GetDevicePaths())
            {
                SafeFileHandle fileHandle = MsHidHelper.CreateFileHandle(devicePath);

                int vendorId, productId;
                if (MsHidHelper.TryGetHidInfo(fileHandle, out vendorId, out productId) && vendorId == VID && productId == PID)
                {
                    fileHandle.Close();
                    yield return new MsHidDeviceInfo(devicePath);
                }
            }
        }

        public void Update()
        {
            if (IsDiscovering)
                throw new InvalidOperationException("This provider is already discovering.");
            _IsDiscovering = true;
            List<string> devicePaths = new List<string>(MsHidDeviceProviderHelper.GetWiiDevicePaths());

            foreach (string devicePath in devicePaths)
            {
                if (!_FoundDevices.ContainsKey(devicePath))
                {
                    MsHidDeviceInfo deviceInfo = new MsHidDeviceInfo(devicePath);
                    _FoundDevices.Add(devicePath, deviceInfo);
                    OnDeviceFound(new DeviceInfoEventArgs(deviceInfo));
                }
            }
            if (devicePaths.Count < _FoundDevices.Count)
            {
                List<string> devicesToBeRemoved = new List<string>();
                foreach (KeyValuePair<string, IDeviceInfo> pair in _FoundDevices)
                {
                    if (!devicePaths.Contains(pair.Key))
                    {
                        devicesToBeRemoved.Add(pair.Key);
                    }
                }
                foreach (string devicePathToBeRemoved in devicesToBeRemoved)
                {
                    OnDeviceLost(new DeviceInfoEventArgs(_FoundDevices[devicePathToBeRemoved]));
                    _FoundDevices.Remove(devicePathToBeRemoved);
                }
            }
            _IsDiscovering = false;
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            MsHidDeviceInfo hidDeviceInfo = deviceInfo as MsHidDeviceInfo;
            if (hidDeviceInfo == null)
                throw new ArgumentException("The specified DeviceInfo does not belong to this DeviceProvider.", "deviceInfo");


            ReportWiimote wiimote;
            if (!TryConnect(hidDeviceInfo, out wiimote))
            {
                UseSetOutputReport = !UseSetOutputReport;
                if (!TryConnect(hidDeviceInfo, out wiimote))
                {
                    throw new DeviceConnectException("Both methods of connecting timed out.");
                }
            }
            
            wiimote.Disconnected += device_Disconnected;
            ConnectedDevices.Add(wiimote);
            MsHidDeviceProviderHelper.SetDevicePathConnected(hidDeviceInfo.DevicePath, true);

            OnDeviceConnected(new DeviceEventArgs(wiimote));
            return wiimote;
        }

        private bool TryConnect(MsHidDeviceInfo hidDeviceInfo, out ReportWiimote wiimote)
        {
            Stream hidStream;
            if (UseSetOutputReport)
                hidStream = new MsHidSetOutputReportStream(hidDeviceInfo.DevicePath);
            else
                hidStream = new MsHidStream(hidDeviceInfo.DevicePath);

            try
            {
                wiimote = new ReportWiimote(hidDeviceInfo, hidStream);
                wiimote.Initialize();
            }
            catch (TimeoutException)
            {
                hidStream.Dispose();
                wiimote = null;
                return false;
            }
            catch (Exception)
            {
                hidStream.Dispose();
                throw;
            }
            return true;
        }

        void device_Disconnected(object sender, EventArgs e)
        {
            ReportWiimote wiimote = (ReportWiimote)sender;
            MsHidDeviceInfo deviceInfo = (MsHidDeviceInfo)wiimote.DeviceInfo;
            wiimote.Disconnected -= device_Disconnected;
            ConnectedDevices.Remove(wiimote);
            MsHidDeviceProviderHelper.SetDevicePathConnected(deviceInfo.DevicePath, false);
            OnDeviceDisconnected(new DeviceEventArgs(wiimote));
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
