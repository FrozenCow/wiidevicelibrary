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
    /// <summary>
    /// An interface that extensions must implement to provide automatic creation
    /// of the extension class.
    /// </summary>
    public interface IWiimoteExtensionFactory
    {
        /// <summary>
        /// Creates an instance of an extension class that will be linked to
        /// the provided IWiimote.
        /// </summary>
        /// <param name="wiimote">The IWiimote instance that the extension will be linked to.</param>
        /// <returns>An instance of the an extension class.</returns>
        IWiimoteExtension Create(IWiimote wiimote);

        /// <summary>
        /// Gets the extension id that the wiimote will report when this extension will be connected.
        /// </summary>
        ushort ExtensionId
        {
            get;
        }
    }
}
