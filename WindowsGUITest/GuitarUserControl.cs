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
    public partial class GuitarUserControl : UserControl, IExtensionControl
    {
        private GuitarExtension _Guitar;
        public GuitarExtension Guitar
        {
            get { return _Guitar; }
            set { _Guitar = value; }
        }

        public GuitarUserControl()
        {
            InitializeComponent();
        }

        public void UpdateUI()
        {
            if (Guitar != null)
            {
                buttonsBox.Text = Guitar.Buttons.ToString();
                whammyBox.Text = Guitar.WhammyBar.ToString();
                stickXBox.Text = Guitar.Stick.X.ToString();
                stickYBox.Text = Guitar.Stick.Y.ToString();
            }
        }
    }
}
