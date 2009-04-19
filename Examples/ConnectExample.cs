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
    public class ConnectExample
    {
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
                Console.WriteLine("The address of the Bluetooth device is {0}", ((IBluetoothDeviceInfo)lostDeviceInfo).BluetoothAddress);
            }
        }
        #endregion
        
        static void deviceProvider_DeviceFound(object sender, DeviceInfoEventArgs e)
        {
            IDeviceProvider deviceProvider = (IDeviceProvider)sender;

            Console.WriteLine("A device has been found.");
            IDeviceInfo foundDeviceInfo = e.DeviceInfo;

            // Example 2. Connecting to a discovered device.
            // When you connect to a discovered device, it will result in a IDevice.
            // IDevice represents the device when connected.
            Console.WriteLine("Connecting to the device...");
            IDevice device = deviceProvider.Connect(foundDeviceInfo);
            Console.WriteLine("Connected to the device.");
            
            // You have connected to the device,
            // and can now use 'device' to interact with the connected device.
            // Note that 'device.DeviceInfo' is refering to 'foundDeviceInfo', which we used to connect to the device.
            // We can use the Disconnected event to stay informed if we can still use the device.
            device.Disconnected += device_Disconnected;
            
            // When the 'device' is an 'IDevice' you can only access properties that are common to 
            // all Devices (Wiimote, BalanceBoard, etc...).
            // These devices have their own types in WiiDeviceLibrary: IWiimote and IBalanceBoard. Both of these types inherit from IDevice.
            // To use properties specific to the devices you will have to cast the 'device' to the proper interface. 
            
            // First we check if the device is the desired type.
            if (device is IWiimote)
            {
                IWiimote wiimote = (IWiimote)device;
                Console.WriteLine("We have connected to a Wiimote device.");
                // Here we have access to all the operations that we can perform on a Wiimote.
                // That's it for connecting to Wii devices.
            }
            else if (device is IBalanceBoard)
            {
                IBalanceBoard balanceboard = (IBalanceBoard)device;
                Console.WriteLine("We have connected to a BalanceBoard device.");
                // Here we have access to all the operations that we can perform on a BalanceBoard.
            }
            
            // If we don't want to be connected to the device, we can disconnect like this:
            device.Disconnect();
        }
        
        static void device_Disconnected(object sender, EventArgs e)
        {
            IDevice device = (IDevice)sender;
            Console.WriteLine("We have disconnected from the device.");
        }
    }
}