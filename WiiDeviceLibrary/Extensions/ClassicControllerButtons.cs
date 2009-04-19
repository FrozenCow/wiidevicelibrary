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

namespace WiiDeviceLibrary.Extensions
{
    /// <summary>
    /// An enumeration of the buttons on the classic controller.
    /// </summary>
    [Flags]
    public enum ClassicControllerButtons
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        ZR = 16,
        ZL = 32,
        A = 64,
        B = 128,
        X = 256,
        Y = 512,
        Plus = 1024,
        Home = 2048,
        Minus = 4096,
        LT = 8192,
        RT = 16384        
    }
}
