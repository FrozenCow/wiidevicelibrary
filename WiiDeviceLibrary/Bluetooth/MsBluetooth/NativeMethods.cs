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
using System.ComponentModel;
using System.Threading;

namespace WiiDeviceLibrary.Bluetooth.MsBluetooth
{
    internal static class BluetoothServices
    {
        public static readonly Guid ServiceDiscoveryServerServiceClassID_UUID = new Guid(0x00001000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid BrowseGroupDescriptorServiceClassID_UUID = new Guid(0x00001001, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid PublicBrowseGroupServiceClass_UUID = new Guid(0x00001002, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid SerialPortServiceClass_UUID = new Guid(0x00001101, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid LANAccessUsingPPPServiceClass_UUID = new Guid(0x00001102, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid DialupNetworkingServiceClass_UUID = new Guid(0x00001103, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid IrMCSyncServiceClass_UUID = new Guid(0x00001104, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid OBEXObjectPushServiceClass_UUID = new Guid(0x00001105, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid OBEXFileTransferServiceClass_UUID = new Guid(0x00001106, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid IrMCSyncCommandServiceClass_UUID = new Guid(0x00001107, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HeadsetServiceClass_UUID = new Guid(0x00001108, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid CordlessTelephonyServiceClass_UUID = new Guid(0x00001109, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AudioSourceServiceClass_UUID = new Guid(0x0000110A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AudioSinkServiceClass_UUID = new Guid(0x0000110B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AVRemoteControlTargetServiceClass_UUID = new Guid(0x0000110C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AdvancedAudioDistributionServiceClass_UUID = new Guid(0x0000110D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AVRemoteControlServiceClass_UUID = new Guid(0x0000110E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid VideoConferencingServiceClass_UUID = new Guid(0x0000110F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid IntercomServiceClass_UUID = new Guid(0x00001110, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid FaxServiceClass_UUID = new Guid(0x00001111, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HeadsetAudioGatewayServiceClass_UUID = new Guid(0x00001112, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid WAPServiceClass_UUID = new Guid(0x00001113, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid WAPClientServiceClass_UUID = new Guid(0x00001114, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid PANUServiceClass_UUID = new Guid(0x00001115, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid NAPServiceClass_UUID = new Guid(0x00001116, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid GNServiceClass_UUID = new Guid(0x00001117, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid DirectPrintingServiceClass_UUID = new Guid(0x00001118, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ReferencePrintingServiceClass_UUID = new Guid(0x00001119, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ImagingServiceClass_UUID = new Guid(0x0000111A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ImagingResponderServiceClass_UUID = new Guid(0x0000111B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ImagingAutomaticArchiveServiceClass_UUID = new Guid(0x0000111C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ImagingReferenceObjectsServiceClass_UUID = new Guid(0x0000111D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HandsfreeServiceClass_UUID = new Guid(0x0000111E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HandsfreeAudioGatewayServiceClass_UUID = new Guid(0x0000111F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid DirectPrintingReferenceObjectsServiceClass_UUID = new Guid(0x00001120, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid ReflectedUIServiceClass_UUID = new Guid(0x00001121, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid BasicPringingServiceClass_UUID = new Guid(0x00001122, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid PrintingStatusServiceClass_UUID = new Guid(0x00001123, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HumanInterfaceDeviceServiceClass_UUID = new Guid(0x00001124, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HardcopyCableReplacementServiceClass_UUID = new Guid(0x00001125, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HCRPrintServiceClass_UUID = new Guid(0x00001126, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid HCRScanServiceClass_UUID = new Guid(0x00001127, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid CommonISDNAccessServiceClass_UUID = new Guid(0x00001128, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid VideoConferencingGWServiceClass_UUID = new Guid(0x00001129, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid UDIMTServiceClass_UUID = new Guid(0x0000112A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid UDITAServiceClass_UUID = new Guid(0x0000112B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid AudioVideoServiceClass_UUID = new Guid(0x0000112C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid PnPInformationServiceClass_UUID = new Guid(0x00001200, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid GenericNetworkingServiceClass_UUID = new Guid(0x00001201, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid GenericFileTransferServiceClass_UUID = new Guid(0x00001202, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid GenericAudioServiceClass_UUID = new Guid(0x00001203, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        public static readonly Guid GenericTelephonyServiceClass_UUID = new Guid(0x00001204, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }

    internal enum BluetoothError : int
    {
        BTH_ERROR_SUCCESS = 0x00,
        BTH_ERROR_UNKNOWN_HCI_COMMAND = 0x01,
        BTH_ERROR_NO_CONNECTION = 0x02,
        BTH_ERROR_HARDWARE_FAILURE = 0x03,
        BTH_ERROR_PAGE_TIMEOUT = 0x04,
        BTH_ERROR_AUTHENTICATION_FAILURE = 0x05,
        BTH_ERROR_KEY_MISSING = 0x06,
        BTH_ERROR_MEMORY_FULL = 0x07,
        BTH_ERROR_CONNECTION_TIMEOUT = 0x08,
        BTH_ERROR_MAX_NUMBER_OF_CONNECTIONS = 0x09,
        BTH_ERROR_MAX_NUMBER_OF_SCO_CONNECTIONS = 0x0a,
        BTH_ERROR_ACL_CONNECTION_ALREADY_EXISTS = 0x0b,
        BTH_ERROR_COMMAND_DISALLOWED = 0x0c,
        BTH_ERROR_HOST_REJECTED_LIMITED_RESOURCES = 0x0d,
        BTH_ERROR_HOST_REJECTED_SECURITY_REASONS = 0x0e,
        BTH_ERROR_HOST_REJECTED_PERSONAL_DEVICE = 0x0f,
        BTH_ERROR_HOST_TIMEOUT = 0x10,
        BTH_ERROR_UNSUPPORTED_FEATURE_OR_PARAMETER = 0x11,
        BTH_ERROR_INVALID_HCI_PARAMETER = 0x12,
        BTH_ERROR_REMOTE_USER_ENDED_CONNECTION = 0x13,
        BTH_ERROR_REMOTE_LOW_RESOURCES = 0x14,
        BTH_ERROR_REMOTE_POWERING_OFF = 0x15,
        BTH_ERROR_LOCAL_HOST_TERMINATED_CONNECTION = 0x16,
        BTH_ERROR_REPEATED_ATTEMPTS = 0x17,
        BTH_ERROR_PAIRING_NOT_ALLOWED = 0x18,
        BTH_ERROR_UKNOWN_LMP_PDU = 0x19,
        BTH_ERROR_UNSUPPORTED_REMOTE_FEATURE = 0x1a,
        BTH_ERROR_SCO_OFFSET_REJECTED = 0x1b,
        BTH_ERROR_SCO_INTERVAL_REJECTED = 0x1c,
        BTH_ERROR_SCO_AIRMODE_REJECTED = 0x1d,
        BTH_ERROR_INVALID_LMP_PARAMETERS = 0x1e,
        BTH_ERROR_UNSPECIFIED_ERROR = 0x1f,
        BTH_ERROR_UNSUPPORTED_LMP_PARM_VALUE = 0x20,
        BTH_ERROR_ROLE_CHANGE_NOT_ALLOWED = 0x21,
        BTH_ERROR_LMP_RESPONSE_TIMEOUT = 0x22,
        BTH_ERROR_LMP_TRANSACTION_COLLISION = 0x23,
        BTH_ERROR_LMP_PDU_NOT_ALLOWED = 0x24,
        BTH_ERROR_ENCRYPTION_MODE_NOT_ACCEPTABLE = 0x25,
        BTH_ERROR_UNIT_KEY_NOT_USED = 0x26,
        BTH_ERROR_QOS_IS_NOT_SUPPORTED = 0x27,
        BTH_ERROR_INSTANT_PASSED = 0x28,
        BTH_ERROR_PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED = 0x29,
        BTH_ERROR_UNSPECIFIED = 0xFF
    }

    internal static class NativeMethods
    {
        public const int BLUETOOTH_MAX_NAME_SIZE = 248;

        public static void HandleError()
        {
            int result = Marshal.GetLastWin32Error();
            if (result != 0 && result != 259 && result != 1008)
            {
                throw new Win32Exception(result);
            }
        }

        public static void HandleError(int result)
        {
            if (Enum.IsDefined(typeof(BluetoothError), result))
            {
                BluetoothError error = (BluetoothError)result;
                if (error != BluetoothError.BTH_ERROR_SUCCESS)
                    throw new Win32Exception(error.ToString());
            }
            if (result == 0)
                result = Marshal.GetLastWin32Error();
            if (result != 0 && result != 1008 && result != 1168)
                throw new Win32Exception(result);

        }

        public static void RemoveDevice(BluetoothAddress address)
        {
            byte[] a = address.address;
            int result = BluetoothRemoveDevice(a);
            HandleError(result);
        }

        public static IEnumerable<BluetoothDeviceInfo> GetDeviceInfos(bool returnAuthenticated, bool returnConnected, bool returnRemembered, bool returnUnknown, bool issueInquiry, byte timeoutMultiplier)
        {
            BluetoothDeviceSearchParams searchParams = new BluetoothDeviceSearchParams();
            searchParams.returnAuthenticated = returnAuthenticated;
            searchParams.returnConnected = returnConnected;
            searchParams.returnRemembered = returnRemembered;
            searchParams.returnUnknown = returnUnknown;
            searchParams.issueInquiry = issueInquiry;
            searchParams.timeoutMultiplier = timeoutMultiplier;
            searchParams.size = (uint)Marshal.SizeOf(searchParams);
            return GetDeviceInfos(searchParams);
        }

        public static IEnumerable<BluetoothDeviceInfo> GetDeviceInfos(BluetoothDeviceSearchParams searchParams)
        {
            IList<BluetoothDeviceInfo> deviceInfos = new List<BluetoothDeviceInfo>();
            BluetoothDeviceInfo deviceInfo = default(BluetoothDeviceInfo);
            deviceInfo.size = (uint)Marshal.SizeOf(deviceInfo);
            IntPtr searchHandle = BluetoothFindFirstDevice(ref searchParams, ref deviceInfo);
            HandleError();
            if (searchHandle != IntPtr.Zero)
            {
                do
                {
                    deviceInfos.Add(deviceInfo);
                    deviceInfo = default(BluetoothDeviceInfo);
                }
                while (BluetoothFindNextDevice(searchHandle, ref deviceInfo));
                BluetoothFindDeviceClose(searchHandle);
            }
            return deviceInfos;
        }

        public static IEnumerable<IntPtr> GetRadioHandles(BluetoothFindRadioParams searchParams)
        {
            IList<IntPtr> radioHandles = new List<IntPtr>();
            IntPtr radioHandle = IntPtr.Zero;
            IntPtr searchHandle = BluetoothFindFirstRadio(ref searchParams, ref radioHandle);
            if (searchHandle != IntPtr.Zero)
            {
                do
                {
                    radioHandles.Add(radioHandle);
                    radioHandle = IntPtr.Zero;
                }
                while (BluetoothFindNextRadio(searchHandle, ref radioHandle));
                BluetoothFindRadioClose(searchHandle);
            }
            return radioHandles;
        }

        public static BluetoothRadioInfo GetRadioInfo(IntPtr radioHandle)
        {
            BluetoothRadioInfo radioInfo = default(BluetoothRadioInfo);
            int result = BluetoothGetRadioInfo(radioHandle, ref radioInfo);
            HandleResult(result);
            //if (result != 0)
            //    throw new InvalidOperationException();
            return radioInfo;
        }

        public static IEnumerable<BluetoothRadioInfo> GetRadioInfos(BluetoothFindRadioParams searchParams)
        {
            IList<BluetoothRadioInfo> radioInfos = new List<BluetoothRadioInfo>();
            foreach (IntPtr radioHandle in GetRadioHandles(searchParams))
            {
                radioInfos.Add(GetRadioInfo(radioHandle));
            }
            return radioInfos;
        }

        public static IEnumerable<Guid> GetServiceGuids(BluetoothDeviceInfo deviceInfo)
        {
            int recordCount = 16;
            byte[] servicesBytes = new byte[recordCount * 32];
            int result = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref deviceInfo, ref recordCount, servicesBytes);
            HandleResult(result);

            Guid serviceGuid;
            Guid[] services = new Guid[recordCount];
            for (int i = 0; i < recordCount; i++)
            {
                byte[] serviceBytes = new byte[16];
                Array.Copy(servicesBytes, i * 16, serviceBytes, 0, 16);
                serviceGuid = new Guid(serviceBytes);
                services[i] = serviceGuid;
            }
            return services;
        }

        /// <Summary>
        /// Sends an authentication request to a remote Bluetooth device.
        /// </Summary>
        //DWORD BluetoothAuthenticateDevice(
        //    HWND hwndParent,
        //    HANDLE hRadio,
        //    BLUETOOTH_DEVICE_INFO* pbtdi,
        //    PWCHAR pszPasskey,
        //    ULONG ulPasskeyLength
        //);
        [DllImport("Bthprops.cpl", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int BluetoothAuthenticateDevice(
               IntPtr hwndParent,
               IntPtr hRadio,
               ref BluetoothDeviceInfo pbtdi,
               byte[] pszPasskey,
               ulong ulPassKeyLength
            );

        /// <Summary>
        /// Enables the caller to prompt for multiple devices to be authenticated during a single instance of the Bluetooth Connection Wizard.
        /// </Summary>
        //DWORD BluetoothAuthenticateMultipleDevices(
        //    HWND hwndParent,
        //    HANDLE hRadio,
        //    DWORD cDevices,
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothAuthenticateMultipleDevices();

        /// <Summary>
        /// Invokes the Control Panel device information property sheet.
        /// </Summary>
        //BOOL BluetoothDisplayDeviceProperties(
        //    HWND hwndParent,
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothDisplayDeviceProperties(
            IntPtr hwndParent,
            ref BluetoothDeviceInfo pbtdi
            );

        /// <Summary>
        /// Changes the discovery state of a local Bluetooth radio or radios.
        /// </Summary>
        //BOOL BluetoothEnableDiscovery(
        //    HANDLE hRadio,
        //    BOOL fEnabled
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothEnableDiscovery();

        /// <Summary>
        /// Modifies whether a local Bluetooth radio accepts incoming connections.
        /// </Summary>
        //BOOL BluetoothEnableIncomingConnections(
        //    HANDLE hRadio,
        //    BOOL fEnabled
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothEnableIncomingConnections();

        /// <Summary>
        /// Enumerates the GUIDs (globally unique identifiers) of the services that are enabled on a Bluetooth device.
        /// </Summary>
        //DWORD BluetoothEnumerateInstalledServices(
        //    HANDLE hRadio,
        //    BLUETOOTH_DEVICE_INFO* pbtdi,
        //    DWORD* pcServices,
        //    GUID* pGuidServices
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothEnumerateInstalledServices(
            IntPtr hRadio,
            ref BluetoothDeviceInfo pbtdi,
            ref int pcServices,
            byte[] pGuidServices
            );

        /// <Summary>
        /// Closes an enumeration handle that is associated with a device query.
        /// </Summary>
        //BOOL BluetoothFindDeviceClose(
        //    HBLUETOOTH_DEVICE_FIND hFind
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindDeviceClose(
            IntPtr hFind
            );

        /// <Summary>
        /// Begins the enumeration of local Bluetooth devices.
        /// </Summary>
        //HBLUETOOTH_DEVICE_FIND BluetoothFindFirstDevice(
        //    BLUETOOTH_DEVICE_SEARCH_PARAMS* pbtsp,
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern IntPtr BluetoothFindFirstDevice(
            ref BluetoothDeviceSearchParams pbtsp,
            ref BluetoothDeviceInfo pbtdi
            );

        /// <Summary>
        /// Begins the enumeration of local Bluetooth radios.
        /// </Summary>
        //HBLUETOOTH_RADIO_FIND BluetoothFindFirstRadio(
        //    BLUETOOTH_FIND_RADIO_PARAMS* pbtfrp,
        //    HANDLE* phRadio
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern IntPtr BluetoothFindFirstRadio(
            ref BluetoothFindRadioParams pbtfrp,
            ref IntPtr phRadio
            );

        /// <Summary>
        /// Finds the next local Bluetooth device.
        /// </Summary>
        //BOOL BluetoothFindNextDevice(
        //    HBLUETOOTH_DEVICE_FIND hFind,
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindNextDevice(
            IntPtr hFind,
            ref BluetoothDeviceInfo pbtdi
            );

        /// <Summary>
        /// Finds the next Bluetooth radio.
        /// </Summary>
        //BOOL BluetoothFindNextRadio(
        //    HBLUETOOTH_RADIO_FIND hFind,
        //    HANDLE* phRadio
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindNextRadio(
            IntPtr hFind,
            ref IntPtr phRadio
            );

        /// <Summary>
        /// Closes the enumeration handle that is associated with finding Bluetooth radios.
        /// </Summary>
        //BOOL BluetoothFindRadioClose(
        //    HBLUETOOTH_RADIO_FIND hFind
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothFindRadioClose(
            IntPtr hFind
            );

        /// <Summary>
        /// Retrieves information about a remote Bluetooth device. The Bluetooth device must have been previously identified through a successful device inquiry function call.
        /// </Summary>
        //DWORD BluetoothGetDeviceInfo(
        //    HANDLE hRadio,
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetDeviceInfo(
            IntPtr hRadio,
            ref BluetoothDeviceInfo pbtdi
            );

        /// <Summary>
        /// Obtains information about a Bluetooth radio.
        /// </Summary>
        //DWORD BluetoothGetRadioInfo(
        //    HANDLE hRadio,
        //    PBLUETOOTH_RADIO_INFO pRadioInfo
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothGetRadioInfo(
            IntPtr hRadio,
            ref BluetoothRadioInfo pRadioInfo
            );

        /// <Summary>
        /// Determines whether a Bluetooth radio or radios is connectable.
        /// </Summary>
        //BOOL BluetoothIsConnectable(
        //    HANDLE hRadio
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsConnectable(
            IntPtr hRadio
            );

        /// <Summary>
        /// Determines whether a Bluetooth radio or radios is discoverable.
        /// </Summary>
        //BOOL BluetoothIsDiscoverable(
        //    HANDLE hRadio
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BluetoothIsDiscoverable(
            IntPtr hRadio
            );

        /// <Summary>
        /// Registers a callback function that is called when a particular Bluetooth device requests authentication.
        /// </Summary>
        //DWORD BluetoothRegisterForAuthentication(
        //    BLUETOOTH_DEVICE_INFO* pbtdi,
        //    HBLUETOOTH_AUTHENTICATION_REGISTRATION* phRegHandle,
        //    PFN_AUTHENTICATION_CALLBACK pfnCallback,
        //    PVOID pvParam
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothRegisterForAuthentication();

        /// <Summary>
        /// Removes authentication between a Bluetooth device and the computer, purging any cached information about the device.
        /// </Summary>
        //DWORD BluetoothRemoveDevice(
        //    BLUETOOTH_ADDRESS* pAddress
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothRemoveDevice(byte[] pAddress);

        /// <Summary>
        /// Enumerates through the SDP record stream and calls the callback function for each attribute in the record.
        /// </Summary>
        //BOOL BluetoothSdpEnumAttributes(
        //    LPBYTE pSDPStream,
        //    ULONG cbStreamSize,
        //    PFN_BLUETOOTH_ENUM_ATTRIBUTES_CALLBACK pfnCallback,
        //    LPVOID pvParam
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSdpEnumAttributes();

        /// <Summary>
        /// Retrieves the attribute value for an attribute identifier.
        /// </Summary>
        //DWORD BluetoothSdpGetAttributeValue(
        //  __in   LPBYTE pRecordStream,
        //  __in   ULONG cbRecordLength,
        //  __in   USHORT usAttributeId,
        //  __out  PSDP_ELEMENT_DATA pAttributeData
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSdpGetAttributeValue();

        /// <Summary>
        /// Iterates over a container stream and returns each element that is contained within the container element.
        /// </Summary>
        //DWORD BluetoothSdpGetContainerElementData(
        //  __in     LPBYTE pContainerStream,
        //  __in     ULONG cbContainerLength,
        //  __inout  HBLUETOOTH_CONTAINER_ELEMENT* pElement,
        //  __out    PSDP_ELEMENT_DATA pData
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSdpGetContainerElementData();

        /// <Summary>
        /// Retrieves and parses a single element from an SDP stream.
        /// </Summary>
        //DWORD BluetoothSdpGetElementData(
        //  __in   LPBYTE pSdpStream,
        //  __in   ULONG cbSpdStreamLength,
        //  __out  PSDP_ELEMENT_DATA pData
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSdpGetElementData();

        /// <Summary>
        /// Converts a raw string that is embedded in the SDP record into a Unicode string.
        /// </Summary>
        //DWORD BluetoothSdpGetString(
        //  __in     LPBYTE pRecordStream,
        //  __in     ULONG cbRecordLength,
        //  __in     PSDP_STRING_DATA_TYPE pStringData,
        //  __in     USHORT usStringOffset,
        //  __out    PWCHAR pszString,
        //  __inout  PULONG pcchStringLength
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSdpGetString();

        /// <Summary>
        /// Enables Bluetooth device selection.
        /// </Summary>
        //BOOL BluetoothSelectDevices(
        //    BLUETOOTH_SELECT_DEVICE_PARAMS* pbtsdp
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSelectDevices();

        /// <Summary>
        /// Frees resources associated with a previous call to the BluetoothSelectDevices function.
        /// </Summary>
        //BOOL BluetoothSelectDevicesFree(
        //    BLUETOOTH_SELECT_DEVICE_PARAMS* pbtsdp
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSelectDevicesFree();

        /// <Summary>
        /// Called when an authentication request to send the passkey response is received.
        /// </Summary>
        //DWORD BluetoothSendAuthenticationResponse(
        //    HANDLE hRadio,
        //    BLUETOOTH_DEVICE_INFO* pbtdi,
        //    LPWSTR pszPasskey
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothSendAuthenticationResponse(
            IntPtr hRadio,
            ref BluetoothDeviceInfo pbtdi,
            string pszPasskey
            );

        /// <Summary>
        /// Sets local service information for a specific Bluetooth radio.
        /// </Summary>
        //DWORD WINAPI BluetoothSetLocalServiceInfo(
        //  __in_opt  HANDLE hRadioIn,
        //  __in      const GUID* pClassGuid,
        //            ULONG ulInstance,
        //            const BLUETOOTH_LOCAL_SERVICE_INFO* pServiceInfoIn
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothSetLocalServiceInfo();

        /// <Summary>
        /// Enables or disables services for a Bluetooth device.
        /// </Summary>
        //DWORD BluetoothSetServiceState(
        //    HANDLE hRadio,
        //    BLUETOOTH_DEVICE_INFO* pbtdi,
        //    GUID* pGuidService,
        //    DWORD dwServiceFlags
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothSetServiceState(
            IntPtr hRadio,
            ref BluetoothDeviceInfo pbtdi,
            ref Guid pGuidService,
            Int32 dwServiceFlags
            );

        /// <Summary>
        /// Removes registration for a callback routine that was previously registered with a call to the BluetoothRegisterForAuthentication function.
        /// </Summary>
        //BOOL BluetoothUnregisterAuthentication(
        //    HBLUETOOTH_AUTHENTICATION_REGISTRATION hRegHandle
        //);
        // [DllImport("Bthprops.cpl", SetLastError = true)]
        // public static extern void BluetoothUnregisterAuthentication();

        /// <Summary>
        /// Updates the local computer cache about a Bluetooth device.
        /// </Summary>
        //DWORD BluetoothUpdateDeviceRecord(
        //    BLUETOOTH_DEVICE_INFO* pbtdi
        //);
        [DllImport("Bthprops.cpl", SetLastError = true)]
        public static extern int BluetoothUpdateDeviceRecord(
            ref BluetoothDeviceInfo pbtdi
            );

        public static void HandleResult(int result)
        {
            if (!Enum.IsDefined(typeof(BluetoothError), result))
            {
                throw new Win32Exception(result);
            }
            else
            {
                BluetoothError error = (BluetoothError)result;
                if (error != BluetoothError.BTH_ERROR_SUCCESS)
                    throw new Win32Exception(result, error.ToString());
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct BluetoothDeviceSearchParams
        {
            public UInt32 size;         // 32 (pack = 4!)
            public bool returnAuthenticated;
            public bool returnRemembered;
            public bool returnUnknown;
            public bool returnConnected;
            public bool issueInquiry;
            public Byte timeoutMultiplier;
            public IntPtr hRadio;
        }



        //typedef struct _BLUETOOTH_DEVICE_INFO {
        //  DWORD dwSize;
        //  BLUETOOTH_ADDRESS Address;
        //  ULONG ulClassofDevice;
        //  BOOL fConnected;
        //  BOOL fRemembered;
        //  BOOL fAuthenticated;
        //  SYSTEMTIME stLastSeen;
        //  SYSTEMTIME stLastUsed;
        //  WCHAR szName[BLUETOOTH_MAX_NAME_SIZE];
        //} BLUETOOTH_DEVICE_INFO;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        internal struct BluetoothDeviceInfo
        {
            public UInt32 size;        // 560
            public UInt32 dummya;
            public BluetoothAddress address;
            public UInt16 dummyb;
            public UInt32 classofDevice;
            public bool connected;
            public bool remembered;
            public bool authenticated;
            public SystemTime lastSeen;
            public SystemTime lastUsed;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            public String name;

            // Obsolete.
            //public void Initialize()
            //{
            //    name = new String('*', BLUETOOTH_MAX_NAME_SIZE);
            //    lastSeen = new SystemTime();
            //    lastUsed = new SystemTime();
            //    size = (UInt32)Marshal.SizeOf(this);
            //}
        }


        public struct BluetoothFindRadioParams
        {
            public UInt32 size;

            public void Initialize()
            {
                size = (UInt32)Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct BluetoothRadioInfo
        {
            public UInt32 size;
            public BluetoothAddress address;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            public String name;
            public UInt32 classofDevice;
            public UInt16 subversion;
            public UInt16 manufacturer;

            public void Initialize()
            {
                this.name = new String('*', BLUETOOTH_MAX_NAME_SIZE);
                this.address = new BluetoothAddress();
                this.size = (UInt32)Marshal.SizeOf(this);
            }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SystemTime
        {
            public UInt16 year;
            public UInt16 month;
            public UInt16 dayOfWeek;
            public UInt16 day;
            public UInt16 hour;
            public UInt16 minute;
            public UInt16 second;
            public UInt16 milliseconds;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BluetoothAddress
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] address;

            public override string ToString()
            {
                return String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                    address[5].ToString("x2"),
                    address[4].ToString("x2"),
                    address[3].ToString("x2"),
                    address[2].ToString("x2"),
                    address[1].ToString("x2"),
                    address[0].ToString("x2")
                    );
            }

            public override int GetHashCode()
            {
                int result = 0;
                for (int i = 0; i < address.Length; i++)
                    result = (result << 2) ^ address[i];
                return result;
            }

            public override bool Equals(object obj)
            {
                if (obj is BluetoothAddress)
                {
                    BluetoothAddress other = (BluetoothAddress)obj;
                    if (this.address.Length != other.address.Length)
                        return false;
                    for (int i = 0; i < address.Length; i++)
                    {
                        if (this.address[i] != other.address[i])
                            return false;
                    }
                    return true;
                }
                return false;
            }

            public static bool operator ==(BluetoothAddress bluetoothAddressA, BluetoothAddress bluetoothAddressB)
            {
                return bluetoothAddressA.Equals(bluetoothAddressB);
            }

            public static bool operator !=(BluetoothAddress bluetoothAddressA, BluetoothAddress bluetoothAddressB)
            {
                return !bluetoothAddressA.Equals(bluetoothAddressB);
            }
        }
    }
}
