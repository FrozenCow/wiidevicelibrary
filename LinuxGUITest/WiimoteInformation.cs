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
using System.IO;
using WiiDeviceLibrary;
using System.Threading;
using WiiDeviceLibrary.Extensions;

namespace LinuxGUITest
{
	public partial class WiimoteInformation : Gtk.Bin, IDeviceInformation
	{
        #region Fields
		private IWiimote _Wiimote;
		private Gtk.Widget _Separator;
		private int _UpdateCounter = 0;
		private Gtk.Window _Parent;
		private IExtensionInformation _ExtensionInformation = null;
        #endregion
		
		public Gtk.Widget Separator
		{
			get { return _Separator; }
			set { _Separator = value; }
		}
		
		public Gtk.Widget Widget
		{
			get { return this; }
		}
		
		public IDevice Device
		{
			get { return _Wiimote; }
		}			
		
		public WiimoteInformation(Gtk.Window parent, IWiimote wiimote)
		{
			_Parent = parent;
			_Wiimote = wiimote;
			if(_Wiimote.Extension != null)
				ExtensionAttached(_Wiimote, new WiimoteExtensionEventArgs(_Wiimote.Extension));
			
			_Wiimote.ExtensionAttached += new EventHandler<WiimoteExtensionEventArgs>(ExtensionAttached);
			_Wiimote.ExtensionDetached += new EventHandler<WiimoteExtensionEventArgs>(ExtensionDetached);
			this.Build();
            foreach (ReportingMode reportingMode in Enum.GetValues(typeof(ReportingMode)))
			{
                comboboxReportingMode.AppendText(reportingMode.ToString());
			}
		}
		
		private void ExtensionAttached(object sender, WiimoteExtensionEventArgs args)
		{
			Gtk.Application.Invoke(delegate{
				if(_Wiimote.Extension is NunchukExtension)
				{
					_ExtensionInformation = new NunchukInformation((NunchukExtension)_Wiimote.Extension);
					GtkAlignment2.Add(_ExtensionInformation.Widget);
				}
				else if(_Wiimote.Extension is GuitarExtension)
				{
					_ExtensionInformation = new GuitarInformation((GuitarExtension)_Wiimote.Extension);						
					GtkAlignment2.Add(_ExtensionInformation.Widget);
				}
				else if(_Wiimote.Extension is ClassicControllerExtension)
				{
					_ExtensionInformation = new ClassicControllerInformation((ClassicControllerExtension)_Wiimote.Extension);
					GtkAlignment2.Add(_ExtensionInformation.Widget);
				}
				_Wiimote.SetReportingMode(_Wiimote.ReportingMode);
			});
		}
		
		private void ExtensionDetached(object sender, WiimoteExtensionEventArgs args)
		{
			Gtk.Application.Invoke(delegate
			{
				GtkAlignment2.Remove(_ExtensionInformation.Widget);
				_ExtensionInformation = null;
				_Wiimote.SetReportingMode(_Wiimote.ReportingMode);
			});
		}		
		
		private bool IsLedOn(WiimoteLeds led)
		{
			return (_Wiimote.Leds & led) == led;
		}

