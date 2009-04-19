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
    public static class DeviceProviderRegistry
    {
        static IList<IDeviceProviderFactory> _Factories = new List<IDeviceProviderFactory>();
        public static IList<IDeviceProviderFactory> Factories
        {
            get { return _Factories; }
        }

        static DeviceProviderRegistry()
        {
            Factories.Add(new WiiDeviceLibrary.Bluetooth.Bluez.BluezDeviceProviderFactory());
            Factories.Add(new WiiDeviceLibrary.Bluetooth.Bluesoleil.BluesoleilDeviceProviderFactory());
            Factories.Add(new WiiDeviceLibrary.Bluetooth.MsBluetooth.MsBluetoothDeviceProviderFactory());
            Factories.Add(new WiiDeviceLibrary.Bluetooth.MsHid.MsHidDeviceProviderFactory());
        }

        public static IDeviceProviderFactory GetSupportedFactory()
        {
            foreach (IDeviceProviderFactory factory in Factories)
            {
                if (factory.IsSupported)
                    return factory;
            }
            return null;
        }

        public static IDeviceProvider CreateSupportedDeviceProvider()
        {
            IDeviceProviderFactory factory = GetSupportedFactory();
            if (factory == null)
                return null;
            return factory.Create();
        }
    }
}
