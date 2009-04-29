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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WiiDeviceLibrary;

namespace WindowsGUITest
{
    public partial class BalanceBoardUserControl : UserControl
    {
        Random random = new Random();

        float averageWeight;

        // The coordinates for the 'get the box' mini-game.
        float _BoxX, _BoxY;

        private IBalanceBoard _BalanceBoard;
        public IBalanceBoard BalanceBoard
        {
            get { return _BalanceBoard; }
            set
            {
                if (_BalanceBoard != value)
                {
                    _BalanceBoard = value;
                    InitializeBalanceboard();
                }
            }
        }

        public BalanceBoardUserControl()
        {
            InitializeComponent();

            _BoxX = (float)random.NextDouble();
            _BoxY = (float)random.NextDouble();
        }

        private void InitializeBalanceboard()
        {
            if (!IsHandleCreated)
                CreateControl();
            ledCheck.Checked = BalanceBoard.Led;

            BalanceBoard.Updated += BalanceBoard_Updated;
        }

        void BalanceBoard_Updated(object sender, EventArgs e)
        {
            if (BalanceBoard != null)
            {
                balanceBox.Invalidate();
            }
        }

        public void UpdateUI()
        {
            buttonBox.Text = BalanceBoard.Button.ToString();
            topLeftWeightBox.Text = BalanceBoard.TopLeftWeight.ToString();
            topRightWeightBox.Text = BalanceBoard.TopRightWeight.ToString();
            bottomLeftWeightBox.Text = BalanceBoard.BottomLeftWeight.ToString();
            bottomRightWeightBox.Text = BalanceBoard.BottomRightWeight.ToString();
            totalWeightBox.Text = BalanceBoard.TotalWeight.ToString();
            averageWeight = averageWeight * 0.9f + BalanceBoard.TotalWeight * 0.1f;
            averageWeightBox.Text = averageWeight.ToString();
        }

        private void balanceBox_Paint(object sender, PaintEventArgs e)
        {
            UpdateUI();

            Graphics g = e.Graphics;
            Pen p = Pens.White;
            float width = balanceBox.Width;
            float height = balanceBox.Height;

            // draw grid
            g.DrawLine(p, (int)(width / 2), 0, (int)(width / 2), (int)height);
            g.DrawLine(p, 0, (int)(height / 2), (int)width, (int)(height / 2));

            // draw box
            int boxX = (int)(_BoxX * width - 5);
            int boxY = (int)(_BoxY * height - 5);
            g.DrawRectangle(p, boxX, boxY, 10, 10);

            // draw balance point
            float total = BalanceBoard.BottomLeftWeight + BalanceBoard.BottomRightWeight + BalanceBoard.TopLeftWeight + BalanceBoard.TopRightWeight;
            float x = ((BalanceBoard.BottomRightWeight + BalanceBoard.TopRightWeight) - (BalanceBoard.BottomLeftWeight + BalanceBoard.TopLeftWeight)) / total * width / 2f;
            float y = ((BalanceBoard.BottomLeftWeight + BalanceBoard.BottomRightWeight) - (BalanceBoard.TopRightWeight + BalanceBoard.TopLeftWeight)) / total * height / 2f;
            int dotX = (int)(x + width / 2f - 1);
            int dotY = (int)(y + height / 2f - 1);
            if (dotX >= 0 && dotX < width &&
                dotY >= 0 && dotY < height)
                g.DrawRectangle(p, dotX, dotY, 3, 3);

            // test for collision, and relocate box on collision
            if (dotX + 2 > boxX && dotX + 2 < boxX + 10 && dotY + 2 > boxY && dotY + 2 < boxY + 10)
            {
                _BoxX = (float)random.NextDouble();
                _BoxY = (float)random.NextDouble();
            }
        }

        private void ledCheck_CheckedChanged(object sender, EventArgs e)
        {
            BalanceBoard.Led = ledCheck.Checked;
        }
    }
}
