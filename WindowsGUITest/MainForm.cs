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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WiiDeviceLibrary;

namespace WindowsGUITest
{
    public partial class MainForm : Form
    {
        #region Fields
        private System.Windows.Forms.Timer idleTimer;

        private ICollection<UIDevice> devices = new List<UIDevice>();
        private IDictionary<IDevice, UIDevice> deviceLookup = new Dictionary<IDevice, UIDevice>();
        private IDictionary<IDeviceInfo, UIDevice> deviceinfoLookup = new Dictionary<IDeviceInfo, UIDevice>();

        private IDeviceProvider _Provider;
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        public UIDevice Add(IDeviceInfo deviceInfo)
        {
            UIDevice uiDevice;
            if (!deviceinfoLookup.TryGetValue(deviceInfo, out uiDevice))
            {
                uiDevice = new UIDevice();
                uiDevice.DeviceInfo = deviceInfo;
                deviceinfoLookup.Add(deviceInfo, uiDevice);
                devices.Add(uiDevice);

                Invoke(new Action<UIDevice>(delegate(UIDevice ud)
                {
                    devicesBox.Items.Add(ud);
                    devicesBox.SelectedIndex = devicesBox.Items.Count - 1;
                }), uiDevice);
            }
            else
                uiDevice.DeviceInfo = deviceInfo;
            devicesBox.Invalidate();
            return uiDevice;
        }

        public UIDevice Add(IDevice device)
        {
            UIDevice uiDevice = Add(device.DeviceInfo);
            uiDevice.Device = device;

            devicesBox.Invalidate();
            devicesBox.Update();
            return uiDevice;
        }

        public void Remove(IDeviceInfo deviceInfo)
        {
            UIDevice uiDevice;
            if (deviceinfoLookup.TryGetValue(deviceInfo, out uiDevice))
            {
                if (uiDevice.Device == null)
                {
                    devices.Remove(uiDevice);
                    deviceinfoLookup.Remove(deviceInfo);

                    Invoke(new Action<UIDevice>(delegate(UIDevice ud)
                    {
                        devicesBox.Items.Remove(ud);
                    }), uiDevice);
                }
            }
            devicesBox.Invalidate();
        }

        public void Remove(IDevice device)
        {
            UIDevice uiDevice;
            if (deviceLookup.TryGetValue(device, out uiDevice))
            {
                devices.Remove(uiDevice);
                deviceinfoLookup.Remove(uiDevice.DeviceInfo);
                deviceLookup.Remove(device);

                Invoke(new Action<UIDevice>(delegate(UIDevice ud)
                {
                    if (ud.Control != null)
                        wiidevicePanel.Controls.Remove(ud.Control);
                    devicesBox.Items.Remove(ud);
                }), uiDevice);
            }
            devicesBox.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            idleTimer = new System.Windows.Forms.Timer();
            idleTimer.Tick += idleTimer_Tick;
            idleTimer.Interval = 10;
            idleTimer.Start();

            _Provider = DeviceProviderRegistry.CreateSupportedDeviceProvider();

            _Provider.DeviceFound += DeviceFound;
            _Provider.DeviceLost += DeviceLost;

            _Provider.StartDiscovering();
        }

        void idleTimer_Tick(object sender, EventArgs e)
        {
            Application.RaiseIdle(EventArgs.Empty);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _Provider.DeviceFound -= DeviceFound;
            _Provider.DeviceLost -= DeviceLost;
            _Provider.StopDiscovering();
        }

        private void DeviceFound(object sender, DeviceInfoEventArgs args)
        {
            UIDevice uiDevice = Add(args.DeviceInfo);
            if (autoconnectBox.Checked)
                Connect(uiDevice);
        }

        private void DeviceLost(object sender, DeviceInfoEventArgs args)
		{
            Remove(args.DeviceInfo);
		}

        //private void Updating()
        //{
        //    while (_UpdateThread != Thread.CurrentThread)
        //    {
        //        // Invoke Update.
        //        Thread.Sleep(20);
        //    }
        //}

        private void connectButton_Click(object sender, EventArgs e)
        {
            UIDevice uiDevice = devicesBox.SelectedItem as UIDevice;
            Connect(uiDevice);
            devicesBox.Invalidate();
        }

        public IDevice Connect(UIDevice uiDevice)
        {
            IDevice device = _Provider.Connect(uiDevice.DeviceInfo);
            device.Disconnected += new EventHandler(device_Disconnected);
            if (device is IWiimote)
            {

                ((IWiimote)device).Leds = WiimoteLeds.Led1;
                ((IWiimote)device).IsRumbling = true;
                System.Threading.Thread.Sleep(500);
                ((IWiimote)device).IsRumbling = false;
            }
            uiDevice.Device = device;
            deviceLookup[device] = uiDevice;
            if (device is IWiimote)
            {
                Invoke(new Action<IWiimote>(delegate(IWiimote wiimote)
                {
                    WiimoteUserControl ucontrol = new WiimoteUserControl();
                    ucontrol.Wiimote = wiimote;
                    uiDevice.Control = ucontrol;
                    wiidevicePanel.Controls.Add(ucontrol);
                }), device);
            }
            else if (device is IBalanceBoard)
            {
                Invoke(new Action<IBalanceBoard>(delegate(IBalanceBoard balanceBoard)
                {
                    BalanceBoardUserControl ucontrol = new BalanceBoardUserControl();
                    ucontrol.BalanceBoard = balanceBoard;
                    uiDevice.Control = ucontrol;
                    wiidevicePanel.Controls.Add(ucontrol);
                }), device);
            }

            return device;
        }

        void device_Disconnected(object sender, EventArgs e)
        {
            IDevice device = (IDevice)sender;
            Remove(device);
        }

        private void devicesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            connectButton.Enabled = !autoconnectBox.Checked && devicesBox.SelectedItems.Count > 0;
        }
    }

    public class UIDevice
    {
        private IDeviceInfo _DeviceInfo;
        public IDeviceInfo DeviceInfo
        {
            get { return _DeviceInfo; }
            set { _DeviceInfo = value; }
        }

        private IDevice _Device;
        public IDevice Device
        {
            get { return _Device; }
            set { _Device = value; }
        }

        private Control _Control;
        public Control Control
        {
            get { return _Control; }
            set { _Control = value; }
        }

        public override string ToString()
        {
            StringBuilder sbuilder = new StringBuilder();
            if (DeviceInfo is IBluetoothDeviceInfo)
                sbuilder.Append(((IBluetoothDeviceInfo)DeviceInfo).BluetoothAddress);
            else
                sbuilder.Append(DeviceInfo.ToString());
            if (Device != null)
            {
                sbuilder.Append(" (");
                sbuilder.Append(Device.ToString());
                sbuilder.Append(")");
            }
            return sbuilder.ToString();
        }
    }
}