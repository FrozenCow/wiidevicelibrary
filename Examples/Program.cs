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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. DiscoverExample");
            Console.WriteLine("2. ConnectExample");
            Console.WriteLine("3. LedsExample");
            Console.WriteLine("4. ButtonExample");
            Console.WriteLine("5. ReportingModeExample");
            Console.WriteLine("6. AccelerometerExample");
            Console.WriteLine("7. IrExample");
            Console.WriteLine("8. NunchukExample");
            
            int choice = 0;
            while(choice == 0)
            {
                Console.Write("Enter a number to run one of the examples: "); 
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            DiscoverExample.Main(args);
                            break;
                        case 2:
                            ConnectExample.Main(args);
                            break;
                        case 3:
                            LedsExample.Main(args);
                            break;
                        case 4:
                            ButtonExample.Main(args);
                            break;
                        case 5:
                            ReportingModeExample.Main(args);
                            break;
                        case 6:
                            AccelerometerExample.Main(args);
                            break;
                        case 7:
                            IrExample.Main(args);
                            break;
                        case 8:
                            NunchukExample.Main(args);
                            break;
                        default:
                            choice = 0;
                            break;
                    }
                }
            }
        }
    }
}
