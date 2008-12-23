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
    public class PressureCalibration
    {
        ushort[] calibrationData;
        float[] newtonFormCoefficients;

        public PressureCalibration(ushort[] calibrationData)
        {
            if (calibrationData.Length != 3)
                throw new ArgumentException("The length of calibrationData must be equal to 3.", "calibrationData");
            this.calibrationData = calibrationData;

            // Calculating Newton form coefficients.
            newtonFormCoefficients = new float[] {
                0,
                17,
                34
            };
            for (int i = 1; i < 3; i++)
            {
                for (int j = 0; j < 3 - i; j++)
                {
                    newtonFormCoefficients[j] = (newtonFormCoefficients[j + 1] - newtonFormCoefficients[j]) / ((float)calibrationData[j + i] - (float)calibrationData[j]);
                }
            }
        }

        public float Calibrate(ushort rawValue)
        {
            return (newtonFormCoefficients[0]
                * ((float)rawValue - (float)calibrationData[1]) + newtonFormCoefficients[1])
                * ((float)rawValue - (float)calibrationData[2]) + newtonFormCoefficients[2];
        }
    }
}
