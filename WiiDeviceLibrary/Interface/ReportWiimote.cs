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
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace WiiDeviceLibrary
{
    public class ReportWiimote : ReportDevice, IWiimote
    {
        #region Fields
        private IRMode irMode = IRMode.Off;
        private WiimoteLeds _Leds;
        private bool _IsRumbling;
        private bool _IsSpeakerEnabled;
        private WiimoteButtons _Buttons;
        private BasicIRBeacon[] _CachedIRBeacons = new BasicIRBeacon[4];
        private BasicIRBeacon[] _IRBeacons = new BasicIRBeacon[4];
        private Accelerometer _Accelerometer;
        private IWiimoteExtension _Extension = null;
        public event EventHandler<WiimoteExtensionEventArgs> ExtensionAttached;
		public event EventHandler<WiimoteExtensionEventArgs> ExtensionDetached;
        #endregion

        #region Public Properties
        public WiimoteLeds Leds
        {
            get { return _Leds; }
            set
            {
                if (_Leds != value)
                {
                    _Leds = value;
                    SetLeds(value);
                }
            }
        }

        public bool IsRumbling
        {
            get { return _IsRumbling; }
            set
            {
                if (_IsRumbling != value)
                {
                    _IsRumbling = value;

                    SetRumble();
                }
            }
        }

        public bool IsSpeakerEnabled
        {
            get { return _IsSpeakerEnabled; }
            set
            {
                if (_IsSpeakerEnabled != value)
                {
                    _IsSpeakerEnabled = value;
                    SetSpeaker(value);
                }
            }
        }

        public byte SpeakerFrequency
        {
            get { return ReadMemory(0x04a20004); }
            set { WriteMemory(0x04a20004, value); }
        }

        public byte SpeakerVolume
        {
            get { return ReadMemory(0x04a20005); }
            set { WriteMemory(0x04a20005, value); }
        }

        public WiimoteButtons Buttons
        {
            get { return _Buttons; }
        }

        public BasicIRBeacon[] IRBeacons
        {
            get { return _IRBeacons; }
        }

        public Accelerometer Accelerometer
        {
            get { return _Accelerometer; }
        }

        public IWiimoteExtension Extension
        {
            get { return _Extension; }
        }
        #endregion

        #region Constructors
        public ReportWiimote(IDeviceInfo deviceInfo, Stream communicationStream)
            : base(deviceInfo, communicationStream)
        {
        }
        #endregion

        public override void Initialize()
        {
            UpdateStatus();
            ReadCalibrationData();
        }

        protected void ReadCalibrationData()
        {
			byte[] calibrationBytes = ReadMemory(0x16, 10);
			
            AccelerometerCalibration calibration = new AccelerometerCalibration(
                calibrationBytes[0], calibrationBytes[4],
			    calibrationBytes[1], calibrationBytes[5],
			    calibrationBytes[2], calibrationBytes[6]
                );
			
            _Accelerometer = new Accelerometer(calibration);
        }

        protected void UpdateCalibratedAccelerometer()
        {
            Accelerometer.Calibration.Calibrate(Accelerometer.Raw, Accelerometer.Calibrated);
        }

        protected override void SendReport()
        {
            OutputBuffer[1] = (byte)((OutputBuffer[1] & 0xFE) | (IsRumbling ? 1 : 0));
            base.SendReport();
        }

        public override void SetReportingMode(ReportingMode reportMode)
        {
            SetIRMode(GetIrMode((InputReport)reportMode));
            base.SetReportingMode(reportMode);
        }

        protected static IRMode GetIrMode(InputReport reportType)
        {
            switch (reportType)
            {
                case InputReport.ButtonsAccelerometer12Ir:
                    return IRMode.Extended;
                case InputReport.Buttons10Ir9Extension:
                case InputReport.ButtonsAccelerometer10Ir6Extension:
                    return IRMode.Basic;
                case InputReport.ButtonsAccelerometer36IrA:
                case InputReport.ButtonsAccelerometer36IrB:
                    return IRMode.Full;
            }
            return IRMode.Off;
        }

        public void SendSpeakerData(byte[] buffer, int offset, byte count)
        {
            if (count > 20)
                throw new ArgumentException("SpeakerData cannot be bigger than 20 bytes.");
            CreateReport(OutputReport.SendSpeakerData);
            OutputBuffer[1] = (byte)(count << 3);
            Array.Copy(buffer, offset, OutputBuffer, 2, count);
            SendReport();
        }

        #region Set Methods
        protected void SetSpeaker(bool enabled)
        {
            CreateReport(OutputReport.SetSpeaker);
            OutputBuffer[1] = (byte)(enabled ? 0x04 : 0x00);
            SendReport();

            if (IsSpeakerEnabled)
            {
                CreateReport(OutputReport.SetMute);
                OutputBuffer[1] = 0x04;
                SendReport();

                WriteMemory(0x04a20009, 0x01);
                WriteMemory(0x04a20001, 0x08);

                // TODO: Include frequency and volume as parameters.
                byte frequency = 15;
                byte volume = 0x20;

                // byte[] speakerConfiguration = new byte[] { 0x00, 0x00, 0x0C, frequency, volume, 0x00, 0x00 };
                byte[] speakerConfiguration = new byte[] { 0x00, 0x00, 0x0C, frequency, volume, 0x00, 0x00 };
                WriteMemory(0x04a20001, speakerConfiguration);
                WriteMemory(0x04a20008, 0x01);

                CreateReport(OutputReport.SetMute);
                OutputBuffer[1] = 0x00;
                SendReport();
            }
        }

        protected void SetLeds(WiimoteLeds leds)
        {
            CreateReport(OutputReport.SetLeds);
            OutputBuffer[1] = (byte)((byte)leds << 4);
            SendReport();
        }

        protected void SetRumble()
        {
            SetLeds(Leds);
        }

        public void SetIRMode(IRMode irMode)
        {
            bool enabled = (irMode != IRMode.Off);

            // Enable Pixel Clock.
            CreateReport(OutputReport.SetPixelClock);
            OutputBuffer[1] = (byte)(enabled ? 0x04 : 0x00);
            SendReport();

            // Enable IR Logic.
            CreateReport(OutputReport.SetCamera);
            OutputBuffer[1] = (byte)(enabled ? 0x04 : 0x00);
            SendReport();

            // No need to configure the camera when it is disabled.
            if (enabled)
            {
                // Configure the camera.
                WriteMemory(0x04b00030, 0x08);
                // TODO: Make sensitivity configurable.
                // Set the sensitivity of the camera.

                // Wii level 1
                //WriteData(0xb00000, new byte[] { 0x02, 0x00, 0x00, 0x71, 0x01, 0x00, 0x64, 0x00, 0xfe });
                //WriteData(0xb0001a, new byte[] { 0xfd, 0x05 });

                // Wii level 5
                //WriteData(0xb00000, new byte[] { 0x07, 0x00, 0x00, 0x71, 0x01, 0x00, 0x72, 0x00, 0x20 });
                //WriteData(0xb0001a, new byte[] { 0x1f, 0x03 });

                // Max sensitivity (inio's suggestion)
                //WriteData(0x04b00000, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x00, 0x41 });
                //WriteData(0x04b0001a, new byte[] { 0x40, 0x00 });

                // Marcan's suggestion.
                WriteMemory(0x4b00000, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x00, 0xc0 });
                WriteMemory(0x4b0001a, new byte[] { 0x40, 0x00 });

                // Set the ir-mode
                WriteMemory(0x04b00033, (byte)irMode);
            }
        }


        #endregion

        #region Parsing Methods
        protected override bool ParseReport(byte[] report)
        {
            if (report[0] < 0x20)
                throw new InvalidOperationException("Received an output report!");
            InputReport type = (InputReport)report[0];
            if (type >= InputReport.Buttons && type <= InputReport.ButtonsAccelerometer36IrB)
            {
                if (type == InputReport.ButtonsAccelerometer36IrB)
                    ReportingMode = ReportingMode.ButtonsAccelerometer36Ir;
                else
                    ReportingMode = (ReportingMode)type;

                IRMode irMode = GetIrMode(type);
                if (this.irMode != irMode)
                {
                    for (int i = 0; i < _CachedIRBeacons.Length; i++)
                    {
                        BasicIRBeacon newBeacon = null;
                        switch (irMode)
                        {
                            case IRMode.Basic:
                                newBeacon = new BasicIRBeacon();
                                break;
                            case IRMode.Extended:
                                newBeacon = new ExtendedIRBeacon();
                                break;
                            case IRMode.Full:
                                newBeacon = new FullIRBeacon();
                                break;
                        }
                        _CachedIRBeacons[i] = newBeacon;
                        _IRBeacons[i] = null;
                    }
                    this.irMode = irMode;
                }
            }
            switch (type)
            {
                case InputReport.Buttons:
                    ParseButtons(report, 1);
                    break;
                case InputReport.ButtonsAccelerometer:
                    ParseButtons(report, 1);
                    ParseAccelerometer(report, 3);
                    break;
                case InputReport.Buttons8Extension:
                    ParseButtons(report, 1);
                    ParseExtension(report, 3, 8);
                    break;
                case InputReport.ButtonsAccelerometer12Ir:
                    {
                        ParseButtons(report, 1);
                        ParseAccelerometer(report, 3);

                        ExtendedIRBeacon irBeacon;

                        // Parse beacon 0.
                        irBeacon = (ExtendedIRBeacon)_CachedIRBeacons[0];
                        _IRBeacons[0] = ParseExtendedBeacon(report, 6, irBeacon) ? irBeacon : null;

                        // Parse beacon 1.
                        irBeacon = (ExtendedIRBeacon)_CachedIRBeacons[1];
                        _IRBeacons[1] = ParseExtendedBeacon(report, 9, irBeacon) ? irBeacon : null;

                        // Parse beacon 2.
                        irBeacon = (ExtendedIRBeacon)_CachedIRBeacons[2];
                        _IRBeacons[2] = ParseExtendedBeacon(report, 12, irBeacon) ? irBeacon : null;

                        // Parse beacon 3.
                        irBeacon = (ExtendedIRBeacon)_CachedIRBeacons[3];
                        _IRBeacons[3] = ParseExtendedBeacon(report, 15, irBeacon) ? irBeacon : null;
                    }
                    break;
                case InputReport.Buttons19Extension:
                    ParseButtons(report, 1);
                    ParseExtension(report, 3, 19);
                    break;
                case InputReport.ButtonsAccelerometer16Extension:
                    ParseButtons(report, 1);
                    ParseAccelerometer(report, 3);
                    ParseExtension(report, 6, 16);
                    break;
                case InputReport.Buttons10Ir9Extension:
                case InputReport.ButtonsAccelerometer10Ir6Extension:
                    {
                        int offset = 1;
                        offset += ParseButtons(report, offset);
                        if (type == InputReport.ButtonsAccelerometer10Ir6Extension)
                            offset += ParseAccelerometer(report, offset);

                        BasicIRBeacon irBeacon;

                        // Parse beacon 0.
                        irBeacon = _CachedIRBeacons[0];
                        _IRBeacons[0] = ParseBasicBeacon(report, offset, 0, irBeacon) ? irBeacon : null;

                        // Parse beacon 1.
                        irBeacon = _CachedIRBeacons[1];
                        _IRBeacons[1] = ParseBasicBeacon(report, offset, 1, irBeacon) ? irBeacon : null;

                        // Parse beacon 2.
                        irBeacon = _CachedIRBeacons[2];
                        _IRBeacons[2] = ParseBasicBeacon(report, offset + 5, 0, irBeacon) ? irBeacon : null;

                        // Parse beacon 3.
                        irBeacon = _CachedIRBeacons[3];
                        _IRBeacons[3] = ParseBasicBeacon(report, offset + 5, 1, irBeacon) ? irBeacon : null;

                        offset += 10; // Each pair of IR-beacons is 5 bytes.

                        if (type == InputReport.ButtonsAccelerometer10Ir6Extension)
                            ParseExtension(report, offset, 6);
                        else
                            ParseExtension(report, offset, 9);
                    }
                    break;
                case InputReport.Extension:
                    ParseExtension(report, 1, 21);
                    break;
                case InputReport.ButtonsAccelerometer36IrA:
                    {
                        ParseButtons(report, 1); // since button information is complete on both the interlaced reports it needs no temporary storage

                        _PartialAccelerometerX = report[3];
                        _PartialAccelerometerZ = (ushort)(((report[2] & 0x60) << 1) | ((report[1] & 0x60) >> 1));

                        _PartialIRBeaconOneResult = ParseFullBeacon(report, 4, (FullIRBeacon)_CachedIRBeacons[0]);
                        _PartialIRBeaconTwoResult = ParseFullBeacon(report, 13, (FullIRBeacon)_CachedIRBeacons[1]);
                    }
                    break;
                case InputReport.ButtonsAccelerometer36IrB:
                    {
                        ParseButtons(report, 1);

                        // Complete the _PartialAccelerometerZ field
                        _PartialAccelerometerZ = (ushort)(_PartialAccelerometerZ | ((report[2] & 60) >> 3) | ((report[1] & 60) >> 5));

                        // construct the new accelerometer from now completed data
                        Accelerometer.Raw.X = _PartialAccelerometerX;
                        Accelerometer.Raw.Y = report[3];
                        Accelerometer.Raw.Z = _PartialAccelerometerZ;
                        UpdateCalibratedAccelerometer();

                        FullIRBeacon irBeacon;

                        // Commit partial irbeacon from 'a' report
                        IRBeacons[0] = _PartialIRBeaconOneResult ? _CachedIRBeacons[0] : null;

                        // Commit partial irbeacon from 'a' report
                        IRBeacons[1] = _PartialIRBeaconTwoResult ? _CachedIRBeacons[1] : null;

                        // Parse beacon 2.
                        irBeacon = (FullIRBeacon)_CachedIRBeacons[2];
                        IRBeacons[2] = ParseFullBeacon(report, 4, irBeacon) ? irBeacon : null;

                        // Parse beacon 3.
                        irBeacon = (FullIRBeacon)_CachedIRBeacons[3];
                        IRBeacons[3] = ParseFullBeacon(report, 13, irBeacon) ? irBeacon : null;
                    }
                    break;

                case InputReport.GetStatusResult:
                    ParseButtons(report, 1);
                    ParseStatus(report);
                    break;
                case InputReport.ReadDataResult:
                    ParseButtons(report, 1);
                    // The rest is handled by the ReadData method (waiting for the report).
                    break;
                case InputReport.WriteDataResult:
                    // The rest is handled by the WriteData method (waiting for the report).
                    break;
                default:
                    Debug.WriteLine("Unknown report type: " + type.ToString("x"));
                    return false;
            }

            if ((type >= InputReport.Buttons || type == InputReport.GetStatusResult) && (type != InputReport.ButtonsAccelerometer36IrA))
                OnUpdated();
            return true;
        }
        // Partial data received on InputReport.ButtonsAccelerometer36IRa for the interlaced reporting mode 
        // to be used when an InputReport.ButtonsAccelerometer36IRb is received.
        private ushort _PartialAccelerometerX = 0;
        private ushort _PartialAccelerometerZ = 0;
        private bool _PartialIRBeaconOneResult = false;
        private bool _PartialIRBeaconTwoResult = false;

        #region Data Report Parsing


        /// <summary>
        /// Parses a standard button report into the ButtonState struct
        /// </summary>
        /// <param name="buff">Data buffer</param>
        protected int ParseButtons(byte[] buff, int offset)
        {
            WiimoteButtons buttons =
                ((buff[offset + 1] & 0x08) == 0 ? WiimoteButtons.None : WiimoteButtons.A) |
                ((buff[offset + 1] & 0x04) == 0 ? WiimoteButtons.None : WiimoteButtons.B) |
                ((buff[offset + 1] & 0x10) == 0 ? WiimoteButtons.None : WiimoteButtons.Minus) |
                ((buff[offset + 1] & 0x80) == 0 ? WiimoteButtons.None : WiimoteButtons.Home) |
                ((buff[offset + 0] & 0x10) == 0 ? WiimoteButtons.None : WiimoteButtons.Plus) |
                ((buff[offset + 1] & 0x02) == 0 ? WiimoteButtons.None : WiimoteButtons.One) |
                ((buff[offset + 1] & 0x01) == 0 ? WiimoteButtons.None : WiimoteButtons.Two) |
                ((buff[offset + 0] & 0x08) == 0 ? WiimoteButtons.None : WiimoteButtons.Up) |
                ((buff[offset + 0] & 0x04) == 0 ? WiimoteButtons.None : WiimoteButtons.Down) |
                ((buff[offset + 0] & 0x01) == 0 ? WiimoteButtons.None : WiimoteButtons.Left) |
                ((buff[offset + 0] & 0x02) == 0 ? WiimoteButtons.None : WiimoteButtons.Right);
            _Buttons = buttons;
            return 2;
        }

        /// <summary>
        /// Parse accelerometer data
        /// </summary>
        /// <param name="buff">Data buffer</param>
        protected int ParseAccelerometer(byte[] buff, int offset)
        {
            if (Accelerometer != null)
            {
                Accelerometer.Raw.X = buff[offset];
                Accelerometer.Raw.Y = buff[offset + 1];
                Accelerometer.Raw.Z = buff[offset + 2];
                UpdateCalibratedAccelerometer();
            }
            return 3;
        }

        #region ParseBeacon Methods
        /// <summary>
        /// http://wiibrew.org/index.php?title=Wiimote#Basic_Mode
        /// </summary>
        protected static bool ParseBasicBeacon(byte[] buff, int offset, int beaconIndex, BasicIRBeacon irBeacon)
        {
            switch (beaconIndex)
            {
                case 0:
                    irBeacon.X = buff[offset + 0] | ((buff[offset + 2] >> 4) & 0x03) << 8;
                    irBeacon.Y = buff[offset + 1] | ((buff[offset + 2] >> 6) & 0x03) << 8;
                    break;
                case 1:
                    irBeacon.X = buff[offset + 3] | ((buff[offset + 2] >> 0) & 0x03) << 8;
                    irBeacon.Y = buff[offset + 4] | ((buff[offset + 2] >> 2) & 0x03) << 8;
                    break;
                case 2:
                    irBeacon.X = buff[11] | ((buff[13] >> 4) & 0x03) << 8;
                    irBeacon.Y = buff[12] | ((buff[13] >> 6) & 0x03) << 8;
                    break;
                case 3:
                    irBeacon.X = buff[14] | ((buff[13] >> 0) & 0x03) << 8;
                    irBeacon.Y = buff[15] | ((buff[13] >> 2) & 0x03) << 8;
                    break;
            }
            return !(irBeacon.X == 1023 && irBeacon.Y == 1023);
        }

        /// <summary>
        /// http://wiibrew.org/index.php?title=Wiimote#Extended_Mode
        /// </summary>
        protected static bool ParseExtendedBeacon(byte[] buff, int offset, ExtendedIRBeacon irBeacon)
        {
            if (buff[offset + 0] == 0xff && buff[offset + 1] == 0xff && buff[offset + 2] == 0xff)
                return false;
            irBeacon.X = buff[offset + 0] | ((buff[offset + 2] >> 4) & 0x03) << 8;
            irBeacon.Y = buff[offset + 1] | ((buff[offset + 2] >> 6) & 0x03) << 8;
            irBeacon.Size = buff[offset + 2] & 0x0f;
            return true;
        }

        /// <summary>
        /// http://wiibrew.org/index.php?title=Wiimote#Full_Mode
        /// </summary>
        protected static bool ParseFullBeacon(byte[] buff, int offset, FullIRBeacon irBeacon)
        {
            if (buff[offset + 0] == 0xff && buff[offset + 1] == 0xff && buff[offset + 2] == 0xff)
                return false;

            irBeacon.X = buff[offset + 0] | ((buff[offset + 2] >> 4) & 0x03) << 8;
            irBeacon.Y = buff[offset + 1] | ((buff[offset + 2] >> 6) & 0x03) << 8;
            irBeacon.Size = buff[offset + 2] & 0x0F;

            irBeacon.XMin = buff[offset + 3] & 0x7F;
            irBeacon.YMin = buff[offset + 4] & 0x7F;
            irBeacon.XMax = buff[offset + 5] & 0x7F;
            irBeacon.YMax = buff[offset + 6] & 0x7F;
            irBeacon.Intensity = buff[offset + 8];
            return true;
        }
        #endregion
        #endregion

        protected void ParseExtension(byte[] buff, int offset, int count)
        {
            if (Extension != null)
                Extension.ParseExtensionData(buff, offset, count);
        }

        protected void ParseStatus(byte[] buff)
        {
            bool extensionConnected = (buff[3] & 0x02) != 0;

            _IsSpeakerEnabled = (buff[3] & 0x04) != 0;

            // TODO: Make IrSensorEnabled publicly available.
            //bool irSensorEnabled = (buff[3] & 0x08) != 0;
            _Leds =
                ((buff[3] & 0x10) == 0 ? WiimoteLeds.None : WiimoteLeds.Led1) |
                ((buff[3] & 0x20) == 0 ? WiimoteLeds.None : WiimoteLeds.Led2) |
                ((buff[3] & 0x40) == 0 ? WiimoteLeds.None : WiimoteLeds.Led3) |
                ((buff[3] & 0x80) == 0 ? WiimoteLeds.None : WiimoteLeds.Led4);

            BatteryLevel = buff[6];

            if (extensionConnected && _Extension == null)
                InitializeExtension();
			else if (_Extension != null && !extensionConnected)
			{
                ReportingMode = ReportingMode.None;
				if(ExtensionDetached != null)
					ExtensionDetached(this, new WiimoteExtensionEventArgs(_Extension));
				_Extension = null;
			}
        }
        #endregion

        #region Memory Methods
        protected byte[] ReadMemory(uint address, short count)
        {
            byte[] result = new byte[count];
            ReadMemory(address, result, 0, count);
            return result;
        }
        protected byte ReadMemory(uint address)
        {
            return ReadMemory(address, 1)[0];
        }

        protected void WriteMemory(uint address, byte[] data)
        {
            WriteMemory(address, data, 0, (byte)data.Length);
        }

        protected void WriteMemory(uint address, byte data)
        {
            WriteMemory(address, new byte[] { data });
        }
        #endregion

        #region Extension Methods
        protected void InitializeExtension()
        {
            try
            {
                WriteMemory(0x04a40040, 0); // Set the encryption-key to 0.
            }
            catch (TimeoutException)
            {
                if (IsConnected)
                    throw;
                return;
            }
            byte[] extensionTypeBytes = ReadMemory(0x04a400fe, 2);
            extensionTypeBytes[0] = (byte)((extensionTypeBytes[0] ^ 0x17) + 0x17 & 0xFF);
            extensionTypeBytes[1] = (byte)((extensionTypeBytes[1] ^ 0x17) + 0x17 & 0xFF);
            ushort extensionType = (ushort)(extensionTypeBytes[1] << 8 | extensionTypeBytes[0]);
            IWiimoteExtension newExtension;
            switch (extensionType)
            {
                case 0x2e2e:
                    newExtension = null;
                    break;
                case 0xffff:
                    newExtension = new InvalidExtension(this);
                    break;
                default:
                    IWiimoteExtensionFactory extensionFactory;
                    if (WiimoteExtensionRegistry.TryGetValue(extensionType, out extensionFactory))
                        newExtension = extensionFactory.Create(this);
                    else
                        newExtension = new UnknownExtension(this);
                    break;
            }
            ReportingMode = ReportingMode.None;
			_Extension = newExtension;
			if(_Extension != null)
			{
				if(ExtensionAttached != null)
					ExtensionAttached(this, new WiimoteExtensionEventArgs(_Extension));
			}
        }

        protected void DetachExtension()
        {
            if (Extension == null)
                throw new InvalidOperationException("Can't detach extension: No extension is connected.");

            ReportingMode = ReportingMode.None;
            _Extension.Detached();
			if(ExtensionDetached != null)
				ExtensionDetached(this, new WiimoteExtensionEventArgs(_Extension));
            _Extension = null;
        }
        #endregion

        #region Updated Event
        protected void OnUpdated()
        {
            OnUpdated(EventArgs.Empty);
        }

        protected virtual void OnUpdated(EventArgs e)
        {
            if (Updated == null)
                return;
            Updated(this, e);
        }
        public event EventHandler Updated;
        #endregion
    }
}