		public void Update()
		{
			if(_UpdateCounter == 0)
			{
				_Wiimote.IsRumbling = true;
			}
			else if(_UpdateCounter == 20)
			{
				_Wiimote.IsRumbling = false;
			}
			
			// update leds
			if(IsLedOn(WiimoteLeds.Led1) != checkboxLed1.Active)
				checkboxLed1.Active = IsLedOn(WiimoteLeds.Led1);
			if(IsLedOn(WiimoteLeds.Led2) != checkboxLed2.Active)
				checkboxLed2.Active = IsLedOn(WiimoteLeds.Led2);
			if(IsLedOn(WiimoteLeds.Led3) != checkboxLed3.Active)
				checkboxLed3.Active = IsLedOn(WiimoteLeds.Led3);
			if(IsLedOn(WiimoteLeds.Led4) != checkboxLed4.Active)
				checkboxLed4.Active = IsLedOn(WiimoteLeds.Led4);
			
			// update rumble
			if(_Wiimote.IsRumbling != checkboxRumble.Active)
				checkboxRumble.Active = _Wiimote.IsRumbling;
			
			// update buttons
			entryButtons.Text = _Wiimote.Buttons.ToString();
			
			// update accelerometers
			entryX.Text = _Wiimote.Accelerometer.Calibrated.X.ToString();
            entryY.Text = _Wiimote.Accelerometer.Calibrated.Y.ToString();
            entryZ.Text = _Wiimote.Accelerometer.Calibrated.Z.ToString();
			
			progressbarBattery.Fraction = (double)(_Wiimote.BatteryLevel) / 0xC8;
			
			drawingareaIR.QueueDraw();
			
			if(_ExtensionInformation != null)
				_ExtensionInformation.Update();
			_UpdateCounter++;			
		}
		
		protected virtual void OnDrawingareaIRExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			Gdk.Window window = args.Event.Window;
			Gdk.GC gc = drawingareaIR.Style.BlackGC;
			window.Background = new Gdk.Color(0,0,0);
			gc.RgbFgColor = new Gdk.Color(255,255,255);
			
			float scaleFactor = (float)args.Event.Area.Width / 1024f;			
			
			int count = 0;
	        foreach (BasicIRBeacon irBeacon in _Wiimote.IRBeacons)
	        {
	            if (irBeacon == null)
	                continue;
				window.DrawRectangle(gc, true, new Gdk.Rectangle(args.Event.Area.Width - (int)(scaleFactor * irBeacon.X - 2), (int)(scaleFactor * irBeacon.Y - 2), 4, 4));
				count++;
	        }			
			
			if(count == 0)
			{
				gc.RgbFgColor = new Gdk.Color(255,0,0);
				window.DrawLine(gc, 0,0,args.Event.Area.Width, args.Event.Area.Height);
				window.DrawLine(gc, args.Event.Area.Width,0,0, args.Event.Area.Height);
			}
		}		

		protected virtual void OnCheckboxLed1Pressed (object sender, System.EventArgs e)
		{
			_Wiimote.Leds = _Wiimote.Leds ^ WiimoteLeds.Led1; 
		}

		protected virtual void OnCheckboxLed2Pressed (object sender, System.EventArgs e)
		{
			_Wiimote.Leds = _Wiimote.Leds ^ WiimoteLeds.Led2;
		}

		protected virtual void OnCheckboxLed3Pressed (object sender, System.EventArgs e)
		{
			_Wiimote.Leds = _Wiimote.Leds ^ WiimoteLeds.Led3;
		}

		protected virtual void OnCheckboxLed4Pressed (object sender, System.EventArgs e)
		{
			_Wiimote.Leds = _Wiimote.Leds ^ WiimoteLeds.Led4;
		}

		protected virtual void OnCheckboxRumblePressed (object sender, System.EventArgs e)
		{
			_Wiimote.IsRumbling = !_Wiimote.IsRumbling;
		}

		protected virtual void OnComboboxReportingModeChanged (object sender, System.EventArgs e)
		{
            foreach (ReportingMode reportingMode in Enum.GetValues(typeof(ReportingMode)))
			{
                if (comboboxReportingMode.ActiveText == reportingMode.ToString())
				{
                    if (reportingMode != _Wiimote.ReportingMode)
                        _Wiimote.SetReportingMode(reportingMode);
				}
			}			
		}

		protected virtual void OnBtnUpdateBatteryPressed (object sender, System.EventArgs e)
		{
			_Wiimote.UpdateStatus();
		}

		protected virtual void OnDisconnectActionActivated (object sender, System.EventArgs e)
		{
			_Wiimote.Disconnect();
		}

