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

namespace WiiDeviceLibrary
{
    /// <summary>
    /// Structure containing an X, Y and Z axis for a set of accelerometers.
    /// </summary>
    public class Accelerometer
    {
        #region Fields

        #endregion

        public Accelerometer(AccelerometerCalibration calibration)
        {
            _Calibration = calibration;
            _Raw = new AccelerometerAxes<ushort>();
            _Calibrated = new AccelerometerAxes<float>();
        }

        #region Properties
        private AccelerometerAxes<ushort> _Raw;
        public AccelerometerAxes<ushort> Raw
        {
            get { return _Raw; }
        }

        private AccelerometerAxes<float> _Calibrated;
        public AccelerometerAxes<float> Calibrated
        {
            get { return _Calibrated; }
        }

        private AccelerometerCalibration _Calibration;
        public AccelerometerCalibration Calibration
        {
            get { return _Calibration; }
        }
        #endregion
    }
}
