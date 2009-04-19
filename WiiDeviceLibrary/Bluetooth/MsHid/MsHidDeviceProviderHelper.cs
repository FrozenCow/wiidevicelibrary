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
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public static class MsHidDeviceProviderHelper
    {
        private static IDictionary<string, bool> connectedDevicePaths = new Dictionary<string, bool>();
        public static void SetDevicePathConnected(string devicePath, bool isConnected)
        {
            lock (connectedDevicePaths)
            {
                connectedDevicePaths[devicePath.ToLowerInvariant()] = isConnected;
            }
        }

        public static bool IsDevicePathConnected(string devicePath)
        {
            bool result;
            lock (connectedDevicePaths)
            {
                if (connectedDevicePaths.TryGetValue(devicePath.ToLowerInvariant(), out result))
                    return result;
            }
            return false;
        }

        public static IEnumerable<KeyValuePair<string, SafeFileHandle>> GetWiiDeviceHandles()
        {
            foreach (string devicePath in MsHidHelper.GetDevicePaths())
            {
                SafeFileHandle fileHandle = MsHidHelper.CreateFileHandle(devicePath);

                int vendorId, productId;
                if (MsHidHelper.TryGetHidInfo(fileHandle, out vendorId, out productId))
                {
                    if (IsDevicePathConnected(devicePath))
                        continue;
                    yield return new KeyValuePair<string, SafeFileHandle>(devicePath, fileHandle);
                }
            }
        }

        public static bool TryConnect(ReportDevice device, Stream deviceStream, string devicePath, SafeFileHandle fileHandle)
        {
            bool success;
            try
            {
                device.Initialize();

                ((MsHidDeviceInfo)device.DeviceInfo).DevicePath = devicePath;
                SetDevicePathConnected(devicePath, true);
                success = true;
            }
            catch (TimeoutException)
            {
                device.Disconnect();
                deviceStream.Dispose();
                fileHandle.Close();
                success = false;
            }
            return success;
        }

        // TODO: Make this compatible with ReportBalanceBoard too.
        public static bool TryConnectWiimote(MsHidDeviceInfo deviceInfo, out ReportWiimote wiimote)
        {
            wiimote = null;
            foreach (string devicePath in MsHidHelper.GetDevicePaths())
            {
                SafeFileHandle fileHandle = MsHidHelper.CreateFileHandle(devicePath);

                int vendorId, productId;
                if (MsHidHelper.TryGetHidInfo(fileHandle, out vendorId, out productId))
                {
                    if (IsDevicePathConnected(devicePath))
                        continue;
                    Stream communicationStream = new UnsureMsHidStream(fileHandle);
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

                    deviceInfo.DevicePath = devicePath;
                    SetDevicePathConnected(devicePath, true);
                    break;
                }
                fileHandle.Close();
            }
            return wiimote != null;
        }
    }
}
