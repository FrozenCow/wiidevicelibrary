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

namespace WiiDeviceLibrary
{
    /// <summary>
    /// The report format in which the Wiimote should return data.
    /// </summary>	
    public enum InputReport
    {
        None =                                  0x00,
        GetStatusResult =                       0x20,
        ReadDataResult =                        0x21,
        WriteDataResult =                       0x22,
        Buttons =                               0x30,
        ButtonsAccelerometer =                  0x31,
        Buttons8Extension =                     0x32,
        ButtonsAccelerometer12Ir =              0x33,
        Buttons19Extension =                    0x34,
        ButtonsAccelerometer16Extension =       0x35,
        Buttons10Ir9Extension =                 0x36,
        ButtonsAccelerometer10Ir6Extension =    0x37,
        Extension =                             0x3d,
        ButtonsAccelerometer36IrA =             0x3e,
        ButtonsAccelerometer36IrB =             0x3f
    }
}
