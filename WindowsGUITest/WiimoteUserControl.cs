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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WiiDeviceLibrary;
using WiiDeviceLibrary.Extensions;
using System.IO;

namespace WindowsGUITest
{
    public partial class WiimoteUserControl : UserControl
    {
        private IWiimote _Wiimote;
        public IWiimote Wiimote
        {
            get { return _Wiimote; }
            set
            {
                if (_Wiimote != value)
                {
                    if (_Wiimote != null)
                        DeinitializeWiimote();
                    _Wiimote = value;
                    if (_Wiimote != null)
                        InitializeWiimote();
                }
            }
        }

        private IExtensionControl _ExtensionControl;
        public IExtensionControl ExtensionControl
        {
            get { return _ExtensionControl; }
            protected set { _ExtensionControl = value; }
        }

        private void DeinitializeWiimote()
        {
            Wiimote.ExtensionAttached -= Wiimote_ExtensionAttached;
            Wiimote.ExtensionDetached -= Wiimote_ExtensionDetached;
        }

        private void InitializeWiimote()
        {
            if (!IsHandleCreated)
                CreateControl();
            reportingmodeBox.SelectedItem = Wiimote.ReportingMode;
            led1Check.Checked = (Wiimote.Leds & WiimoteLeds.Led1) == WiimoteLeds.Led1;
            led2Check.Checked = (Wiimote.Leds & WiimoteLeds.Led2) == WiimoteLeds.Led2;
            led3Check.Checked = (Wiimote.Leds & WiimoteLeds.Led3) == WiimoteLeds.Led3;
            led4Check.Checked = (Wiimote.Leds & WiimoteLeds.Led4) == WiimoteLeds.Led4;
            rumbleCheck.Checked = Wiimote.IsRumbling;

            if (Wiimote.Extension != null)
                Wiimote_ExtensionAttached(Wiimote, new WiimoteExtensionEventArgs(Wiimote.Extension));

            Wiimote.Updated += Wiimote_Updated;
            Wiimote.ExtensionAttached += Wiimote_ExtensionAttached;
            Wiimote.ExtensionDetached += Wiimote_ExtensionDetached;
        }

        void Wiimote_Updated(object sender, EventArgs e)
        {
            if (Wiimote != null)
            {
                irBox.Invalidate();
            }
        }

        void Wiimote_ExtensionAttached(object sender, WiimoteExtensionEventArgs e)
        {
            IWiimoteExtension wiimoteExtension = Wiimote.Extension;
            Invoke(new Action<IWiimoteExtension>(delegate(IWiimoteExtension extension)
            {
                Control extensionControl = null;
                if (extension is NunchukExtension)
                {
                    NunchukUserControl nunchukUC = new NunchukUserControl();
                    nunchukUC.Nunchuk = (NunchukExtension)extension;
                    extensionControl = nunchukUC;
                } 
                else if (extension is ClassicControllerExtension)
                {
                    ClassicControllerUserControl classicControllerUC = new ClassicControllerUserControl();
                    classicControllerUC.ClassicController = (ClassicControllerExtension)extension;
                    extensionControl = classicControllerUC;
                }
                else if (extension is GuitarExtension)
                {
                    GuitarUserControl guitarUC = new GuitarUserControl();
                    guitarUC.Guitar = (GuitarExtension)extension;
                    extensionControl = guitarUC;
                }

                ExtensionControl = (IExtensionControl)extensionControl;
                if (extensionControl != null)
                {
                    extensionBox.Height = extensionControl.Height + 50;
                    extensionBox.Text = extension.GetType().Name; 
                    extensionBox.Controls.Add(extensionControl);
                    extensionControl.Dock = DockStyle.Fill;
                    this.Height = extensionBox.Top + extensionBox.Height;
                }
                reportingmodeBox.SelectedItem = Wiimote.ReportingMode;
            }), wiimoteExtension);
        }

        void Wiimote_ExtensionDetached(object sender, WiimoteExtensionEventArgs e)
        {
            Invoke(new System.Threading.ThreadStart(delegate()
            {
                while (extensionBox.Controls.Count > 0)
                {
                    Control c = extensionBox.Controls[0];
                    extensionBox.Controls.RemoveAt(0);
                    c.Dispose();
                }
                extensionBox.Text = "Extension";
                extensionBox.Height = 50;
                this.Height = extensionBox.Top + extensionBox.Height;
                reportingmodeBox.SelectedItem = Wiimote.ReportingMode;
            }));
        }

        public WiimoteUserControl()
        {
            InitializeComponent();

            Array reportingModeArray = Enum.GetValues(typeof(ReportingMode));
            object[] reportingModeValues = new object[reportingModeArray.Length];
            for (int i = 0; i < reportingModeArray.Length; i++)
            {
                reportingModeValues[i] = reportingModeArray.GetValue(i);
            }
            reportingmodeBox.Items.AddRange(reportingModeValues);
        }

