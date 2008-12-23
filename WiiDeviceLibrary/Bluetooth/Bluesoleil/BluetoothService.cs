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
    public class BluetoothService
    {
        #region Properties
        private BluesoleilService owner;
        public BluesoleilService Owner
        {
            get { return owner; }
        }

        private NativeMethods.GENERAL_SERVICE_INFO serviceInfo;
        internal NativeMethods.GENERAL_SERVICE_INFO ServiceInfo
        {
            get { return serviceInfo; }
        }

        private BluetoothDevice device;
        public BluetoothDevice Device
        {
            get { return device; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }
        #endregion

        #region Constructors
        internal BluetoothService(BluesoleilService owner, BluetoothDevice device, NativeMethods.GENERAL_SERVICE_INFO serviceInfo)
        {
            this.owner = owner;
            this.serviceInfo = serviceInfo;
            int zeroIndex = Array.IndexOf<byte>(serviceInfo.szServiceName, 0);
            this.name = Encoding.ASCII.GetString(serviceInfo.szServiceName, 0, zeroIndex);
            this.device = device;
        }
        #endregion
    }
}
