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
using System.Runtime.InteropServices;

namespace WiiDeviceLibrary.Bluetooth.Bluesoleil
{
    internal static class NativeMethods
    {
        #region Constants
        // General constants
        public const byte DEVICE_ADDRESS_LENGTH = 6;
        public const byte DEVICE_CLASS_LENGTH = 3;
        public const byte MAX_DEVICE_NAME_LENGTH = 64;
        public const byte MAX_PIN_CODE_LENGTH = 16;
        public const byte MAX_SERVICE_NAME_LENGTH = 128;

        //Connection state
        public enum STATE : sbyte
        {
            STATE_CONNECTED = 1,
            STATE_DISCONNECTED = 0
        }

        //Inquiry mode
        public enum INQUIRY : byte
        {
            INQUIRY_GENERAL_MODE = 0x00,
            INQUIRY_LIMITED_MODE = 0x01,
            INQUIRY_PAIRED = 0x02,
            INQUIRY_GENERAL_REFRESH = 0x03
        }


        //API calling status code
        public enum BTSTATUS : uint
        {
            BTSTATUS_FAIL = 0x00000000,
            BTSTATUS_SUCCESS = 0x00000001,
            BTSTATUS_SYSTEM_ERROR = 0x00000002,
            BTSTATUS_BT_NOT_READY = 0x00000003,
            BTSTATUS_ALREADY_PAIRED = 0x00000004,
            BTSTATUS_AUTHENTICATE_FAILED = 0x00000005,
            BTSTATUS_BT_BUSY = 0x00000006,
            BTSTATUS_CONNECTION_EXIST = 0x00000007,
            BTSTATUS_CONNECTION_NOT_EXIST = 0x00000008,
            BTSTATUS_PARAMETER_ERROR = 0x00000009,
            BTSTATUS_SERVICE_NOT_EXIST = 0x0000000a,
            BTSTATUS_DEVICE_NOT_EXIST = 0x0000000b
        }

        /////////////////////////////////////////////////////////////////////////////
        // DUN
        public const int DUN_MAX_NAME_LENGTH = 64;
        public const int DUN_MAX_PASSWORD_LENGTH = 64;
        public const int DUN_MAX_DIAL_NUMBER_LENGTH = 64;

        public const int DUN_SET_NONE = 0x0000;
        public const int DUN_SET_USER_NAME = 0x0001;
        public const int DUN_SET_PASSWORD = 0x0002;
        public const int DUN_SET_DIAL_NUMBER = 0x0004;

        /////////////////////////////////////////////////////////////////////////////
        // OPP
        public const int OPP_COMMAND_PUSH = 0x0001;
        public const int OPP_COMMAND_PULL = 0x0002;


        /////////////////////////////////////////////////////////////////////////////
        // SYNC
        public const int SYNC_VCARD = 0x0001;
        public const int SYNC_VCAL = 0x0002;
        public const int SYNC_NOTE = 0x0004;
        public const int SYNC_VMESSAGE = 0x0008;

        /////////////////////////////////////////////////////////////////////////////
        // Service class 16bits UUIDs
        public const int CLS_SERIAL_PORT = 0x1101;
        public const int CLS_LAN_ACCESS = 0x1102;
        public const int CLS_DIALUP_NET = 0x1103;
        public const int CLS_IRMC_SYNC = 0x1104;
        public const int CLS_OBEX_OBJ_PUSH = 0x1105;
        public const int CLS_OBEX_FILE_TRANS = 0x1106;
        public const int CLS_IRMC_SYNC_CMD = 0x1107;
        public const int CLS_HEADSET = 0x1108;
        public const int CLS_CORDLESS_TELE = 0x1109;
        public const int CLS_AUDIO_SOURCE = 0x110A;
        public const int CLS_AUDIO_SINK = 0x110B;
        public const int CLS_AVRCP_TG = 0x110C;
        public const int CLS_ADV_AUDIO_DISTRIB = 0x110D;
        public const int CLS_AVRCP_CT = 0x110E;
        public const int CLS_VIDEO_CONFERENCE = 0x110F;
        public const int CLS_INTERCOM = 0x1110;
        public const int CLS_FAX = 0x1111;
        public const int CLS_HEADSET_AG = 0x1112;
        public const int CLS_WAP = 0x1113;
        public const int CLS_WAP_CLIENT = 0x1114;

        public const int CLS_PAN_PANU = 0x1115;
        public const int CLS_PAN_NAP = 0x1116;
        public const int CLS_PAN_GN = 0x1117;

