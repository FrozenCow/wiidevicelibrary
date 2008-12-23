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
    public class AnalogStick
    {
        #region Fields

        #endregion

        public AnalogStick(AnalogStickCalibration calibration)
        {
            _Calibration = calibration;
            _Raw = new AnalogStickAxes<byte>();
            _Calibrated = new AnalogStickAxes<float>();
        }

        #region Properties
        private AnalogStickAxes<byte> _Raw;
        public AnalogStickAxes<byte> Raw
        {
            get { return _Raw; }
        }

        private AnalogStickAxes<float> _Calibrated;
        public AnalogStickAxes<float> Calibrated
        {
            get { return _Calibrated; }
        }

        private AnalogStickCalibration _Calibration;
        public AnalogStickCalibration Calibration
        {
            get { return _Calibration; }
        }
        #endregion
    }
}
