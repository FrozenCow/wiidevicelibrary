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

// Credits of the C code of this file goes to the contributers of Wiili.
// http://www.wiili.org/index.php/Talk:Wiimote#ADPCM_4-bit_Format_Discussion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WiiDeviceLibrary
{
    public class Pcm8ToAdpcm4Stream : Stream
    {
        private Stream pcm8Stream;
        //private static int[] index_table = new int[16] { -1, -1, -1, -1, 2, 4, 6, 8, -1, -1, -1, -1, 2, 4, 6, 8 };
        private static int[] diff_table = new int[16] { 1, 3, 5, 7, 9, 11, 13, 15, -1, -3, -5, -7, -9, -11, -13, 15 };
        private static int[] step_scale = new int[16] { 230, 230, 230, 230, 307, 409, 512, 614, 230, 230, 230, 230, 307, 409, 512, 614 };

        int adpcm_prev_value = 0;
        int adpcm_step = 127;
        byte[] pcm_buffer;

        #region Properties
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
            get { return false; }
        }
        #endregion

        public Pcm8ToAdpcm4Stream(Stream pcm8Stream)
        {
            this.pcm8Stream = pcm8Stream;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if ((pcm_buffer == null) || (pcm_buffer.Length != count * 2))
                pcm_buffer = new byte[count * 2];
            int readBytes = pcm8Stream.Read(pcm_buffer, 0, count * 2);

            for (int i = 0; i < readBytes; i++)
            {
                int value = (int)unchecked((short)(pcm_buffer[i] << 8)); // pcm_data is 8-bit signed char waveform
                int diff = value - adpcm_prev_value;
                byte encoded_val = 0;
                if (diff < 0)
                {
                    encoded_val |= 8;
                    diff = -diff;
                }
                diff = (diff << 2) / adpcm_step;
                if (diff > 7)
                    diff = 7;
                encoded_val |= (byte)diff;
                adpcm_prev_value += ((adpcm_step * diff_table[encoded_val]) / 8);
                if (adpcm_prev_value > 0x7fff)
                    adpcm_prev_value = 0x7fff;
                if (adpcm_prev_value < -0x8000)
                    adpcm_prev_value = -0x8000;
                adpcm_step = (adpcm_step * step_scale[encoded_val]) >> 8;
                if (adpcm_step < 127)
                    adpcm_step = 127;
                if (adpcm_step > 24567)
                    adpcm_step = 24567;

                if (i % 2 == 1)
                    buffer[offset + (i >> 1)] |= (byte)(    encoded_val         & 0x0f);
                else
                    buffer[offset + (i >> 1)] = (byte)((    encoded_val << 4)   & 0xf0);
            }

            return readBytes / 2 + (readBytes % 2);
        }

        public override void Close()
        {
            base.Close();
            pcm8Stream.Close();
        }

        #region Not Supported Methods
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

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