        public const int CLS_DIRECT_PRINT = 0x1118;
        public const int CLS_REF_PRINT = 0x1119;
        public const int CLS_IMAGING = 0x111A;
        public const int CLS_IMAG_RESPONDER = 0x111B;
        public const int CLS_IMAG_AUTO_ARCH = 0x111C;
        public const int CLS_IMAG_REF_OBJ = 0x111D;
        public const int CLS_HANDSFREE = 0x111E;
        public const int CLS_HANDSFREE_AG = 0x111F;
        public const int CLS_HID = 0x1124;
        public const int CLS_HCRP = 0x1125;
        public const int CLS_HCR_PRINT = 0x1126;
        public const int CLS_HCR_SCAN = 0x1127;
        public const int CLS_SIM_ACCESS = 0x112D;
        public const int CLS_PNP_INFO = 0x1200;
        public const int CLS_GENERIC_NET = 0x1201;
        public const int CLS_GENERIC_FILE_TRANS = 0x1202;
        public const int CLS_GENERIC_AUDIO = 0x1203;
        public const int CLS_GENERIC_TELE = 0x1204;


        //Mask for device information
        public const int MASK_DEVICE_NAME = 0x00000001;
        public const int MASK_DEVICE_CLASS = 0x00000002;
        public const int MASK_DEVICE_ADDRESS = 0x00000004;
        public const int MASK_LMP_VERSION = 0x00000008;
        public const int MASK_GET_DATA_COUNT = 0x00000010;
        public const int MASK_CONNECT_STATUS = 0x00000040;
        public const int MASK_PAIR_STATUS = 0x00000080;
        public const int MASK_CLOCK_OFFSET = 0x00000200;
        public const int MASK_DATA_RATE = 0x00000400;
        public const int MASK_SIGNAL_STRENGTH = 0x00000800;

        //Events to register
        public enum EVENT : sbyte
        {
            EVENT_CONNECTION_STATUS = 0x03,
            EVENT_DUN_RAS_CALLBACK = 0x04,
            EVENT_ERROR = 0x05,
            EVENT_INQUIRY_DEVICE_REPORT = 0x06,
            EVENT_SPPEX_CONNECTION_STATUS = 0x07,
            EVENT_BLUETOOTH_STATUS = 0x08
        }

        //For EVENT_CONNECTION_STATUS and EVENT_SPPEX_CONNECTION_STATUS
        public enum CONNECTION_STATUS : byte
        {
            STATUS_INCOMING_CONNECT = 0x01,
            STATUS_OUTGOING_CONNECT = 0x02,
            STATUS_INCOMING_DISCONNECT = 0x03,
            STATUS_OUTGOING_DISCONNECT = 0x04
        }