		private void ThreadDumpMemory(object stateInfo)
		{
			if(!(stateInfo is Gtk.FileChooserDialog))
				return;
			Gtk.FileChooserDialog dialog = stateInfo as Gtk.FileChooserDialog;
			
			using (Stream fileStream = File.Create(dialog.Filename))
            {
                byte[] memoryBuffer = new byte[0x1700];
				_Wiimote.ReadMemory(0x000000, memoryBuffer, 0, (short)memoryBuffer.Length);
                fileStream.Write(memoryBuffer, 0, memoryBuffer.Length);
                fileStream.Close();
            }
			dialog.Hide();
		}
		
		protected virtual void OnDumpMemoryActionActivated (object sender, System.EventArgs e)
		{
            Gtk.FileChooserDialog dialog = new Gtk.FileChooserDialog("Save the memory dump", _Parent, Gtk.FileChooserAction.Save, Gtk.Stock.Cancel, Gtk.ResponseType.Cancel, Gtk.Stock.Save, Gtk.ResponseType.Ok);
            dialog.DoOverwriteConfirmation = true;
            Gtk.ResponseType result = (Gtk.ResponseType)dialog.Run();
            if (result == Gtk.ResponseType.Ok)
            {
                dialog.Modal = true;
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadDumpMemory), dialog);
                ThreadDumpMemory(dialog);
            }
            else
            {
                dialog.Hide();
            }
		}

		private void ThreadPlaySound(object stateInfo)
		{
            int readBytes;
            using (Stream pcmStream = new FileStream(@"Audio.au", FileMode.Open))
            {
                using (Stream adpcmStream = new Pcm8ToAdpcm4Stream(pcmStream))
                {
                    using (Stream speakerStream = new WiimoteSpeakerStream(_Wiimote))
                    {

                        byte[] adpcm_data = new byte[20];

                        _Wiimote.IsSpeakerEnabled = true;

                        while ((readBytes = adpcmStream.Read(adpcm_data, 0, 20)) > 0)
                            speakerStream.Write(adpcm_data, 0, readBytes);

                        _Wiimote.IsSpeakerEnabled = false;

                        speakerStream.Close();
                    }
                    adpcmStream.Close();
                }
                pcmStream.Close();
            }			
		}

        private void CalibrateWiimote()
        {
            IWiimote wiimote = (IWiimote)Device;
            ReportingMode oldReportingMode = wiimote.ReportingMode;
            wiimote.SetReportingMode(ReportingMode.ButtonsAccelerometer);
            AccelerometerAxes<ushort> raw = wiimote.Accelerometer.Raw;
            Gtk.MessageDialog dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info, Gtk.ButtonsType.OkCancel, false, "Place the wiimote on a table.", new object[0]);
            if (dialog.Run() == -5)
            {
                ushort xZero, yZero, zZero, xOne, yOne, zOne = 0;
                xZero = raw.X;
                yZero = raw.Y;
                zOne = raw.Z;
                dialog.Markup = "Place the wiimote on its left side.";
                if (dialog.Run() == -5)
                {
                    xOne = raw.X;
                    zZero = raw.Z;

                    // Invert zOne (so that the values are negated).
                    zOne = (ushort)(zZero - (zOne - zZero));
                    
                    dialog.Markup = "Place the wiimote on its lower side, so that it points up.";
                    if (dialog.Run() == -5)
                    {
                        yOne = raw.Y;

                        wiimote.WriteMemory(0x16, new byte[] {
                            (byte)xZero, (byte)yZero, (byte)zZero, 0,
                            (byte)xOne, (byte)yOne, (byte)zOne, 0 }, 0, 8);

                        wiimote.Initialize();
                    }
                }
            }
            dialog.Destroy();
            wiimote.SetReportingMode(oldReportingMode);
        }
		
		protected virtual void OnPlaySoundActionActivated (object sender, System.EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPlaySound));
		}

		protected virtual void OnCalibrateActionActivated (object sender, System.EventArgs e)
		{
			CalibrateWiimote();
		}
	}
}
