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
using System.Threading;
using System.Collections.Generic;
using WiiDeviceLibrary;
using Gtk;

namespace LinuxGUITest
{
	public partial class DiscoverWindow : Gtk.Window
	{
        #region Fields
		private List<IDeviceInformation> _DeviceInformations = new List<IDeviceInformation>();
		private Dictionary<IDeviceInformation, Gtk.TreeIter> _DeviceNodes = new Dictionary<IDeviceInformation,TreeIter>();
		private HBox _HBox = null;
		private ListStore _ListStore = null;
		private Thread _UpdateThread = null;
		private IDeviceProvider _Provider;
        #endregion
		
		public DiscoverWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Title = "LinuxGUITest";
			this.DeleteEvent += new DeleteEventHandler(OnDeleteEvent);
			this.WindowPosition = Gtk.WindowPosition.Center;

            _Provider = DeviceProviderRegistry.CreateSupportedDeviceProvider();

			_Provider.DeviceFound += new EventHandler<DeviceInfoEventArgs>(DeviceFound);
			_Provider.DeviceLost += new EventHandler<DeviceInfoEventArgs>(DeviceLost);
			
			_UpdateThread = new Thread(new ThreadStart(UpdateFunction));
			_UpdateThread.Start();
			
			_HBox = new HBox(true, 0);
			_HBox.Homogeneous = false;
			this.Add(_HBox);
			_HBox.Show();

			Gtk.TreeView tree = new Gtk.TreeView();
			tree.WidthRequest = 250;
			tree.HeightRequest = 300;
			_HBox.PackStart(tree);

			// new column
			Gtk.TreeViewColumn devicetypeColumn = new Gtk.TreeViewColumn();
			devicetypeColumn.Title = "Type";
			Gtk.CellRendererText devicetypeCell = new CellRendererText();
			devicetypeColumn.PackStart(devicetypeCell, true);
			devicetypeColumn.AddAttribute(devicetypeCell, "text", 0);
			
			// new column
			Gtk.TreeViewColumn detailsColumn = new Gtk.TreeViewColumn();
			detailsColumn.Title = "Details";
			Gtk.CellRendererText detailseCell = new CellRendererText();
			detailsColumn.PackStart(detailseCell, true);
			detailsColumn.AddAttribute(detailseCell, "text", 1);			
			
			tree.AppendColumn(devicetypeColumn);
			tree.AppendColumn(detailsColumn);
			_ListStore = new Gtk.ListStore(typeof(string), typeof(string));
			
			tree.Model = _ListStore;
		
			_HBox.ShowAll();
			
			
			LogLine("Searching for wii devices...");
            _Provider.StartDiscovering();
		}		
		
        #region Event Handlers
		private void DeviceDisconnected(object sender, EventArgs args)
		{
			if(!(sender is IDevice))
			{
				return;
			}
			Gtk.Application.Invoke(delegate {
				foreach(IDeviceInformation deviceInformation in _DeviceInformations)
				{
					if(deviceInformation.Device == (IDevice)sender)
					{
						_HBox.Remove(deviceInformation.Separator);
						_HBox.Remove(deviceInformation.Widget);
						_DeviceInformations.Remove(deviceInformation);
						int height = 0;
						int width = 0;
						this.GetSize(out width, out height);
						this.Resize(10,height);
						Gtk.TreeIter iter = _DeviceNodes[deviceInformation];
						_ListStore.Remove(ref iter);
						break;								
					}
				}
			});
		}
		
		private void DeviceLost(object sender, DeviceInfoEventArgs args)
		{
			Gtk.Application.Invoke(delegate { LogLine("Lost a device"); });
		}
		
		private void DeviceFound(object sender, DeviceInfoEventArgs args)
		{
			Gtk.TreeIter iter = default(Gtk.TreeIter);
			Gtk.Application.Invoke(delegate {
				LogLine("Found a device, trying to connect...");
				string details = "unavailable";
				if(args.DeviceInfo is IBluetoothDeviceInfo)
					details = ((IBluetoothDeviceInfo)(args.DeviceInfo)).BluetoothAddress.ToString();
				iter = _ListStore.AppendValues("Device", details);
			} );
			try
			{
				IDevice device = _Provider.Connect(args.DeviceInfo);
				Gtk.Application.Invoke(delegate { LogLine("Successfully connected to the device!"); 
					AddDevice(device, iter); });
			}
			catch(DeviceConnectException e)
			{
				Gtk.Application.Invoke(delegate {
					LogLine(e.Message);
					_ListStore.Remove(ref iter);
				}); 				
			}
		}
		
		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			_Provider.StopDiscovering();
			_UpdateThread = null;

            foreach (IDevice connectedDevice in new List<IDevice>(_Provider.ConnectedDevices))
                connectedDevice.Disconnect();

			Environment.Exit(1);
			args.RetVal = true;
		}		
        #endregion
		
        #region Thread Methods
		private void UpdateFunction()
		{
			while(_UpdateThread != null)
			{
				Gtk.Application.Invoke(delegate {
				foreach(IDeviceInformation deviceInformation in _DeviceInformations)
				{
						deviceInformation.Update();					
				}
				});
				Thread.Sleep(20);
			}
		}
        #endregion
		
        #region Helpers
		private void LogLine(string text)
		{
			Console.WriteLine(text);
		}
		
		private void AddDevice(IDevice device, Gtk.TreeIter iter)
		{
			// choose the deviceInformation to create
			IDeviceInformation deviceInformation = null;
			if(device is IWiimote)
			{
				_ListStore.SetValue(iter, 0, "Wiimote");
				deviceInformation = new WiimoteInformation(this, (IWiimote)device);
			}
			else if(device is IBalanceBoard)
			{
				_ListStore.SetValue(iter, 0, "Balanceboard");
				deviceInformation = new BalanceBoardInformation((IBalanceBoard)device);
			}
			
			// add a seperator to the form to separate from other deviceInformations
			deviceInformation.Separator = new VSeparator();
			_HBox.PackStart(deviceInformation.Separator);
			deviceInformation.Separator.Show();

			// add the deviceInformation to the form
			_HBox.PackStart(deviceInformation.Widget, false, false, 0);
			deviceInformation.Widget.Show();
			_DeviceInformations.Add(deviceInformation);
			_DeviceNodes[deviceInformation] = iter;
			
			// register for the event so the deviceInformation can be removed upon disconnection
			device.Disconnected += new EventHandler(DeviceDisconnected);
		}		
        #endregion
	}
}
