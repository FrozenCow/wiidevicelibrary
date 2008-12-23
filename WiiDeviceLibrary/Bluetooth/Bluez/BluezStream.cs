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
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;

namespace WiiDeviceLibrary.Bluetooth.Bluez
{
    /// <summary>
    /// Exposes a System.IO.Stream around a bluetooth connection to a wiimote.
    /// </summary>
    public class BluezStream : Stream
    {
        #region Fields
        private int _ControlSocket = 0;
        private int _InterruptSocket = 0;
        private byte[] _ReceiveBuffer = new byte[23];
        private byte[] _SendBuffer = new byte[23];
		private bool _Connected = false;
        #endregion
        #region Capability properties
        public override bool CanRead
        {
            get { return _Connected; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _Connected; }
        }
        #endregion
        #region Constructors
        public BluezStream(string address)
        {
            NativeMethods.bdaddr_t bdaddress;
            NativeMethods.str2ba(address, out bdaddress);
            Connect(bdaddress);
        }

		public BluezStream(BluetoothAddress address) : this(address.ToString())
		{
		}
		
        //internal BluezStream(NativeMethods.bdaddr_t address)
        //{
        //    Connect(address);
        //}
        #endregion

        private void Connect(NativeMethods.bdaddr_t bdaddress)
        {
			// get bluetooth address
			NativeMethods.bdaddr_t dongleAddr;
			int deviceId = NativeMethods.hci_get_route(IntPtr.Zero);
			NativeMethods.hci_devba(deviceId, out dongleAddr);
			
			// create l2cap address
            NativeMethods.sockaddr_l2 address;
            address.l2_family = NativeMethods.AF_BLUETOOTH;
			
            // allocate sockets
			_ControlSocket = -1;
			while(_ControlSocket == -1)
			{
				_ControlSocket = NativeMethods.socket(NativeMethods.AF_BLUETOOTH, NativeMethods.SOCK_SEQPACKET, NativeMethods.BTPROTO_L2CAP);
				int error = Marshal.GetLastWin32Error();
				if(_ControlSocket == -1)
				{
					if(error == 4)
					{
						Console.WriteLine("Retrying control");
						continue;
					}
					throw new WiiDeviceLibrary.DeviceConnectException("Failed to allocate the control socket.");
				}
			}
                
			_InterruptSocket = -1;
			while(_InterruptSocket == -1)
			{
				_InterruptSocket = NativeMethods.socket(NativeMethods.AF_BLUETOOTH, NativeMethods.SOCK_SEQPACKET, NativeMethods.BTPROTO_L2CAP);
				int error = Marshal.GetLastWin32Error();
				if(_InterruptSocket == -1)
				{
					if(error == 4)
					{
						Console.WriteLine("Retrying interrupt");
						continue;
					}
					NativeMethods.close(_ControlSocket);
					throw new WiiDeviceLibrary.DeviceConnectException("Failed to allocate the interrupt socket.");
				}
			}

			// bind the bluetooth socket
			address.l2_psm = 0x0;
			address.bdaddr = dongleAddr;
			if(NativeMethods.bind(_ControlSocket, ref address, (uint)Marshal.SizeOf(address)) == -1)
			{
				NativeMethods.close(_ControlSocket);
				NativeMethods.close(_InterruptSocket);
				throw new WiiDeviceLibrary.DeviceConnectException("Failed to bind the control socket");
			}
			
            // connect the control socket
			address.bdaddr = bdaddress;
			address.l2_psm = 0x11;
            if (NativeMethods.connect(_ControlSocket, ref address, (uint)Marshal.SizeOf(address)) == -1)
            {
				int error = Marshal.GetLastWin32Error();
				if(error != 4)
				{
	                NativeMethods.close(_ControlSocket);
	                NativeMethods.close(_InterruptSocket);
	                throw new WiiDeviceLibrary.DeviceConnectException("Failed to connect the control socket: " + error);
				}
            }

            // connect the interrupt socket
			address.bdaddr = bdaddress;
			address.l2_psm = 0x13;		
            if (NativeMethods.connect(_InterruptSocket, ref address, (uint)Marshal.SizeOf(address)) == -1)
            {
                NativeMethods.close(_ControlSocket);
                NativeMethods.close(_InterruptSocket);
                throw new WiiDeviceLibrary.DeviceConnectException("Failed to connect the interrupt socket.");
            }
			_Connected = true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int receivedByteCount = NativeMethods.recv(_InterruptSocket, _ReceiveBuffer, _ReceiveBuffer.Length, 0);
            if (receivedByteCount > 0)
			{
				// with bluez you get a hid byte, this must not be copied into the buffer
				count = Math.Min(count, receivedByteCount - 1);
	            Array.Copy(_ReceiveBuffer, 1, buffer, offset, count);
	            return count;					
			}
			else if (receivedByteCount <= 0)
			{
				NativeMethods.close(_InterruptSocket);
				NativeMethods.close(_ControlSocket);
				_Connected = false;
				if(receivedByteCount < 0)
				{
					throw new IOException("Failed to read from the interrupt socket.");
				}
			}
			return 0;
        }

		delegate int ReadDelegate (byte[] buffer, int offset, int count);
		
		public override IAsyncResult BeginRead (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
	
			ReadDelegate r = new ReadDelegate(Read);
			return r.BeginInvoke(buffer, offset, count, callback, state);
		}
		
		public override int EndRead (IAsyncResult asyncResult)
		{
			AsyncResult result = asyncResult as AsyncResult;
			if(result == null)
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			
			ReadDelegate r = result.AsyncDelegate as ReadDelegate;
			if(r == null)
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			
			return r.EndInvoke(asyncResult);
		}

        public override void Write(byte[] buffer, int offset, int count)
        {
			if(!_Connected)
				throw new IOException("The control socket is not connected");
            _SendBuffer[0] = 0x52;
            Array.Copy(buffer, offset, _SendBuffer, 1, count);
            int returnValue = NativeMethods.send(_ControlSocket, _SendBuffer, count + 1, 0);
			if(returnValue == -1)
			{
				NativeMethods.close(_InterruptSocket);
				NativeMethods.close(_ControlSocket);		
				_Connected = false;				
				throw new IOException("Failed to write to the control socket.");
			}
        }

		protected override void Dispose(bool disposing)
		{
			if(_Connected)
			{
				_SendBuffer[0] = 0x15;
				_SendBuffer[1] = 0x1;
				NativeMethods.send(_ControlSocket, _SendBuffer, 2, 0);
				NativeMethods.close(_InterruptSocket);
				NativeMethods.close(_ControlSocket);
			}
			base.Dispose (disposing);
		}
		
        #region Not supported methods
        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
