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
using WiiDeviceLibrary;
using WiiDeviceLibrary.Extensions;

namespace LinuxGUITest
{
	
	
	public partial class BalanceBoardInformation : Gtk.Bin, IDeviceInformation
	{
        #region Fields
		private List<float> _WeightHistory = new List<float>();
		private IBalanceBoard _Board = null;
		private Random _Random = new Random();
		private float _BoxX = 0;
		private float _BoxY = 0;
		private Gtk.Widget _Separator;
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
			get { return _Board; }
		}
		
		public BalanceBoardInformation(IBalanceBoard board)
		{
			_Board = board;
			_BoxX = (float)_Random.NextDouble();
			_BoxY = (float)_Random.NextDouble();
			this.Build();			
		}
		
		public void Update()
		{
			entryTopLeft.Text = _Board.TopLeftWeight.ToString();
			entryTopRight.Text = _Board.TopRightWeight.ToString();
			entryBottomLeft.Text = _Board.BottomLeftWeight.ToString();
			entryBottomRight.Text = _Board.BottomRightWeight.ToString();
			entryTotalWeight.Text = _Board.TotalWeight.ToString();
			entryButton.Text = _Board.Button ? "Power" : "None";
			if(_Board.Led != checkboxLed.Active)
			{
				checkboxLed.Active = _Board.Led;
			}

			progressbar1.Fraction = (double)(_Board.BatteryLevel) / 0xC8;
			
			// keep a history of the weight in previous measurements to calculate an average
			_WeightHistory.Add(_Board.TotalWeight);
			if(_WeightHistory.Count > 20)
			{
				_WeightHistory.RemoveAt(0);
			}
			
			// calculate the average weight from the history
			float averageWeight = 0;
			for(int i = 0; i < _WeightHistory.Count; i++)
			{
				averageWeight += _WeightHistory[i] / _WeightHistory.Count;
			}
			entryAverageWeight.Text = averageWeight.ToString();
		
			drawingareaBalance.QueueDraw();
		}

		protected virtual void OnDrawingareaBalanceExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			Gdk.Window window = args.Event.Window;
			Gdk.GC gc = drawingareaBalance.Style.BlackGC;
			window.Background = new Gdk.Color(0,0,0);
			gc.RgbFgColor = new Gdk.Color(255,255,255);
			float width = args.Event.Area.Width;
			float height = args.Event.Area.Height;
			
			// draw grid
			window.DrawLine(gc, (int)(width / 2), 0, (int)(width / 2), (int)height);
			window.DrawLine(gc, 0, (int)(height / 2), (int)width, (int)(height / 2));
			
			// draw box
			int boxX = (int)(_BoxX * width - 5);
			int boxY = (int)(_BoxY * height - 5);
			window.DrawRectangle(gc, false, boxX, boxY, 10, 10);
			
			// draw balance point
			float total = _Board.BottomLeftWeight + _Board.BottomRightWeight + _Board.TopLeftWeight + _Board.TopRightWeight;
			float x = ((_Board.BottomRightWeight + _Board.TopRightWeight) - (_Board.BottomLeftWeight + _Board.TopLeftWeight)) / total * width / 2f;
			float y = ((_Board.BottomLeftWeight + _Board.BottomRightWeight) - (_Board.TopRightWeight + _Board.TopLeftWeight)) / total * height / 2f;
            int dotX = (int)(x + width / 2f - 1);
            int dotY = (int)(y + height / 2f - 1);
			window.DrawRectangle(gc, true, dotX, dotY, 3, 3);
			
			// test for collision, and relocate box on collision
			if(dotX+2 > boxX && dotX+2 < boxX+10 && dotY+2 > boxY && dotY+2 < boxY+10)
			{
				_BoxX = (float)_Random.NextDouble();
				_BoxY = (float)_Random.NextDouble();
			}
		}

		protected virtual void OnCheckboxLedPressed (object sender, System.EventArgs e)
		{
			_Board.Led = !_Board.Led;
		}

		protected virtual void OnDisconnectActionActivated (object sender, System.EventArgs e)
		{
			_Board.Disconnect();
		}

		protected virtual void OnBtnUpdateBatteryPressed (object sender, System.EventArgs e)
		{
			_Board.UpdateStatus();
		}
	}
}
