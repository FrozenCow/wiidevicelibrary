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
using WiiDeviceLibrary.Extensions;

namespace LinuxGUITest
{
	public partial class GuitarInformation : Gtk.Bin, IExtensionInformation
	{
		private GuitarExtension _Extension = null;
		
		public GuitarInformation(GuitarExtension extension)
		{
			_Extension = extension;
			this.Build();
		}
		
		public void Update()
		{
			// buttons
			entry1.Text = _Extension.Buttons.ToString();
			
			// whammy bar
			entry2.Text = _Extension.WhammyBar.ToString();
			
			// analog stick
			entry3.Text = _Extension.Stick.X.ToString();
			entry4.Text = _Extension.Stick.Y.ToString();
		}
		
		public Gtk.Widget Widget
		{
			get { return this; }
		}
	}
}
