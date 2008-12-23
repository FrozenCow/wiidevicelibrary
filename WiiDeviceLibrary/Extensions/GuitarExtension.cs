//    Copyright 2008 Wii Device Library authors
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
    /// An implementation of the guitar extension for the wiimote.
    /// </summary>
    /// <remarks>
    /// The implementation was made according to the documentation provided on the wiibrew website
    /// at the following url: http://wiibrew.org/wiki/Wiimote/Extension_Controllers#Guitar_Hero_Guitar.
    /// </remarks>
    public class GuitarExtension : IWiimoteExtension
    {
        #region Fields
        private IWiimote _Wiimote = null;
        private GuitarButtons _Buttons = GuitarButtons.None;
        private AnalogStickAxes<byte> _Stick = new AnalogStickAxes<byte>();
        private byte _WhammyBar = 0;
        #endregion

        internal GuitarExtension(IWiimote wiimote)
        {
            _Wiimote = wiimote;
        }

        #region Properties
        /// <summary>
        /// Gets the value that indicates which buttons are pressed.
        /// </summary>
        public GuitarButtons Buttons
        {
            get { return _Buttons; }
        }

        /// <summary>
        /// Gets the analog stick.
        /// </summary>
        public AnalogStickAxes<byte> Stick
        {
            get { return _Stick; }
        }

        /// <summary>
        /// Gets the raw value of the whammy bar. 
        /// </summary>
        public byte WhammyBar
        {
            get { return _WhammyBar; }
        }
        #endregion

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

			_Stick.X = buffer[offset + 0];
			_Stick.Y = buffer[offset + 1];
			
            _WhammyBar = buffer[offset + 3];

            _Buttons =
                ((buffer[offset + 4] & 4) == 0 ? GuitarButtons.None : GuitarButtons.Plus) |
                ((buffer[offset + 4] & 16) == 0 ? GuitarButtons.None : GuitarButtons.Minus) |
                ((buffer[offset + 4] & 64) == 0 ? GuitarButtons.None : GuitarButtons.Down) |

                ((buffer[offset + 5] & 1) == 0 ? GuitarButtons.None : GuitarButtons.Up) |
                ((buffer[offset + 5] & 8) == 0 ? GuitarButtons.None : GuitarButtons.Yellow) |
                ((buffer[offset + 5] & 16) == 0 ? GuitarButtons.None : GuitarButtons.Green) |
                ((buffer[offset + 5] & 32) == 0 ? GuitarButtons.None : GuitarButtons.Blue) |
                ((buffer[offset + 5] & 64) == 0 ? GuitarButtons.None : GuitarButtons.Red) |
                ((buffer[offset + 5] & 128) == 0 ? GuitarButtons.None : GuitarButtons.Orange);
        }

        public void Detached()
        {
        }
        #endregion
    }
}
