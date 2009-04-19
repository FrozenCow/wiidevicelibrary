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

namespace WiiDeviceLibrary.Bluetooth.Bluez
{
	public class BluezDeviceInfo : IBluetoothDeviceInfo
	{
		#region Fields
		private BluetoothAddress _BluetoothAddress;
		private DateTime _LastSeen;
		private string _Name = String.Empty;
        #endregion
		
        #region Properties
		public BluetoothAddress BluetoothAddress
		{
			get { return _BluetoothAddress; }
		}
		
		public string Name
		{
			get { return _Name; }
		}
		
		public DateTime LastSeen
		{
			get { return _LastSeen; }
			internal set { _LastSeen = value; }
		}
        #endregion
		
		internal BluezDeviceInfo(BluetoothAddress bluetoothAddress, string name)
		{
			_BluetoothAddress = bluetoothAddress;
			_Name = name;
			_LastSeen = DateTime.Now;
		}
    }
}
