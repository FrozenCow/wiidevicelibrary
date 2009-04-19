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

namespace WiiDeviceLibrary.Extensions
{
    /// <summary>
    /// An implementation of the classic controller extension for the wiimote.
    /// </summary>
    /// <remarks>
    /// The implementation was made according to the documentation provided on the wiibrew website
    /// at the following url: http://wiibrew.org/wiki/Wiimote/Extension_Controllers#Classic_Controller
    /// since no information about calibration is specified there this is currently lacking in the implementation.
    /// </remarks>
    public class ClassicControllerExtension : IWiimoteExtension
    {
	
        #region Fields
        private IWiimote _Wiimote = null;
        private AnalogStick _LeftStick = default(AnalogStick);
        private AnalogStick _RightStick = default(AnalogStick);
        private byte _LeftTrigger = 0;
        private byte _RightTrigger = 0;
        private ClassicControllerButtons _Buttons = ClassicControllerButtons.None;
        #endregion

        #region Properties of the classic controller
        /// <summary>
        /// Gets the left analog stick.
        /// </summary>
        public AnalogStick LeftStick
        {
            get { return _LeftStick; }
        }

        /// <summary>
        /// Gets the right analog stick.
        /// </summary>
        public AnalogStick RightStick
        {
            get { return _RightStick; }
        }

        /// <summary>
        /// Gets the raw value of the distance the left trigger is pressed.
        /// </summary>
        public byte LeftTrigger
        {
            get { return _LeftTrigger; }
        }

        /// <summary>
        /// Gets the raw value of the distance the right trigger is pressed.
        /// </summary>
        public byte RightTrigger
        {
            get { return _RightTrigger; }
        }

        /// <summary>
        /// Gets the value that indicates which buttons are pressed.
        /// </summary>
        public ClassicControllerButtons Buttons
        {
            get { return _Buttons; }
        }
        #endregion

        internal ClassicControllerExtension(IWiimote wiimote)
        {
            _Wiimote = wiimote;
			ReadCalibrationData();
        }

		private void ReadCalibrationData()
		{
            byte[] buffer = new byte[16];
            _Wiimote.ReadMemory(0x04A40020, buffer, 0, 16);
            for (int i = 0; i < 16; i++)
            {
                buffer[i] = (byte)((buffer[i] ^ 0x17) + 0x17 & 0xFF);
            }

            AnalogStickCalibration leftStickCalibration = new AnalogStickCalibration((byte)(buffer[1] >> 2), (byte)(buffer[2] >> 2), (byte)(buffer[0] >> 2),
			                                                   (byte)(buffer[4]>>2),(byte)(buffer[5]>>2),(byte)(buffer[3]>>2));

            _LeftStick = new AnalogStick(leftStickCalibration);

            AnalogStickCalibration rightStickCalibration = new AnalogStickCalibration((byte)(buffer[7] >> 3), (byte)(buffer[8] >> 3), (byte)(buffer[6] >> 3),
			                                                   (byte)(buffer[10]>>3),(byte)(buffer[11]>>3),(byte)(buffer[9]>>3));

            _RightStick = new AnalogStick(rightStickCalibration);			
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

            // 6 bits, so values range from 0 to 63
			_LeftStick.Raw.X = (byte)(buffer[offset + 0] & 0x3f);
			_LeftStick.Raw.Y = (byte)(buffer[offset + 1] & 0x3f);
			_LeftStick.Calibration.Calibrate(_LeftStick.Raw, _LeftStick.Calibrated);
			
            // 5 bits are scattered across 3 bytes for the x axis, values range from 0 to 31
			_RightStick.Raw.X = (byte)(((buffer[offset + 0] & 0xc0) >> 3 ) | ((buffer[offset + 1] & 0xc0) >> 5) | (buffer[offset + 2] >> 7));
			_RightStick.Raw.Y = (byte)(buffer[offset + 2] & 0x1f);
			_RightStick.Calibration.Calibrate(_RightStick.Raw, _RightStick.Calibrated);			

            // 5 bits scattered across 2 bytes for the left trigger, values range from 0 to 31
            _LeftTrigger = (byte)(((buffer[offset + 2] & 0x60) >> 2) | (buffer[offset + 3] >> 5));
            _RightTrigger = (byte)(buffer[offset + 3] & 0x1f);

            // buttons are pressed when values are zero
            // therefore if the bit equals one (not equals zero) the button is not pressed
            _Buttons =
                ((buffer[offset + 4] & 2) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.RT) |
                ((buffer[offset + 4] & 4) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Plus) |
                ((buffer[offset + 4] & 8) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Home) |
                ((buffer[offset + 4] & 16) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Minus) |
                ((buffer[offset + 4] & 32) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.LT) |
                ((buffer[offset + 4] & 64) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Down) |
                ((buffer[offset + 4] & 128) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Right) |

                ((buffer[offset + 5] & 1) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Up) |
                ((buffer[offset + 5] & 2) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Left) |
                ((buffer[offset + 5] & 4) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.ZR) |
                ((buffer[offset + 5] & 8) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.X) |
                ((buffer[offset + 5] & 16) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.A) |
                ((buffer[offset + 5] & 32) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.Y) |
                ((buffer[offset + 5] & 64) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.B) |
                ((buffer[offset + 5] & 128) != 0 ? ClassicControllerButtons.None : ClassicControllerButtons.ZL);
        }

        public void Detached()
        {
        }
        #endregion
    }
}
