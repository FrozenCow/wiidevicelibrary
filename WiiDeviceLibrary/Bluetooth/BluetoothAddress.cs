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
using System.Collections;

namespace WiiDeviceLibrary.Bluetooth
{
    public struct BluetoothAddress: IEquatable<BluetoothAddress>
    {
        private const int AddressLength = 6;
        private byte[] address;

        public BluetoothAddress(byte[] address)
        {
            if (address.Length != AddressLength)
                throw new ArgumentException("The specified address must be 6 bytes long.");
            this.address = address;
        }

        public byte[] GetBytes()
        {
            return (byte[])address.Clone();
        }

        public bool Equals(BluetoothAddress other)
        {
            for (int i = 0; i < AddressLength; i++)
            {
                if (address[i] != other.address[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is BluetoothAddress)
                return Equals((BluetoothAddress)obj);
 	         return base.Equals(obj);
        }

        public static bool operator ==(BluetoothAddress bluetoothAddressA, BluetoothAddress bluetoothAddressB)
        {
            return bluetoothAddressA.Equals(bluetoothAddressB);
        }

        public static bool operator !=(BluetoothAddress bluetoothAddressA, BluetoothAddress bluetoothAddressB)
        {
            return !bluetoothAddressA.Equals(bluetoothAddressB);
        }

        public override string ToString()
        {
            StringBuilder sbuilder = new StringBuilder(AddressLength * 3 - 1);
            for (int i = 0; i < AddressLength; i++)
            {
                sbuilder.Append(address[i].ToString("x2"));
                if (i < AddressLength - 1)
                    sbuilder.Append(":");
            }
            return sbuilder.ToString();
        }

        public override int GetHashCode()
        {
            return unchecked(
                (address[0] | address[1] << 8 | address[2] << 16 | address[3] << 24)
                ^ (address[4] | address[5] << 8)
                );
        }
    }
}
