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
using System.Text;
using WiiDeviceLibrary;
using WiiDeviceLibrary.Extensions;
using System.Threading;

namespace Examples
{
    public class NunchukExample
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
            wiimote.ExtensionAttached += wiimote_ExtensionAttached;
            wiimote.ExtensionDetached += wiimote_ExtensionDetached;
            
            wiimote.SetReportingMode(ReportingMode.Buttons10Ir9Extension);
        }

        static void wiimote_ExtensionAttached(object sender, WiimoteExtensionEventArgs e)
        {
            IWiimote wiimote = (IWiimote)sender;
            
            // We can retrieve the attached extension by doing the following:
            IWiimoteExtension extension = e.Extension;
            // The extension is also available through 'wiimote.Extension'.
            
            // Here we can check what type of extension the attached extension is.
            // In this example we will only cover the Nunchuk, but we can also check for other available extensions.
            if (extension is NunchukExtension)
            {
                Console.WriteLine("A nunchuk attached to the Wiimote.");
            }
            // A few 'dummy-extensions' are available to detect various undefined situations.
            else if (extension is InvalidExtension)
            {
                Console.WriteLine("An extension was partially connected or the extension was erroneous.");
            }
            
            // UnknownExtension is a dummy-extension that that the attached extension
            // is not supported by Wii Device Library (or any other extension that is registered in WiimoteExtensionRegistry).
            else if (extension is UnknownExtension)
            {
                Console.WriteLine("An extension was connected, but was not recognized by Wii Device Library.");
            }
            else
            {
                Console.WriteLine("An extension was connected, but was not recognized by this example.");
            }
            wiimote.SetReportingMode(wiimote.ReportingMode);
        }

        static void wiimote_ExtensionDetached(object sender, WiimoteExtensionEventArgs e)
        {
            IWiimote wiimote = (IWiimote)sender;
            IWiimoteExtension extension = e.Extension;
            if (extension is NunchukExtension)
            {
                Console.WriteLine("A nunchuk was detached from the Wiimote.");
            }
            wiimote.SetReportingMode(wiimote.ReportingMode);
        }
        
        static void wiimote_Updated(object sender, EventArgs e)
        {
            IWiimote wiimote = (IWiimote)sender;
            if(wiimote.Extension is NunchukExtension)
            {
                NunchukExtension nunchuk = (NunchukExtension)wiimote.Extension;
                Console.Write("Buttons pushed down: {0} ",  nunchuk.Buttons);
                Console.Write("Stick: X={0,5:0.00} Y={1,5:0.00} ", nunchuk.Stick.Calibrated.X, nunchuk.Stick.Calibrated.Y);
                Console.WriteLine("Accelerometer: X={0,5:0.00} Y={1,5:0.00} Z={2,5:0.00}",  nunchuk.Accelerometer.Calibrated.X, nunchuk.Accelerometer.Calibrated.Y, nunchuk.Accelerometer.Calibrated.Z);
                
            }
        }
    }
}