using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace WiiDeviceLibrary.Bluetooth.MsHid
{
    public class UnsureMsHidStream : MsHidStream
    {
        bool useStreamForWriting = true;
        public UnsureMsHidStream(string devicePath)
            : base(devicePath)
        {
        }

        public UnsureMsHidStream(SafeFileHandle fileHandle)
            : base(fileHandle)
        {
        }

        public UnsureMsHidStream(FileStream fileStream)
            : base(fileStream)
        {
        }

        protected override void WriteHidReport(byte[] writeBuffer, int offset, int count)
        {
            if (useStreamForWriting)
            {
                try
                {
                    base.WriteHidReport(writeBuffer, offset, count);
                }
                catch (IOException)
                {
                    useStreamForWriting = false;
                }
            }
            if (!useStreamForWriting)
            {
                NativeMethods.HidD_SetOutputReport(Handle.DangerousGetHandle(), writeBuffer, 22);
            }
        }
    }
}
