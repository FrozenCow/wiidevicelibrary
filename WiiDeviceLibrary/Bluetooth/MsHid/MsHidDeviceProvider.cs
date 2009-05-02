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
    public class MsHidDeviceProvider: IDeviceProvider
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
	

        public bool IsDiscovering
        {
            get { return false; }
        }

        public MsHidDeviceProvider()
        {
        }

        public void StartDiscovering()
        {
            Update();
        }

        public void StopDiscovering()
        {

        }

        public void Update()
        {
            List<string> devicePaths = new List<string>(MsHidHelper.GetDevicePaths());

            foreach (string devicePath in devicePaths)
            {
                if (!_FoundDevices.ContainsKey(devicePath))
                {
                    MsHidDeviceInfo deviceInfo = new MsHidDeviceInfo(devicePath);
                    _FoundDevices.Add(devicePath, deviceInfo);
                    OnWiimoteFound(new DeviceInfoEventArgs(deviceInfo));
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
                    OnWiimoteLost(new DeviceInfoEventArgs(_FoundDevices[devicePathToBeRemoved]));
                    _FoundDevices.Remove(devicePathToBeRemoved);
                }
            }
        }

        public IDevice Connect(IDeviceInfo deviceInfo)
        {
            MsHidDeviceInfo hidWiimoteInfo = deviceInfo as MsHidDeviceInfo;
            if (hidWiimoteInfo == null)
                throw new ArgumentException("The specified WiimoteInfo does not belong to this WiimoteProvider.", "deviceInfo");
            string devicePath = hidWiimoteInfo.DevicePath;

            Stream hidStream;
            if (UseSetOutputReport)
                hidStream = new MsHidSetOutputReportStream(devicePath);
            else
                hidStream = new MsHidStream(devicePath);

            ReportWiimote wiimote = new ReportWiimote(deviceInfo, hidStream);
            ConnectedDevices.Add(wiimote);
            return wiimote;
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
