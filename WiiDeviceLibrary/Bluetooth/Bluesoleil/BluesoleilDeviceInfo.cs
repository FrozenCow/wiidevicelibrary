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
using WiiDeviceLibrary.Bluetooth.MsHid;

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    public class BluesoleilDeviceInfo : MsHidDeviceInfo, IBluetoothDeviceInfo
    {
        #region Properties
        private BluetoothDevice _Device;
        public BluetoothDevice Device
        {
            get { return _Device; }
        }

        private BluetoothService _Service;
        public BluetoothService Service
        {
            get { return _Service; }
        }

        private BluetoothAddress _Address;
        public BluetoothAddress Address
        {
            get { return _Address; }
        }

        public string Name
        {
            get { return this.Device.Name; }
        }
        #endregion
        #region Constructors
        public BluesoleilDeviceInfo(BluetoothDevice device, BluetoothService service)
        {
            _Device = device;
            _Service = service;
            _Address = new BluetoothAddress(device.Address);
        }
        #endregion
        #region Methods
        public override bool Equals(MsHidDeviceInfo other)
        {
            BluesoleilDeviceInfo bsdiOther = other as BluesoleilDeviceInfo;
            if (bsdiOther == null)
                return false;
            return Equals(bsdiOther);
        }

        public bool Equals(BluesoleilDeviceInfo other)
        {
            return this.Address == other.Address;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override string ToString()
        {
            return Address.ToString();
        }
        #endregion
    }
}
