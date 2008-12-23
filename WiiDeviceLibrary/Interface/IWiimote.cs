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

namespace WiiDeviceLibrary
{
    /// <summary>
    /// An interface for the wiimote.
    /// </summary>
    public interface IWiimote : IWiiDevice
    {
        /// <summary>
        /// Gets or sets the enumeration that indicates which of the leds are enabled.
        /// </summary>
        WiimoteLeds Leds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value that indicates the status of the rumble feature.
        /// </summary>
        bool IsRumbling
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the enumeration that indicates which of the buttons are pressed.
        /// </summary>
        WiimoteButtons Buttons
        {
            get;
        }

        /// <summary>
        /// Gets an array of length 4 that contains the infrared beacons that where detected.
        /// </summary>
        BasicIRBeacon[] IRBeacons
        {
            get;
        }

        /// <summary>
        /// Gets a structure containing the values for the three built in accelerometers.
        /// </summary>
        Accelerometer Accelerometer
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value that indicates the status of the speaker.
        /// </summary>
        bool IsSpeakerEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the currently connected wiimote extension.
        /// </summary>
        IWiimoteExtension Extension
        {
            get;
        }

        /// <summary>
        /// Sends the data in the provided buffer to the speaker.
        /// </summary>
        /// <param name="buffer">A byte[] containing sound data.</param>
        /// <param name="offset">The offset of the sound data within the byte[].</param>
        /// <param name="count">The length of the sound data in the byte[].</param>
        void SendSpeakerData(byte[] buffer, int offset, byte count);		
		
		/// <summary>
		/// Raised whenever an extension is plugged in.
		/// </summary>
		event EventHandler<WiimoteExtensionEventArgs> ExtensionAttached;
		
		/// <summary>
		/// Raised whenever an extension is removed.
		/// </summary>		
		event EventHandler<WiimoteExtensionEventArgs> ExtensionDetached;
    }
}
