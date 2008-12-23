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
using System.Threading;

namespace Examples
{
    public class IrExample
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
            wiimote.SetReportingMode(ReportingMode.Buttons10Ir9Extension);
        }

        static WiimoteButtons oldWiimoteButtons;
        static ReportingMode[] irReportingModes = new ReportingMode[] {
             ReportingMode.ButtonsAccelerometer12Ir,
             ReportingMode.Buttons10Ir9Extension,
             ReportingMode.ButtonsAccelerometer10Ir6Extension,
             ReportingMode.ButtonsAccelerometer36Ir };
        static int modeIndex = 0;
             
        static void wiimote_Updated(object sender, EventArgs e)
        {
            // There are several reporting modes that provide ir information. Choosing the
            // right one depends on the desired level of ir accuracy and other device information that is required.
            // The ir device has three different accuracy levels:
            // 1. Buttons10Ir9Extension / ButtonsAcceleromter10Ir6Extension provide the lowest level of accuracy,
            //    but also provide more information about other parts of the wiimote.
            // 2. ButtonsAccelerometer12Ir provides an increased level of accuracy
            //    by supplying additional data (the size of the beacon) at the cost of extension information.
            // 3. ButtonsAccelerometer36Ir provides the highest level of accuracy
            //    by supplying the intensity and the ... of the beacons
            //    but it is delivered in two seperate messages and is therefore two times slower than the other modes.
            
            IWiimote wiimote = (IWiimote)sender;

            switch (wiimote.ReportingMode)
            {
                case ReportingMode.Buttons10Ir9Extension:
                case ReportingMode.ButtonsAccelerometer10Ir6Extension:
                    Console.WriteLine("Basic IR ({0})", wiimote.ReportingMode);
                    foreach (BasicIRBeacon beacon in wiimote.IRBeacons)
                    {
                        // When a beacon is not found, the value will be null.
                        if (beacon != null)
                            Console.WriteLine("BasicBeacon: X={0} Y={1}", beacon.X, beacon.Y);
                    }
                    break;
                case ReportingMode.ButtonsAccelerometer12Ir:
                    Console.WriteLine("Extended IR ({0})", wiimote.ReportingMode);
                    foreach (ExtendedIRBeacon beacon in wiimote.IRBeacons)
                    {
                        if (beacon != null)
                            Console.WriteLine("ExtendedBeacon: X={0} Y={1} Size={2}", beacon.X, beacon.Y, beacon.Size);
                    }
                    break;
                case ReportingMode.ButtonsAccelerometer36Ir:
                    Console.WriteLine("Full IR ({0})", wiimote.ReportingMode);
                    foreach (FullIRBeacon beacon in wiimote.IRBeacons)
                    {
                        if (beacon != null)
                            Console.WriteLine("FullBeacon: X={0} Y={1} Size={2} XMin={3} XMax={4} YMin={5} YMax={6} Intensity={7}", 
                            beacon.X, beacon.Y, beacon.Size, beacon.XMin, beacon.XMax, beacon.YMin, beacon.YMax, beacon.Intensity);
                    }
                    break;
            }
                 
            // The following code is not part of the example, it is merely to switch between the different ReportingModes.
            WiimoteButtons changedButtons = oldWiimoteButtons ^ wiimote.Buttons;
            WiimoteButtons pressedButtons = changedButtons & wiimote.Buttons;
            oldWiimoteButtons = wiimote.Buttons;
            
            if((pressedButtons & WiimoteButtons.Plus) != WiimoteButtons.None)
            {
                modeIndex = (modeIndex + 1) % 4;
                wiimote.SetReportingMode(irReportingModes[modeIndex]);
            }
            if((pressedButtons & WiimoteButtons.Minus) != WiimoteButtons.None)
            {
                modeIndex = ((modeIndex - 1) + 4) % 4;
                wiimote.SetReportingMode(irReportingModes[modeIndex]);
            }
        }
    }
}