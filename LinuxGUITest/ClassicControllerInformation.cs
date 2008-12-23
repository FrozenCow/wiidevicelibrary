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

using WiiDeviceLibrary.Extensions;

namespace LinuxGUITest
{
	public partial class ClassicControllerInformation : Gtk.Bin, IExtensionInformation
	{
		private ClassicControllerExtension _Extension = null;
		
		public ClassicControllerInformation(ClassicControllerExtension extension)
		{
			_Extension = extension;
			this.Build();
		}
		
		public Gtk.Widget Widget
		{
			get { return this; }
		}
		
		public void Update()
		{
			// buttons
			entry5.Text = _Extension.Buttons.ToString();
			
			// triggers
			entry6.Text = _Extension.LeftTrigger.ToString();
			entry7.Text = _Extension.RightTrigger.ToString();
			
			// sticks
			entry8.Text = _Extension.LeftStick.Calibrated.X.ToString();
			entry9.Text = _Extension.LeftStick.Calibrated.Y.ToString();
			
			entry10.Text = _Extension.RightStick.Calibrated.X.ToString();
			entry11.Text = _Extension.RightStick.Calibrated.Y.ToString();
			
		}
	}
}
