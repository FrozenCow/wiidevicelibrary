//////////////////////////////////////////////////////////////////////////////////
//	HIDImports.cs
//	Managed Wiimote Library
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for MSDN's Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//	Visit http://blogs.msdn.com/coding4fun/archive/2007/03/14/1879033.aspx
//	for more information
//////////////////////////////////////////////////////////////////////////////////
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

using System;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    /// <summary>
    /// Win32 import information for use with the Wiimote library
    /// </summary>
    internal static class NativeMethods
    {
        //
        // Flags controlling what is included in the device information set built
        // by SetupDiGetClassDevs
        //
        public const int DIGCF_DEFAULT = 0x00000001; // only valid with DIGCF_DEVICEINTERFACE
        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_ALLCLASSES = 0x00000004;
        public const int DIGCF_PROFILE = 0x00000008;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

        [Flags]
        internal enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDD_ATTRIBUTES
        {
            public int Size;
            public short VendorID;
            public short ProductID;
            public short VersionNumber;
        }

        [DllImport(@"hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void HidD_GetHidGuid(out Guid gHid);

        [DllImport("hid.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);


        //BOOLEAN __stdcall
        //HidD_GetInputReport(
        //    IN HANDLE  HidDeviceObject,
        //    IN OUT  PVOID  ReportBuffer,
        //    IN ULONG  ReportBufferLength
        //    );
        [DllImport("hid.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        internal extern static bool HidD_GetInputReport(
            IntPtr HidDeviceObject,
            out byte[] lpReportBuffer,
            uint ReportBufferLength);

        //BOOLEAN __stdcall
        //  HidD_SetOutputReport(
        //    IN HANDLE  HidDeviceObject,
        //    IN PVOID  ReportBuffer,
        //    IN ULONG  ReportBufferLength
        //    );
        [DllImport("hid.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        internal extern static bool HidD_SetOutputReport(
            IntPtr HidDeviceObject,
            byte[] lpReportBuffer,
            uint ReportBufferLength);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
            IntPtr hwndParent,
            UInt32 Flags
            );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            //ref SP_DEVINFO_DATA devInfo,
            IntPtr devInvo,
            ref Guid interfaceClassGuid,
            Int32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        [DllImport(@"setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            IntPtr deviceInfoData
        );

        [DllImport(@"setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            IntPtr deviceInfoData
        );

        // Incorrect.
        //[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);


        public struct WSAData
        {
            public short wVersion;
            public short wHighVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
            public string Description;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string Status;
            public int MaxSockets;
            public int MaxUdpDg;
            public IntPtr vendorInfoPointer;

            public WSAData(short version, short highVersion)
            {
                wVersion = 0;
                wHighVersion = 0;
                Description = null;
                Status = null;
                MaxSockets = 0;
                MaxUdpDg = 0;
                vendorInfoPointer = IntPtr.Zero;
            }
        }

        public static void WSAStartup()
        {
            WSAData data = new WSAData(2, 2);

            int result = WSAStartup(36, ref data);
            if (result == 0) // Success
                WSACleanup();
            else
                throw new System.ComponentModel.Win32Exception(result);
        }

        [DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Int32 WSAStartup(Int16 wVersionRequested, ref WSAData wsaData);

        [DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Int32 WSACleanup();
    }
}
