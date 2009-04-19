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


namespace WiiDeviceLibrary.Extensions
{
    /// <summary>
    /// An Implementation of the nunchuk extension for the wiimote.
    /// </summary>
    public class NunchukExtension : IWiimoteExtension
    {
        #region Fields
        private IWiimote _Wiimote = null;
        private AnalogStick _Stick = default(AnalogStick);
        private Accelerometer _Accelerometer = null;
        private NunchukButtons _Buttons = NunchukButtons.None;
        #endregion

        #region Properties of the nunchuk controls
        /// <summary>
        /// Gets the analog stick.
        /// </summary>
        public AnalogStick Stick
        {
            get { return _Stick; }
        }

        /// <summary>
        /// Gets the raw acceleration values for the x, y and z axes.
        /// </summary>
        public Accelerometer Accelerometer
        {
            get { return _Accelerometer; }
        }

        /// <summary>
        /// Gets the value that indicates which buttons are pressed.
        /// </summary>
        public NunchukButtons Buttons
        {
            get { return _Buttons; }
        }
        #endregion

        internal NunchukExtension(IWiimote wiimote)
        {
            _Wiimote = wiimote;
            ReadCalibrationData();
        }

        // read the calibration data and undo obfuscation transformation 
        private void ReadCalibrationData()
        {
            byte[] buffer = new byte[16];
            // 0x04A40020 is the address of the calibration data on the wiimote
            _Wiimote.ReadMemory(0x04A40020, buffer, 0, 16);
            for (int i = 0; i < 16; i++)
            {
                buffer[i] = (byte)((buffer[i] ^ 0x17) + 0x17 & 0xFF);
            }

            AccelerometerCalibration accelerometerCalibration = new AccelerometerCalibration(
                 (ushort)((buffer[0] << 2) + ((buffer[3]) & 0x3)),
                 (ushort)((buffer[4] << 2) + ((buffer[7]) & 0x3)),
                 (ushort)((buffer[1] << 2) + ((buffer[3] >> 2) & 0x3)),
                 (ushort)((buffer[5] << 2) + ((buffer[7] >> 2) & 0x3)),
                 (ushort)((buffer[2] << 2) + ((buffer[3] >> 4) & 0x3)),
                 (ushort)((buffer[6] << 2) + ((buffer[7] >> 4) & 0x3)));

            _Accelerometer = new Accelerometer(accelerometerCalibration);

            AnalogStickCalibration stickCalibration = new AnalogStickCalibration(buffer[9], buffer[10], buffer[8],
			                                               buffer[12],buffer[13], buffer[11]);

            _Stick = new AnalogStick(stickCalibration);
        }

        #region IWiimoteExtension Members
        public IWiimote Wiimote
        {
            get { return _Wiimote; }
        }

        void IWiimoteExtension.ParseExtensionData(byte[] buffer, int offset, int count)
        {
            ParseExtensionData(buffer, offset, count);
        }

        protected void ParseExtensionData(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                buffer[i] = (byte)((buffer[i] ^ 0x17) + 0x17 & 0xFF);
            }

			_Stick.Raw.X = buffer[offset + 0];
			_Stick.Raw.Y = buffer[offset + 1];
			_Stick.Calibration.Calibrate(_Stick.Raw, _Stick.Calibrated);
			
			_Accelerometer.Raw.X = (ushort)((buffer[offset + 2] << 2) + ((buffer[offset + 5] >> 2) & 0x03));
			_Accelerometer.Raw.Y = (ushort)((buffer[offset + 3] << 2) + ((buffer[offset + 5] >> 4) & 0x03));
			_Accelerometer.Raw.Z = (ushort)((buffer[offset + 4] << 2) + ((buffer[offset + 5] >> 6) & 0x03));
			
			_Accelerometer.Calibration.Calibrate(_Accelerometer.Raw, _Accelerometer.Calibrated);
	
            _Buttons =
                ((buffer[offset + 5] & 0x01) != 0 ? NunchukButtons.None : NunchukButtons.Z) |
                ((buffer[offset + 5] & 0x02) != 0 ? NunchukButtons.None : NunchukButtons.C);
        }

        public void Detached()
        {
        }
        #endregion
    }
}
