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

namespace WiiDeviceLibrary
{
	public class AnalogStickCalibration
	{
        #region Fields
		private byte _XMin;
		private byte _XMid;
		private byte _XMax;
		private byte _YMin;
		private byte _YMid;
		private byte _YMax;
        #endregion
		
		public AnalogStickCalibration(byte xMin, byte xMid, byte xMax, byte yMin, byte yMid, byte yMax)
		{
			_XMin = xMin;
			_XMid = xMid;
			_XMax = xMax;
			_YMin = yMin;
			_YMid = yMid;
			_YMax = yMax;
		}
		
        #region Properties
		public byte XMin
		{
			get { return _XMin; }
		}
		public byte XMid
		{
			get { return _XMid; }
		}
		public byte XMax
		{
			get { return _XMax; }
		}
		public byte YMin
		{
			get { return _YMin; }
		}
		public byte YMid
		{
			get { return _YMid; }
		}
		public byte YMax
		{
			get { return _YMax; }
		}
        #endregion
		
        public void Calibrate(AnalogStickAxes<byte> raw, AnalogStickAxes<float> calibrating)
        {
            calibrating.X = CalibrateValue(raw.X, XMin, XMid, XMax);
            calibrating.Y = CalibrateValue(raw.Y, YMin, YMid, YMax);
        }

        private static float CalibrateValue(byte rawValue, byte minValue, byte midValue, byte maxValue)
        {
			if(rawValue < midValue)
			{
				return (float)(rawValue - minValue) / (float)(midValue - minValue) - 1f;
			}
			else
			{
				return (float)(rawValue - midValue) / (float)(maxValue - midValue);
			}
        }		
	}
}
