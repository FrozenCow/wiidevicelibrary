using System;
using System.Collections.Generic;
using System.Text;

namespace WiiDeviceLibrary
{
    public class IRSensitivity
    {
        public static IRSensitivity Default { get; set; }
        public static IRSensitivity Level1 { get; private set; }
        public static IRSensitivity Level2 { get; private set; }
        public static IRSensitivity Level3 { get; private set; }
        public static IRSensitivity Level4 { get; private set; }
        public static IRSensitivity Level5 { get; private set; }

        static IRSensitivity()
        {
            Level1 = new IRSensitivity(new byte[] { 0x02, 0x00, 0x00, 0x71, 0x01, 0x00, 0x64, 0x00, 0xfe }, new byte[] { 0xfd, 0x05 });
            Level2 = new IRSensitivity(new byte[] { 0x02, 0x00, 0x00, 0x71, 0x01, 0x00, 0x96, 0x00, 0xb4 }, new byte[] { 0xb3, 0x04 });
            Level3 = new IRSensitivity(new byte[] { 0x02, 0x00, 0x00, 0x71, 0x01, 0x00, 0xaa, 0x00, 0x64 }, new byte[] { 0x63, 0x03 });
            Level4 = new IRSensitivity(new byte[] { 0x02, 0x00, 0x00, 0x71, 0x01, 0x00, 0xc8, 0x00, 0x36 }, new byte[] { 0x35, 0x03 });
            Level5 = new IRSensitivity(new byte[] { 0x07, 0x00, 0x00, 0x71, 0x01, 0x00, 0x72, 0x00, 0x20 }, new byte[] { 0x1f, 0x03 });
            Default = Level3;
        }

        public byte[] ConfigurationBlock1 { get; private set; }
        public byte[] ConfigurationBlock2 { get; private set; }

        public IRSensitivity(byte[] configurationBlock1, byte[] configurationBlock2)
        {
            if (configurationBlock1 == null)
                throw new ArgumentNullException("configurationBlock1");
            if (configurationBlock2 == null)
                throw new ArgumentNullException("configurationBlock2");
            if (configurationBlock1.Length != 9)
                throw new ArgumentException("ConfigurationBlock1 should have a length of 9 bytes.", "configurationBlock1");
            if (configurationBlock2.Length != 2)
                throw new ArgumentException("ConfigurationBlock2 should have a length of 2 bytes.", "configurationBlock2");
            ConfigurationBlock1 = configurationBlock1;
            ConfigurationBlock2 = configurationBlock2;
        }
    }
}
