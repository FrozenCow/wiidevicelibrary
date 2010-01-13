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

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public class MsHidDeviceInfo: IDeviceInfo, IEquatable<MsHidDeviceInfo>
    {
        private string _DevicePath;
        public string DevicePath
        {
            get { return _DevicePath; }
            set
            {
                if (_DevicePath != null)
                    throw new InvalidOperationException("The devicepath was already set.");
                _DevicePath = value;
            }
        }

        public MsHidDeviceInfo()
        {
        }

        public MsHidDeviceInfo(string devicePath)
        {
            _DevicePath = devicePath;
        }

        public override bool Equals(object obj)
        {
            MsHidDeviceInfo other = obj as MsHidDeviceInfo;
            if (other != null)
                return Equals(other);
            return base.Equals(obj);
        }

        public virtual bool Equals(MsHidDeviceInfo other)
        {
            return this.DevicePath == other.DevicePath;
        }

        public override int GetHashCode()
        {
            return DevicePath.GetHashCode();
        }

        public override string ToString()
        {
            return DevicePath;
        }
    }
}
