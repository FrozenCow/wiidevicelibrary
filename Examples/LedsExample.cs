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
    public class LedsExample
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
            
            // If we don't want to be connected to the device, we can disconnect like this:
            device.Disconnect();
        }
        #endregion
        
        static void OnWiimoteConnected(IWiimote wiimote)
        {
            // It is recommended to always set the leds to a value, so that they will stop flashing.
            Console.WriteLine("Notice that when we are connected to the wiimote and have not yet set the leds to any value, the leds will keep on flashing.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        
            // We set the first and fourth leds on, the rest off.
            // Notice that we only supply the leds that must be on. All others will be off.
            wiimote.Leds = WiimoteLeds.Led1 | WiimoteLeds.Led4;

            Console.WriteLine("Leds: X . . X");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // We can 'add' leds that must be on.
            wiimote.Leds |= WiimoteLeds.Led3;

            Console.WriteLine("Leds: X . X X");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // We can 'remove' leds that must be on.
            wiimote.Leds ^= WiimoteLeds.Led4;

            Console.WriteLine("Leds: X . X .");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}