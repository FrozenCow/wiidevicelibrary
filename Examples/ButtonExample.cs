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
using System.Text;
using WiiDeviceLibrary;
using System.Threading;

namespace Examples
{
    public class ButtonExample
    {
        #region Connecting devices.
        #region Discovering devices.
        public static void Main(string[] args)
        {
            // An IDeviceProvider performs the discovering of and connecting to (bluetooth) devices.
            // An IDeviceInfo holds the information of a discovered device.

            // Create a DeviceProvider based on the installed software.
            IDeviceProvider deviceProvider = DeviceProviderRegistry.CreateSupportedDeviceProvider();
            deviceProvider.DeviceFound += deviceProvider_DeviceFound;
            deviceProvider.DeviceLost += deviceProvider_DeviceLost;
            deviceProvider.StartDiscovering();

            Console.WriteLine("The IDeviceProvider is discovering...");

            while (true) { Thread.Sleep(1000); }
        }
        
        // You can use this eventhandler the same way as deviceProvider_DeviceFound.
        static void deviceProvider_DeviceLost(object sender, DeviceInfoEventArgs e)
        {
            Console.WriteLine("A device has been lost.");
            IDeviceInfo lostDeviceInfo = e.DeviceInfo;

            if (lostDeviceInfo is IBluetoothDeviceInfo)
            {
                Console.WriteLine("The address of the Bluetooth device is {0}", ((IBluetoothDeviceInfo)lostDeviceInfo).Address);
            }
        }
        #endregion
        
        static void device_Disconnected(object sender, EventArgs e)
        {
            IDevice device = (IDevice)sender;
            Console.WriteLine("We have disconnected from the device.");
        }
        
        static void deviceProvider_DeviceFound(object sender, DeviceInfoEventArgs e)
        {
            IDeviceProvider deviceProvider = (IDeviceProvider)sender;

            Console.WriteLine("A device has been found.");
            IDeviceInfo foundDeviceInfo = e.DeviceInfo;

            IDevice device = deviceProvider.Connect(foundDeviceInfo);
            Console.WriteLine("Connected to the device.");
            
            device.Disconnected += device_Disconnected;
            
            if (device is IWiimote)
            {
                IWiimote wiimote = (IWiimote)device;
                Console.WriteLine("We have connected to a Wiimote device.");
                // Here we have access to all the operations that we can perform on a Wiimote.
                
                OnWiimoteConnected(wiimote);
            }
        }
        #endregion
        
        static void OnWiimoteConnected(IWiimote wiimote)
        {
            // To be informed about changes of the wiimote's status, we can use the Updated event:
            wiimote.Updated += wiimote_Updated;
        }
        
        // This field is used to know if an button was just pressed or released.
        // This is not required to check if an button is held down.
        static WiimoteButtons oldWiimoteButtons;
        
        static void wiimote_Updated(object sender, EventArgs e)
        {
            IWiimote wiimote = (IWiimote)sender;

            Console.WriteLine("The following buttons are held down: {0}", wiimote.Buttons);

            // To check if button 'A' is held down, we can do the following.
            // Note: this might look strange, but this is how C# works with flagged enumerations. 'WiimoteButtons' is a flagged enumeration.
            if ((wiimote.Buttons & WiimoteButtons.A) != WiimoteButtons.None)
            {
                Console.WriteLine("Button A is held down.");
            }

            // Here's an example of how to check if buttons were just pressed or released?
            // To understand what is happening here, some knowledge of flagged enumerations is required.
            WiimoteButtons changedButtons = oldWiimoteButtons ^ wiimote.Buttons;
            WiimoteButtons pressedButtons = changedButtons & wiimote.Buttons;
            WiimoteButtons releasedButtons = changedButtons & oldWiimoteButtons;
            oldWiimoteButtons = wiimote.Buttons;
        }
    }
}