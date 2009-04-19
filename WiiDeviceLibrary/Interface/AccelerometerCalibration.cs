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

namespace WiiDeviceLibrary
{
	public class AccelerometerCalibration
	{
        #region Fields
        private ushort[] _Zero;
        private ushort[] _One;
        #endregion
		
		public AccelerometerCalibration(ushort xZero, ushort xOne, ushort yZero, ushort yOne, ushort zZero, ushort zOne)
		{
			_Zero = new ushort[] { xZero, yZero, zZero };
			_One = new ushort[] { xOne, yOne, zOne };
		}
		
        #region Properties
		public ushort XZero
		{
			get { return _Zero[0]; }
		}
		public ushort XOne
		{
			get { return _One[0]; }
		}
		public ushort YZero
		{
			get { return _Zero[1]; }
		}
		public ushort YOne
		{
			get { return _One[1]; }
		}	
		public ushort ZZero
		{
			get { return _Zero[2]; }
		}
		public ushort ZOne
		{
			get { return _One[2]; }
		}		
        #endregion

        public void Calibrate(AccelerometerAxes<ushort> raw, AccelerometerAxes<float> calibrating)
        {
            calibrating.X = CalibrateValue(raw.X, XZero, XOne); // 131 158
            calibrating.Y = CalibrateValue(raw.Y, YZero, YOne); // 131 158
            calibrating.Z = CalibrateValue(raw.Z, ZZero, ZOne); // 129 155
        }

        private static float CalibrateValue(ushort rawValue, ushort zeroValue, ushort oneValue)
        {
            if (zeroValue == oneValue)
                return 0;
            return (float)(rawValue - zeroValue) / (float)(oneValue - zeroValue);
        }
    }
}