        private void reportingmodeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportingMode selectedReportingMode = (ReportingMode)reportingmodeBox.SelectedItem;
            if (Wiimote.ReportingMode != selectedReportingMode)
                Wiimote.SetReportingMode(selectedReportingMode);
        }

        private void rumbleCheck_CheckedChanged(object sender, EventArgs e)
        {
            Wiimote.IsRumbling = rumbleCheck.Checked;
        }

        private void ledCheck_CheckedChanged(object sender, EventArgs e)
        {
            Wiimote.Leds =
                (led1Check.Checked ? WiimoteLeds.Led1 : WiimoteLeds.None) |
                (led2Check.Checked ? WiimoteLeds.Led2 : WiimoteLeds.None) |
                (led3Check.Checked ? WiimoteLeds.Led3 : WiimoteLeds.None) |
                (led4Check.Checked ? WiimoteLeds.Led4 : WiimoteLeds.None);
        }

        private void updateBatteryButton_Click(object sender, EventArgs e)
        {
            Wiimote.UpdateStatus();
        }

        public void UpdateUI()
        {
            if (ExtensionControl != null)
                ExtensionControl.UpdateUI();
            buttonsBox.Text = Wiimote.Buttons.ToString();
            accelerometerXBox.Text = Wiimote.Accelerometer.Calibrated.X.ToString();
            accelerometerYBox.Text = Wiimote.Accelerometer.Calibrated.Y.ToString();
            accelerometerZBox.Text = Wiimote.Accelerometer.Calibrated.Z.ToString();
            batteryBar.Value = Wiimote.BatteryLevel;
        }

        private void irBox_Paint(object sender, PaintEventArgs e)
        {
            UpdateUI();

            Graphics g = e.Graphics;
            int width = irBox.Width;
            int height = irBox.Height;
            float scaleFactor = (float)width / 1024f;
            foreach (BasicIRBeacon irBeacon in Wiimote.IRBeacons)
            {
                if (irBeacon == null)
                    continue;
                Pen p = new Pen(Brushes.White);
                g.DrawRectangle(p, width - (int)(scaleFactor * irBeacon.X - 2), (int)(scaleFactor * irBeacon.Y - 2), 4, 4);
            }
        }

        private void calibrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IWiimote wiimote = Wiimote;
            ReportingMode oldReportingMode = wiimote.ReportingMode;
            wiimote.SetReportingMode(ReportingMode.ButtonsAccelerometer);
            AccelerometerAxes<ushort> raw = wiimote.Accelerometer.Raw;
            if (MessageBox.Show("Place the wiimote on a table.", "Calibration", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                ushort xZero, yZero, zZero, xOne, yOne, zOne = 0;
                xZero = raw.X;
                yZero = raw.Y;
                zOne = raw.Z;
                if (MessageBox.Show("Place the wiimote on its left side.", "Calibration", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    xOne = raw.X;
                    zZero = raw.Z;

                    // Invert zOne (so that the values are negated).
                    zOne = (ushort)(zZero - (zOne - zZero));

                    if (MessageBox.Show("Place the wiimote on its lower side, so that it points up.", "Calibration", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        yOne = raw.Y;

                        wiimote.WriteMemory(0x16, new byte[] {
                            (byte)xZero, (byte)yZero, (byte)zZero, 0,
                            (byte)xOne, (byte)yOne, (byte)zOne, 0 }, 0, 8);

                        wiimote.Initialize();
                    }
                }
            }
            wiimote.SetReportingMode(oldReportingMode);
        }

        private void dumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (Stream fileStream = File.Create(saveFileDialog.FileName))
                {
                    byte[] memoryBuffer = new byte[0x1700];
                    Wiimote.ReadMemory(0x000000, memoryBuffer, 0, (short)memoryBuffer.Length);
                    fileStream.Write(memoryBuffer, 0, memoryBuffer.Length);
                    fileStream.Close();
                }
            }
        }

        private void playSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int readBytes;
            using (Stream pcmStream = new FileStream(@"Audio.au", FileMode.Open))
            {
                using (Stream adpcmStream = new Pcm8ToAdpcm4Stream(pcmStream))
                {
                    using (Stream speakerStream = new WiimoteSpeakerStream(_Wiimote))
                    {

                        byte[] adpcm_data = new byte[20];

                        Wiimote.IsSpeakerEnabled = true;

                        while ((readBytes = adpcmStream.Read(adpcm_data, 0, 20)) > 0)
                            speakerStream.Write(adpcm_data, 0, readBytes);

                        Wiimote.IsSpeakerEnabled = false;

                        speakerStream.Close();
                    }
                    adpcmStream.Close();
                }
                pcmStream.Close();
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wiimote.Disconnect();
        }
    }
}
