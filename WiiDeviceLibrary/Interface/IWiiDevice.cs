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

namespace WiiDeviceLibrary
{
    public interface IWiiDevice: IDevice
    {
        void Initialize();

        /// <summary>
        /// Gets the level of charge the battery has.
        /// </summary>
        byte BatteryLevel
        {
            get;
        }

        /// <summary>
        /// Changes the current reporting mode to the specified <code>InputReport</code>.
        /// </summary>
        /// <param name="reportingMode">The type of <code>InputReport</code>.</param>
        void SetReportingMode(ReportingMode reportingMode);

        /// <summary>
        /// Reads bytes from the memory at the specified <paramref name="address"/> and returns this in 
        /// the <paramref name="buffer"/> at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="address">The address from which to read.</param>
        /// <param name="buffer">The <code>byte[]</code> used to store the read memory.</param>
        /// <param name="offset">The offset at which to store the read memory in the buffer.</param>
        /// <param name="count">The amount of bytes to be read from memory.</param>
        void ReadMemory(uint address, byte[] buffer, int offset, short count);

        /// <summary>
        /// Writes bytes to the memory at the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address to write to.</param>
        /// <param name="buffer">The <code>byte[]</code> containing the bytes to be written to memory.</param>
        /// <param name="offset">The offset within the <paramref name="buffer"/> to start reading from.</param>
        /// <param name="count">The amount of bytes to be written to memory.</param>
        void WriteMemory(uint address, byte[] buffer, int offset, byte count);

        /// <summary>
        /// Updates the status of the device.
        /// </summary>
        /// <remarks>
        /// The device status consists of the combined status of the extension controller, the speaker, the battery level,
        /// the ir camera and the leds.
        /// </remarks>
        void UpdateStatus();

        /// <summary>
        /// Gets the current mode in which the device is operating.
        /// </summary>
        ReportingMode ReportingMode
        {
            get;
        }

        event EventHandler Updated;
    }
}
