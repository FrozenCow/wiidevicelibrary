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

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    
    [global::System.Serializable]
    public class BluesoleilException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BluesoleilException() { }
        public BluesoleilException(string message) : base(message) { }
        public BluesoleilException(string message, Exception inner) : base(message, inner) { }
        protected BluesoleilException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class BluesoleilFailException : BluesoleilException
    {
    }

    public class BluesoleilSystemException : BluesoleilException
    {
    }

    public class BluesoleilNotReadyException : BluesoleilException
    {
    }

    public class BluesoleilAlreadyPairedException : BluesoleilException
    {
    }

    public class BluesoleilAuthenticateException : BluesoleilException
    {
    }

    public class BluesoleilBluetoothBusyException : BluesoleilException
    {
    }

    public class BluesoleilParameterException : BluesoleilException
    {
    }

    public class BluesoleilNonExistingServiceException : BluesoleilException
    {
    }

    public class BluesoleilNonExistingDeviceException : BluesoleilException
    {
    }

    public class BluesoleilNonExistingConnectionException : BluesoleilException
    {
    }

}
