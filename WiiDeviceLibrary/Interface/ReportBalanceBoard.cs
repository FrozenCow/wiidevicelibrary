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
using System.IO;

namespace WiiDeviceLibrary
{
    public class ReportBalanceBoard : ReportDevice, IBalanceBoard
    {
        #region Fields
        private PressureCalibration _TopRightCalibration;
        private PressureCalibration _BottomRightCalibration;
        private PressureCalibration _BottomLeftCalibration;
        private PressureCalibration _TopLeftCalibration;
        private ushort _TopRight = 0;
        private ushort _BottomRight = 0;
        private ushort _TopLeft = 0;
        private ushort _BottomLeft = 0;
        private bool _Led = false;
        private bool _Button = false;
        #endregion

        #region Constructors
        public ReportBalanceBoard(IDeviceInfo deviceInfo, Stream communicationStream)
            : base(deviceInfo, communicationStream)
        {
        }
        #endregion

        public override void Initialize()
        {
            ReadCalibrationData();
            SetReportingMode(ReportingMode.Buttons8Extension);
        }

        protected override bool ParseReport(byte[] report)
        {
			InputReport type = (InputReport)report[0];
			if (type == InputReport.GetStatusResult)
			{
				BatteryLevel = (report[6]);
                OnUpdated();
			}
            else if (type == InputReport.Buttons8Extension)
            {
                _Button = (report[2] & 0x08) == 0x8;

                int offset = 3;
                _TopRight = (ushort)((report[offset + 0] << 8) | report[offset + 1]);
                _BottomRight = (ushort)((report[offset + 2] << 8) | report[offset + 3]);
                _TopLeft = (ushort)((report[offset + 4] << 8) | report[offset + 5]);
                _BottomLeft = (ushort)((report[offset + 6] << 8) | report[offset + 7]);
                OnUpdated();
            }
            return true;
        }

        protected void SetLed(bool value)
        {
            CreateReport(OutputReport.SetLeds);
            if (value)
                OutputBuffer[1] = (byte)((byte)1 << 4);
            SendReport();
            _Led = value;
        }

        private void ReadCalibrationData()
        {
            byte[] buffer = new byte[24];
            ReadMemory(0x04a40024, buffer, 0, 24);

            _TopRightCalibration = new PressureCalibration(new ushort[] {
                (ushort)((buffer[0] << 8) | buffer[1]),
                (ushort)((buffer[8] << 8) | buffer[9]),
                (ushort)((buffer[16] << 8) | buffer[17])
            });
            
            _BottomRightCalibration = new PressureCalibration(new ushort[] {
                (ushort)((buffer[2] << 8) | buffer[3]),
                (ushort)((buffer[10] << 8) | buffer[11]),
                (ushort)((buffer[18] << 8) | buffer[19])
            });
            
            _TopLeftCalibration = new PressureCalibration(new ushort[] {
                (ushort)((buffer[4] << 8) | buffer[5]),
                (ushort)((buffer[12] << 8) | buffer[13]),
                (ushort)((buffer[20] << 8) | buffer[21])
            });

            _BottomLeftCalibration = new PressureCalibration(new ushort[] {
                (ushort)((buffer[6] << 8) | buffer[7]),
                (ushort)((buffer[14] << 8) | buffer[15]),
                (ushort)((buffer[22] << 8) | buffer[23])
            });
        }

        #region IBalanceBoard Members
        public bool Button
        {
            get { return _Button; }
        }

        public bool Led
        {
            get { return _Led; }
            set { if (_Led != value) SetLed(value); }
        }

        public ushort BottomRight
        {
            get { return _BottomRight; }
        }

        public ushort BottomLeft
        {
            get { return _BottomLeft; }
        }

        public ushort TopRight
        {
            get { return _TopRight; }
        }

        public ushort TopLeft
        {
            get { return _TopLeft; }
        }

        /// <summary>
        /// Gets the calibrated value for the top right pressure sensor in kilograms.
        /// </summary>
        public float TopRightWeight
        {
            get { return _TopRightCalibration.Calibrate(_TopRight); }
        }

        /// <summary>
        /// Gets the calibrated value for the bottom right pressure sensor in kilograms.
        /// </summary>
        public float BottomRightWeight
        {
            get { return _BottomRightCalibration.Calibrate(_BottomRight); }
        }

        /// <summary>
        /// Gets the calibrated value for the top left pressure sensor in kilograms.
        /// </summary>
        public float TopLeftWeight
        {
            get { return _TopLeftCalibration.Calibrate(_TopLeft); }
        }

        /// <summary>
        /// Gets the calibrated value for the bottom left pressure sensor in kilograms.
        /// </summary>
        public float BottomLeftWeight
        {
            get { return _BottomLeftCalibration.Calibrate(_BottomLeft); }
        }

        public float TotalWeight
        {
            get { return BottomLeftWeight + BottomRightWeight + TopLeftWeight + TopRightWeight; }
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
