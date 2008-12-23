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

namespace WiiDeviceLibrary
{
    public interface IDevice
    {
        /// <summary>
        /// Gets the <code>IDeviceInfo</code> for this device.
        /// </summary>
        IDeviceInfo DeviceInfo
        {
            get;
        }

        /// <summary>
        /// Disconnects the bluetooth connection to the device.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Occurs when the device was disconnected.
        /// </summary>
        event EventHandler Disconnected;
    }
}
