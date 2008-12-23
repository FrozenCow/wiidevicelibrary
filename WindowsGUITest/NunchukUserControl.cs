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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WiiDeviceLibrary.Extensions;

namespace WindowsGUITest
{
    public partial class NunchukUserControl : UserControl
    {
        private NunchukExtension _Nunchuk;
        public NunchukExtension Nunchuk
        {
            get { return _Nunchuk; }
            set { _Nunchuk = value; }
        }
	
        public NunchukUserControl()
        {
            InitializeComponent();

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (Nunchuk != null)
            {
                buttonsBox.Text = Nunchuk.Buttons.ToString();
                accelerometerXBox.Text = Nunchuk.Accelerometer.Calibrated.X.ToString();
                accelerometerYBox.Text = Nunchuk.Accelerometer.Calibrated.Y.ToString();
                accelerometerZBox.Text = Nunchuk.Accelerometer.Calibrated.Z.ToString();
                analogstickXBox.Text = Nunchuk.Stick.Calibrated.X.ToString();
                analogstickYBox.Text = Nunchuk.Stick.Calibrated.Y.ToString();
            }
        }
    }
}
