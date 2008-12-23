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
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public delegate Stream HidStreamHandler(SafeFileHandle fileHandle);
    public static class MsHidWiiProviderHelper
    {
        public static bool TryConnectWiimote(IDeviceInfo deviceInfo, HidStreamHandler createCommunicationStream, out ReportWiimote wiimote)
        {
            wiimote = null;
            foreach (string devicePath in MsHidHelper.GetDevicePaths())
            {
                SafeFileHandle fileHandle = MsHidHelper.CreateFileHandle(devicePath);

                int vendorId, productId;
                if (MsHidHelper.TryGetHidInfo(fileHandle, out vendorId, out productId))
                {
                    Stream communicationStream = createCommunicationStream(fileHandle);
                    wiimote = new ReportWiimote(deviceInfo, communicationStream);
                    try
                    {
                        wiimote.Initialize();
                    }
                    catch (TimeoutException)
                    {
                        wiimote.Disconnect();
                        communicationStream.Dispose();
                        wiimote = null;
                        continue;
                    }
                    break;
                }
                fileHandle.Close();
            }
            return wiimote != null;
        }
    }
}
