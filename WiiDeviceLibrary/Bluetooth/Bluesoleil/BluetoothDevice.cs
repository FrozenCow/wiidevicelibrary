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

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    public class BluetoothDevice
    {
        #region Properties
        private BluesoleilService owner;
        public BluesoleilService Owner
        {
            get { return owner; }
        }

        private NativeMethods.BLUETOOTH_DEVICE_INFO deviceInfo;
        internal NativeMethods.BLUETOOTH_DEVICE_INFO DeviceInfo
        {
            get { return deviceInfo; }
        }

        private byte[] address;
        public byte[] Address
        {
            get { return address; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }
        #endregion

        #region Constructors
        internal BluetoothDevice(BluesoleilService owner, NativeMethods.BLUETOOTH_DEVICE_INFO deviceInfo)
        {
            this.owner = owner;
            this.deviceInfo = deviceInfo;
            int zeroIndex = Array.IndexOf<byte>(deviceInfo.szName, 0);
            this.name = Encoding.ASCII.GetString(deviceInfo.szName, 0, zeroIndex);
            address = deviceInfo.address;
        }
        #endregion
    }
}
