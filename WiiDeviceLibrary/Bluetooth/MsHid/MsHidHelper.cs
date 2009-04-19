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
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public static class MsHidHelper
    {
        // Most of the following code is from Managed Library for Nintendo's Wiimote by Brian Peek (http://www.brianpeek.com/)
        //Microsoft Permissive License (Ms-PL)

        //This license governs use of the accompanying software. If you use the software,
        //you accept this license. If you do not accept the license, do not use the
        //software.

        //1. Definitions

        //The terms "reproduce," "reproduction," "derivative works," and "distribution"
        //have the same meaning here as under U.S. copyright law.

        //A "contribution" is the original software, or any additions or changes to the
        //software.

        //A "contributor" is any person that distributes its contribution under this
        //license.

        //"Licensed patents" are a contributor's patent claims that read directly on its
        //contribution.

        //2. Grant of Rights

        //(A) Copyright Grant- Subject to the terms of this license, including the license
        //conditions and limitations in section 3, each contributor grants you a
        //non-exclusive, worldwide, royalty-free copyright license to reproduce its
        //contribution, prepare derivative works of its contribution, and distribute its
        //contribution or any derivative works that you create.

        //(B) Patent Grant- Subject to the terms of this license, including the license
        //conditions and limitations in section 3, each contributor grants you a
        //non-exclusive, worldwide, royalty-free license under its licensed patents to
        //make, have made, use, sell, offer for sale, import, and/or otherwise dispose of
        //its contribution in the software or derivative works of the contribution in the
        //software.

        //3. Conditions and Limitations

        //(A) No Trademark License- This license does not grant you rights to use any
        //contributors' name, logo, or trademarks.

        //(B) If you bring a patent claim against any contributor over patents that you
        //claim are infringed by the software, your patent license from such contributor
        //to the software ends automatically.

        //(C) If you distribute any portion of the software, you must retain all
        //copyright, patent, trademark, and attribution notices that are present in the
        //software.

        //(D) If you distribute any portion of the software in source code form, you may
        //do so only under this license by including a complete copy of this license with
        //your distribution. If you distribute any portion of the software in compiled or
        //object code form, you may only do so under a license that complies with this
        //license.

        //(E) The software is licensed "as-is." You bear the risk of using it. The
        //contributors give no express warranties, guarantees or conditions. You may have
        //additional consumer rights under your local laws which this license cannot
        //change. To the extent permitted under your local laws, the contributors exclude
        //the implied warranties of merchantability, fitness for a particular purpose and
        //non-infringement.

        public static IEnumerable<string> GetDevicePaths()
        {
            int index = 0;
            Guid guid;

            // get the GUID of the HID class
            NativeMethods.HidD_GetHidGuid(out guid);

            // get a handle to all devices that are part of the HID class
            // Fun fact:  DIGCF_PRESENT worked on my machine just fine.  I reinstalled Vista, and now it no longer finds the Wiimote with that parameter enabled...
            IntPtr hDevInfo = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, NativeMethods.DIGCF_DEVICEINTERFACE);// | HIDImports.DIGCF_PRESENT);

            // create a new interface data struct and initialize its size
            NativeMethods.SP_DEVICE_INTERFACE_DATA diData = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);

            // get a device interface to a single device (enumerate all devices)
            while (NativeMethods.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData))
            {
                UInt32 size;

                // get the buffer size for this device detail instance (returned in the size parameter)
                NativeMethods.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

                // create a detail struct and set its size
                NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA();

                // yeah, yeah...well, see, on Win x86, cbSize must be 5 for some reason.  On x64, apparently 8 is what it wants.
                // someday I should figure this out.  Thanks to Paul Miller on this...
                if (IntPtr.Size == 8)
                    diDetail.cbSize = 8;
                else
                    diDetail.cbSize = 5;

                // actually get the detail struct
                if (NativeMethods.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero))
                {
                    yield return diDetail.DevicePath;
                }

                index++;
            }
        }

        public static SafeFileHandle CreateFileHandle(string devicePath)
        {
            // open a read/write handle to our device using the DevicePath returned
            return NativeMethods.CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, NativeMethods.EFileAttributes.Overlapped, IntPtr.Zero);
        }

        public static bool TryGetHidInfo(SafeFileHandle hidHandle, out int vendorId, out int productId)
        {
            // create an attributes struct and initialize the size
            NativeMethods.HIDD_ATTRIBUTES attrib = new NativeMethods.HIDD_ATTRIBUTES();
            attrib.Size = Marshal.SizeOf(attrib);

            // get the attributes of the current device
            if (NativeMethods.HidD_GetAttributes(hidHandle.DangerousGetHandle(), ref attrib))
            {
                // if the vendor and product IDs match up
                vendorId = attrib.VendorID;
                productId = attrib.ProductID;
                return true;
            }
            else
            {
                vendorId = 0;
                productId = 0;
                return false;
            }
        }
    }
}
