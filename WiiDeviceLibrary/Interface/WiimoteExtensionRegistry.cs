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
    public static class WiimoteExtensionRegistry
    {
        private static IDictionary<ushort, IWiimoteExtensionFactory> _Factories = new Dictionary<ushort, IWiimoteExtensionFactory>();
        public static ICollection<IWiimoteExtensionFactory> Factories
        {
            get { return _Factories.Values; }
        }

        static WiimoteExtensionRegistry()
        {
            Register(new WiiDeviceLibrary.Extensions.NunchukExtensionFactory());
            Register(new WiiDeviceLibrary.Extensions.ClassicControllerExtensionFactory());
            Register(new WiiDeviceLibrary.Extensions.GuitarExtensionFactory());
        }

        public static void Register(IWiimoteExtensionFactory extensionFactory)
        {
            _Factories.Add(extensionFactory.ExtensionId, extensionFactory);
        }

        public static void Unregister(IWiimoteExtensionFactory extensionFactory)
        {
            _Factories.Remove(extensionFactory.ExtensionId);
        }

        public static bool TryGetValue(ushort extensionId, out IWiimoteExtensionFactory extensionFactory)
        {
            return _Factories.TryGetValue(extensionId, out extensionFactory);
        }
    }
}
