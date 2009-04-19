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
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace WiiDeviceLibrary
{
    public class WiimoteSpeakerStream: Stream
    {
        IWiimote wiimote;

        float SampleRate = 1600;

        int sendBufferOffset = 0;
        byte[] sendBuffer = new byte[20];
        DateTime lastSendTime = DateTime.MinValue;
        public WiimoteSpeakerStream(IWiimote wiimote)
        {
            this.wiimote = wiimote;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (sendBufferOffset > 0)
            {
                int available = sendBuffer.Length - sendBufferOffset;
                int copyCount = count < available ? available : count;
                Array.Copy(buffer, offset, sendBuffer, sendBufferOffset, copyCount);
                offset += copyCount;
                count -= copyCount;
                if (sendBufferOffset == sendBuffer.Length)
                {
                    SendTimedSpeakerData(sendBuffer, 0, sendBuffer.Length);
                    sendBufferOffset = 0;
                }
            }
            if (count == 0)
                return;
            while (count >= sendBuffer.Length)
            {
                SendTimedSpeakerData(buffer, offset, sendBuffer.Length);
                offset += sendBuffer.Length;
                count -= sendBuffer.Length;
            }
            Array.Copy(buffer, offset, sendBuffer, sendBufferOffset, count);
            sendBufferOffset += count;
        }

        private void SendTimedSpeakerData(byte[] buffer, int offset, int count)
        {
            TimeSpan difference = DateTime.Now - lastSendTime;
            float sendRate = (float)SampleRate / 20f; // 20 samples per send.
            TimeSpan waitTime = TimeSpan.FromSeconds(1f / sendRate);
            if (difference < waitTime)
                Thread.Sleep(waitTime);
            lastSendTime = DateTime.Now;

            wiimote.SendSpeakerData(buffer, offset, (byte)count);
        }

        public override bool CanRead
        {
            get { return false; }
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
    }
}
