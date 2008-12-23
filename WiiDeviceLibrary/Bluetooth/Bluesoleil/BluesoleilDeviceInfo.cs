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
    public class BluesoleilDeviceInfo : IBluetoothDeviceInfo
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

        private BluetoothAddress _BluetoothAddress;
        public BluetoothAddress BluetoothAddress
        {
            get { return _BluetoothAddress; }
        }
        #endregion
        #region Constructors
        public BluesoleilDeviceInfo(BluetoothDevice device, BluetoothService service)
        {
            _Device = device;
            _Service = service;
            _BluetoothAddress = new BluetoothAddress(device.Address);
        }
        #endregion
    }
}
