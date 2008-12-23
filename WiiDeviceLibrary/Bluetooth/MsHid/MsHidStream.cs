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
using Microsoft.Win32.SafeHandles;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public class MsHidStream: Stream
    {
        private SafeFileHandle _Handle;
        public SafeFileHandle Handle
        {
            get { return _Handle; }
        }

        private FileStream _BaseStream;
        public FileStream BaseStream
        {
            get { return _BaseStream; }
        }

        private byte[] writeBuffer = new byte[22];

        public MsHidStream(string devicePath)
            : this(CreateFileHandle(devicePath))
        {
        }

        public MsHidStream(SafeFileHandle fileHandle)
            : this(CreateFileStream(fileHandle))
        {
        }

        public MsHidStream(FileStream fileStream)
        {
            _Handle = fileStream.SafeFileHandle;
            _BaseStream = fileStream;
        }

        private static SafeFileHandle CreateFileHandle(string devicePath)
        {
            return NativeMethods.CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, NativeMethods.EFileAttributes.Overlapped, IntPtr.Zero);
        }

        private static FileStream CreateFileStream(SafeFileHandle fileHandle)
        {
            return new FileStream(fileHandle, FileAccess.ReadWrite, 22, true);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                return _BaseStream.Read(buffer, offset, count);
            }
            catch (IOException)
            {
                return 0;
            }
            catch (ObjectDisposedException)
            {
                return 0;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Array.Copy(buffer, offset, writeBuffer, 0, count);
            WriteHidReport();
        }

        private void WriteHidReport()
        {
            WriteHidReport(writeBuffer, 0, writeBuffer.Length);
        }

        protected virtual void WriteHidReport(byte[] writeBuffer, int offset, int count)
        {
            _BaseStream.Write(writeBuffer, 0, 22);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                BaseStream.Close();
            base.Dispose(disposing);
        }

        #region Not supported methods
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

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
