# Wii Device Library

The Wii Device Library is a cross platform C# class library that provides an interface to various Wii related devices, like the Wiimote, Classic Controller, Nunchuk and Balance Board.

It’s possible to use Wii devices like the Wiimote or Balance Board with your computer. All you need besides one of these devices is some kind of bluetooth device. This could either be a bluetooth dongle or bluetooth built into your pc or laptop.

When using the Wii Device Library you don’t have to worry about all the bluetooth stuff, you can just access your device of choice through a simple and intuitive interface. Also new Wii devices and extensions can be implemented by this interface without recompiling Wii Device Library.

Another nice feature is the ability to scan for Wii devices through bluetooth and connect to them when they are available. This makes it possible to automatically connect to Wii devices when they are syncing. It uses the libraries of several bluetooth stacks to scan, connect and communicate with the Wii devices. Also this part of the library can be extended with support for more bluetooth stacks and/or operating systems.

We would like to thank the members of [WiiBrew](http://www.wiibrew.org/) and [WiiLi](http://www.wiili.org/) for the time and effort they put into figuring out how the Wiimote, its extensions and the Balanceboard operate. We also want to mention Brian Peek and his [WiimoteLib](http://www.wiimotelib.org/), part of which was used in Wii Device Library. Without these sources, we wouldn't have been able to create this library.
