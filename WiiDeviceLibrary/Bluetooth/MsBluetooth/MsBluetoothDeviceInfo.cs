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

namespace WiiDeviceLibrary.Bluetooth.MsBluetooth
{
    public class MsBluetoothDeviceInfo: MsHidDeviceInfo, IBluetoothDeviceInfo
    {
        #region Properties
        private BluetoothAddress _BluetoothAddress;
        public BluetoothAddress BluetoothAddress
        {
            get { return _BluetoothAddress; }
        }

        private NativeMethods.BluetoothDeviceInfo _Device;
        internal NativeMethods.BluetoothDeviceInfo Device
        {
            get { return _Device; }
            set { _Device = value; }
        }
        #endregion
        #region Constructors
        internal MsBluetoothDeviceInfo(BluetoothAddress bluetoothAddress, NativeMethods.BluetoothDeviceInfo device)
        {
            _BluetoothAddress = bluetoothAddress;
            _Device = device;
        }
        #endregion
        #region Methods
        public override bool Equals(MsHidDeviceInfo other)
        {
            MsBluetoothDeviceInfo msbtOther = other as MsBluetoothDeviceInfo;
            if (msbtOther == null)
                return false;
            return Equals(msbtOther);
        }

        public bool Equals(MsBluetoothDeviceInfo other)
        {
            return this.BluetoothAddress == other.BluetoothAddress;
        }

        public override int GetHashCode()
        {
            return BluetoothAddress.GetHashCode();
        }
        #endregion
    }
}
