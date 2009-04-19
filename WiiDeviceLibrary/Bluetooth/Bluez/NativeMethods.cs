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
using System.Text;
using System.Runtime.InteropServices;

namespace WiiDeviceLibrary.Bluetooth.Bluez
{	
	internal static class NativeMethods
	{		
		public const int AF_BLUETOOTH = 31;
		public const int SOCK_SEQPACKET = 5;
		public const int BTPROTO_L2CAP = 0;
		public const int OGF_LINK_CTL = 1;
		public const int OCF_INQUIRY = 1;
		public const int OCF_INQUIRY_CANCEL = 2;
		public const int OCF_REMOTE_NAME_REQ = 0x19;
		public const int SOL_HCI = 0;
		public const int HCI_FILTER = 2;
		public const int HCI_EVENT_PKT = 4;
		
		public const int MSG_DONTWAIT = 0x40;

        internal enum HciEvent
        {
            EVT_INQUIRY_COMPLETE = 0x01,
            EVT_INQUIRY_RESULT = 0x02,
            EVT_CONN_COMPLETE = 0x03,
            EVT_CONN_REQUEST = 0x04,
            EVT_DISCONN_COMPLETE = 0x05,
            EVT_AUTH_COMPLETE = 0x06,
            EVT_REMOTE_NAME_REQ_COMPLETE = 0x07,
            EVT_ENCRYPT_CHANGE = 0x08,
            EVT_CHANGE_CONN_LINK_KEY_COMPLETE = 0x09,
            EVT_MASTER_LINK_KEY_COMPLETE = 0x0A,
            EVT_READ_REMOTE_FEATURES_COMPLETE = 0x0B,
            EVT_READ_REMOTE_VERSION_COMPLETE = 0x0C,
            EVT_QOS_SETUP_COMPLETE = 0x0D,
            EVT_CMD_COMPLETE = 0x0E,
            EVT_CMD_STATUS = 0x0F,
            EVT_HARDWARE_ERROR = 0x10,
            EVT_FLUSH_OCCURRED = 0x11,
            EVT_ROLE_CHANGE = 0x12,
            EVT_NUM_COMP_PKTS = 0x13,
            EVT_MODE_CHANGE = 0x14,
            EVT_RETURN_LINK_KEYS = 0x15,
            EVT_PIN_CODE_REQ = 0x16,
            EVT_LINK_KEY_REQ = 0x17,
            EVT_LINK_KEY_NOTIFY = 0x18,
            EVT_LOOPBACK_COMMAND = 0x19,
            EVT_DATA_BUFFER_OVERFLOW = 0x1A,
            EVT_MAX_SLOTS_CHANGE = 0x1B,
            EVT_READ_CLOCK_OFFSET_COMPLETE = 0x1C,
            EVT_CONN_PTYPE_CHANGED = 0x1D,
            EVT_QOS_VIOLATION = 0x1E,
            EVT_PSCAN_REP_MODE_CHANGE = 0x20,
            EVT_FLOW_SPEC_COMPLETE = 0x21,
            EVT_INQUIRY_RESULT_WITH_RSSI = 0x22,
            EVT_READ_REMOTE_EXT_FEATURES_COMPLETE = 0x23,
            EVT_SYNC_CONN_COMPLETE = 0x2C,
            EVT_SYNC_CONN_CHANGED = 0x2D,
            EVT_SNIFF_SUBRATING = 0x2E,
            EVT_EXTENDED_INQUIRY_RESULT = 0x2F,
            EVT_ENCRYPTION_KEY_REFRESH_COMPLETE = 0x30,
            EVT_IO_CAPABILITY_REQUEST = 0x31,
            EVT_IO_CAPABILITY_RESPONSE = 0x32,
            EVT_USER_CONFIRM_REQUEST = 0x33,
            EVT_USER_PASSKEY_REQUEST = 0x34,
            EVT_REMOTE_OOB_DATA_REQUEST = 0x35,
            EVT_SIMPLE_PAIRING_COMPLETE = 0x36,
            EVT_LINK_SUPERVISION_TIMEOUT_CHANGED = 0x38,
            EVT_ENHANCED_FLUSH_COMPLETE = 0x39,
            EVT_USER_PASSKEY_NOTIFY = 0x3B,
            EVT_KEYPRESS_NOTIFY = 0x3C,
            EVT_REMOTE_HOST_FEATURES_NOTIFY = 0x3D,
            EVT_TESTING = 0xFE,
            EVT_VENDOR = 0xFF
        }

		[StructLayout(LayoutKind.Sequential)]
		internal struct hci_filter
		{
			public uint type_mask;
			public uint event_mask_a;
			public uint event_mask_b;
			public ushort opcode;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		internal struct remote_name_req_cp
		{
			public bdaddr_t bdaddr;
			public byte pscan_rep_mode;
			public byte pscan_mode;
			public ushort clock_offset;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		internal struct inquiry_cp
		{
			public byte lap_a;
			public byte lap_b;
			public byte lap_c;
			public byte length;
			public byte num_rsp;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		internal struct bdaddr_t
		{
			public byte a;
			public byte b;
			public byte c;
			public byte d;
			public byte e;
			public byte f;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		internal struct sockaddr_l2
		{
			public ushort l2_family;
			public ushort l2_psm;
			public bdaddr_t bdaddr;
		}
	
		[DllImport("libc")]
		public static extern int setsockopt(int socket, int level, int option_name, ref hci_filter option_value, uint option_len);

		[DllImport("libbluetooth.so.3")]
		public static extern int hci_send_cmd(int sock, ushort ogf, ushort ocf, byte plen, IntPtr param);
	
		[DllImport("libbluetooth.so.3")]
		public static extern int hci_get_route(IntPtr bdaddr);
		
		[DllImport("libbluetooth.so.3")]
		public static extern int hci_devba(int dev_id, out bdaddr_t bdaddr);
		
		[DllImport("libbluetooth.so.3")]
		public static extern int hci_open_dev(int dev_id);
		
		[DllImport("libbluetooth.so.3")]
		public static extern int hci_close_dev(int dev_id);
		
		[DllImport("libbluetooth.so.3")]
		public static extern int str2ba(string str, out bdaddr_t ba);

        [DllImport("libc", SetLastError = true)]
		public static extern int socket(int socket_family, int socket_type, int protocol);
		
		[DllImport("libc", SetLastError = true)]
		public static extern int close(int socket);
		
		[DllImport("libc", SetLastError = true)]
		public static extern int bind(int socket, ref sockaddr_l2 addr, uint size);
		
		[DllImport("libc", SetLastError = true)]
		public static extern int connect(int socket, ref sockaddr_l2 addr, uint size);
		
		[DllImport("libc", SetLastError = true)]
		public static extern int recv(int socket, [MarshalAs(UnmanagedType.LPArray)]byte[] buffer, int length, int flags);
		
		[DllImport("libc", SetLastError = true)]
		public static extern int send(int socket, [MarshalAs(UnmanagedType.LPArray)]byte[] buffer, int length, int flags);			
	}
}
