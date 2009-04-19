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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WiiDeviceLibrary.Extensions;

namespace WindowsGUITest
{
    public partial class ClassicControllerUserControl : UserControl, IExtensionControl
    {
        private ClassicControllerExtension _ClassicController;
        public ClassicControllerExtension ClassicController
        {
            get { return _ClassicController; }
            set { _ClassicController = value; }
        }

        public ClassicControllerUserControl()
        {
            InitializeComponent();
        }

        public void UpdateUI()
        {
            if (ClassicController != null)
            {
                buttonsBox.Text = ClassicController.Buttons.ToString();
                leftTriggerBox.Text = ClassicController.LeftTrigger.ToString();
                rightTriggerBox.Text = ClassicController.RightTrigger.ToString();
                leftStickXBox.Text = ClassicController.LeftStick.Calibrated.X.ToString();
                leftStickYBox.Text = ClassicController.LeftStick.Calibrated.Y.ToString();
                rightStickXBox.Text = ClassicController.RightStick.Calibrated.X.ToString();
                rightStickYBox.Text = ClassicController.RightStick.Calibrated.Y.ToString();
            }
        }
    }
}
