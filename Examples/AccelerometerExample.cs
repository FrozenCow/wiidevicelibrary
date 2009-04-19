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
    public class AccelerometerExample
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
                Console.WriteLine("The address of the Bluetooth device is {0}", ((IBluetoothDeviceInfo)lostDeviceInfo).BluetoothAddress);
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
            wiimote.Updated += wiimote_Updated;
            
            // In this example we will need Accelerometer-data. For this, we change the ReportingMode to ButtonsAccelerometer.
            wiimote.SetReportingMode(ReportingMode.ButtonsAccelerometer);
        }
        
        static void wiimote_Updated(object sender, EventArgs e)
        {
            IWiimote wiimote = (IWiimote)sender;

            // The accelerometer data can be read out either raw or calibrated. The
            // calibrated values present a more usable form that can be used directly.
            // Normally you will use the calibrated values because they are the same for every Wii device.
            // The range of the calibrated values is 0.0 to 1.0. In contrast the range of the raw 
            // values is different for each Wii device. 
            

            // Using the raw data:
            // On their own these values are pretty useless, you could however calibrate them yourself using
            // the calibration-values available in 'wiimote.Accelerometer.Calibration', but luckily WiiDeviceLibrary does that for you :)
            Console.WriteLine("Raw values: X={0} Y={1} Z={2}", wiimote.Accelerometer.Raw.X, wiimote.Accelerometer.Raw.Y, wiimote.Accelerometer.Raw.Z);


            // Using the calibrated data:
            Console.WriteLine("Calibrated values: X={0} Y={1} Z={2}", wiimote.Accelerometer.Calibrated.X, wiimote.Accelerometer.Calibrated.Y, wiimote.Accelerometer.Calibrated.Z);
            
        }
    }
}