        //For EVENT_BLUETOOTH_STATUS
        public enum BLUETOOTH_STATUS : byte
        {
            STATUS_BLUETOOTH_STARTED = 0x01,
            STATUS_BLUETOOTH_STOPED = 0x02
        }
        #endregion
        #region Structures
        //typedef struct _BLUETOOTH_DEVICE_INFO {
        //    DWORD dwSize;
        //    BYTE address[DEVICE_ADDRESS_LENGTH];
        //    BYTE classOfDevice[DEVICE_CLASS_LENGTH];
        //    CHAR szName[MAX_DEVICE_NAME_LENGTH];
        //    BOOL bPaired;
        //} BLUETOOTH_DEVICE_INFO, *PBLUETOOTH_DEVICE_INFO;
        [StructLayout(LayoutKind.Sequential)]
        public struct BLUETOOTH_DEVICE_INFO
        {
            public uint dwSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] address;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] classOfDevice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szName;
            public bool bPaired;
        }

        //typedef struct _GENERAL_SERVICE_INFO {
        //    DWORD dwSize;
        //    DWORD dwServiceHandle;
        //    WORD wServiceClassUuid16;
        //    CHAR szServiceName[MAX_SERVICE_NAME_LENGTH];
        //} GENERAL_SERVICE_INFO,*PGENERAL_SERVICE_INFO;
        [StructLayout(LayoutKind.Sequential)]
        public struct GENERAL_SERVICE_INFO
        {
            public uint dwSize;
            public uint dwServiceHandle;
            public ushort wServiceClassUuid16;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] szServiceName;
        }
        #endregion
        #region Methods
        //BTEXPORT BOOL BT_InitializeLibrary();
        [DllImport("btfunc.dll")]
        public static extern bool BT_InitializeLibrary();

        //BTEXPORT void BT_UninitializeLibrary();
        [DllImport("btfunc.dll")]
        public static extern void BT_UninitializeLibrary();

        //BTEXPORT BOOL BT_IsBlueSoleilStarted(/* [in] */ DWORD dwSeconds );
        [DllImport("btfunc.dll")]
        public static extern bool BT_IsBlueSoleilStarted(int dwSeconds);

        //BTEXPORT BOOL BT_IsBluetoothReady (/* [in] */ DWORD dwSeconds );
        [DllImport("btfunc.dll")]
        public static extern bool BT_IsBluetoothReady(int dwSeconds);

        //BTEXPORT DWORD BT_StartBluetooth();
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_StartBluetooth();

        //BTEXPORT DWORD BT_StopBluetooth(BOOL bSwitch2HidMode);
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_StopBluetooth(bool bSwitch2HidMode);


        //BTEXPORT DWORD BT_InquireDevices(
        //                /* [in] */ UCHAR ucInqMode,
        //                /* [in] */ UCHAR ucInqTimeLen,
        //                /* [in, out] */ DWORD* lpDevsListLength,
        //                /* [out] */ PBLUETOOTH_DEVICE_INFO pDevsList
        //                );
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_InquireDevices(
            INQUIRY ucInqMode,
            byte ucInqTimeLen,
            ref long IpDevsListLength,
            IntPtr DevsList);
        public static BTSTATUS BT_InquireDevices(INQUIRY ucInqMode, byte ucInqTimeLen, out BLUETOOTH_DEVICE_INFO[] deviceInfos)
        {
            NativeMethods.BLUETOOTH_DEVICE_INFO DeviceInfo = new BLUETOOTH_DEVICE_INFO();
            long DeviceInfoSize = Marshal.SizeOf(DeviceInfo);
            int DeviceInfoCount = 1;
            long DeviceInfosSize = DeviceInfoCount * DeviceInfoSize;
            IntPtr DeviceInfosPtr = Marshal.AllocHGlobal((int)DeviceInfosSize);

            // Copy the struct to unmanaged memory.
            Marshal.StructureToPtr(DeviceInfo, DeviceInfosPtr, false);
            BTSTATUS result = NativeMethods.BT_InquireDevices(
                    ucInqMode,
                    ucInqTimeLen,
                    ref DeviceInfosSize,
                    DeviceInfosPtr);
            DeviceInfoCount = (int)(DeviceInfosSize / DeviceInfoSize);

            deviceInfos = new BLUETOOTH_DEVICE_INFO[DeviceInfoCount];
            for (int i = 0; i < DeviceInfoCount; i++)
            {
                DeviceInfo = (BLUETOOTH_DEVICE_INFO)Marshal.PtrToStructure(new IntPtr(DeviceInfosPtr.ToInt64() + i * DeviceInfoSize), typeof(BLUETOOTH_DEVICE_INFO));
                deviceInfos[i] = DeviceInfo;
            }
            return result;
        }

        //BTEXPORT DWORD BT_CancelInquiry();
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_CancelInquiry();

        //BTEXPORT DWORD BT_ConnectService(
        //               /* [in] */ PBLUETOOTH_DEVICE_INFO pDevInfo, 
        //               /* [in] */ PGENERAL_SERVICE_INFO pServiceInfo,
        //               /* [in, out] */ BYTE* lpParam,
        //               /* [out] */ DWORD* lpConnectionHandle
        //               );
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_ConnectService(
            IntPtr pDevInfo,
            IntPtr pServiceInfo,
            IntPtr lpParam,
            ref int lpConnectionHandle
            );
        public static BTSTATUS BT_ConnectService(BLUETOOTH_DEVICE_INFO DeviceInfo, GENERAL_SERVICE_INFO ServiceInfo, ref int lpConnectionHandle)
        {
            int DeviceInfoSize = Marshal.SizeOf(DeviceInfo);
            IntPtr DeviceInfoPtr = Marshal.AllocHGlobal(DeviceInfoSize);
            Marshal.StructureToPtr(DeviceInfo, DeviceInfoPtr, false);

            int ServiceInfoSize = Marshal.SizeOf(ServiceInfo);
            IntPtr ServiceInfoPtr = Marshal.AllocHGlobal(ServiceInfoSize);
            Marshal.StructureToPtr(ServiceInfo, ServiceInfoPtr, false);

            return BT_ConnectService(DeviceInfoPtr, ServiceInfoPtr, IntPtr.Zero, ref lpConnectionHandle);
        }

        //DWORD BT_DisconnectService (/* [in] */ DWORD dwConnectionHandle);
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_DisconnectService(int dwConnectionHandle);

        //BTEXPORT DWORD BT_BrowseServices(
        //                /* [in] */ PBLUETOOTH_DEVICE_INFO pDevInfo,
        //                /* [in] */ BOOL bBrowseAllServices,
        //                /* [in][out] */ DWORD* lpServiceClassListLength,
        //                /* [in][out] */ PGENERAL_SERVICE_INFO pSeriveClassList
        //                );
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_BrowseServices(
            IntPtr pDevInfo,
            bool bBrowseAllServices,
            ref int lpServiceClassListLength,
            IntPtr pSeriveClassList);
        public static BTSTATUS BT_BrowseServices(BLUETOOTH_DEVICE_INFO DeviceInfo, bool bBrowseAllServices, out GENERAL_SERVICE_INFO[] ServiceInfos)
        {
            int DeviceInfoSize = Marshal.SizeOf(DeviceInfo);
            IntPtr DeviceInfoPtr = Marshal.AllocHGlobal(DeviceInfoSize);
            Marshal.StructureToPtr(DeviceInfo, DeviceInfoPtr, false);

            GENERAL_SERVICE_INFO ServiceInfo = new GENERAL_SERVICE_INFO();
            int ServiceInfoCount = 16;
            int ServiceInfoSize = Marshal.SizeOf(ServiceInfo);
            int ServiceInfosSize = ServiceInfoSize * ServiceInfoCount;
            IntPtr ServiceInfosPtr = Marshal.AllocHGlobal(ServiceInfosSize);
            Marshal.StructureToPtr(ServiceInfo, ServiceInfosPtr, false);

            ServiceInfoCount = 0;

            BTSTATUS result = NativeMethods.BT_BrowseServices(
                DeviceInfoPtr,
                bBrowseAllServices,
                ref ServiceInfosSize,
                ServiceInfosPtr);

            ServiceInfoCount = ServiceInfosSize / ServiceInfoSize;

            ServiceInfos = new GENERAL_SERVICE_INFO[ServiceInfoCount];
            for (int i = 0; i < ServiceInfoCount; i++)
            {
                ServiceInfos[i] = (GENERAL_SERVICE_INFO)Marshal.PtrToStructure(new IntPtr(ServiceInfosPtr.ToInt64() + i * ServiceInfoSize), typeof(GENERAL_SERVICE_INFO));
            }
            return result;
        }

        //typedef void (*PCALLBACK_ERROR) (DWORD dwErrorCode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PCALLBACK_ERROR(uint dwErrorCode);

        //typedef void (*PCALLBACK_BLUETOOTH_STATUS) (UCHAR ucStatus);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PCALLBACK_BLUETOOTH_STATUS(BLUETOOTH_STATUS ucStatus);

        //typedef void (*PCALLBACK_INQUIRY_DEVICE) (PBLUETOOTH_DEVICE_INFO pDevInfo);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PCALLBACK_INQUIRY_DEVICE(IntPtr pDevInfo);

        //typedef void (*PCALLBACK_CONNECTION_STATUS) (WORD wServiceClass, BYTE* lpBdAddr, UCHAR ucStatus, DWORD dwConnetionHandle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PCALLBACK_CONNECTION_STATUS(short wServiceClass, IntPtr lpBdAddr, CONNECTION_STATUS ucStatus, int dwConnetionHandle);

        //BTEXPORT DWORD BT_RegisterCallback(
        //                          /* [in] */ UCHAR ucEvent,
        //                          /* [in] */ LPVOID pfnCbkFun
        //                          );
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_RegisterCallback(EVENT ucEvent, PCALLBACK_ERROR pfnCbkFun);

        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_RegisterCallback(EVENT ucEvent, PCALLBACK_INQUIRY_DEVICE pfnCbkFun);

        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_RegisterCallback(EVENT ucEvent, PCALLBACK_CONNECTION_STATUS pfnCbkFun);

        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_RegisterCallback(EVENT ucEvent, PCALLBACK_BLUETOOTH_STATUS pfnCbkFun);


        //BTEXPORT DWORD BT_UnregisterCallback (
        //                          /* [in] */ UCHAR ucEvent
        //                          );
        [DllImport("btfunc.dll")]
        public static extern BTSTATUS BT_UnregisterCallback(EVENT ucEvent);
        #endregion
    }
}
