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

namespace WindowsGUITest
{
    partial class NunchukUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonsBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.analogstickXBox = new System.Windows.Forms.TextBox();
            this.analogstickYBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.accelerometerXBox = new System.Windows.Forms.TextBox();
            this.accelerometerYBox = new System.Windows.Forms.TextBox();
            this.accelerometerZBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buttons";
            // 
            // buttonsBox
            // 
            this.buttonsBox.Location = new System.Drawing.Point(52, 3);
            this.buttonsBox.Name = "buttonsBox";
            this.buttonsBox.ReadOnly = true;
            this.buttonsBox.Size = new System.Drawing.Size(285, 20);
            this.buttonsBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.analogstickYBox);
            this.groupBox1.Controls.Add(this.analogstickXBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analog Stick";
            // 
            // analogstickXBox
            // 
            this.analogstickXBox.Location = new System.Drawing.Point(7, 20);
            this.analogstickXBox.Name = "analogstickXBox";
            this.analogstickXBox.ReadOnly = true;
            this.analogstickXBox.Size = new System.Drawing.Size(100, 20);
            this.analogstickXBox.TabIndex = 0;
            // 
            // analogstickYBox
            // 
            this.analogstickYBox.Location = new System.Drawing.Point(114, 20);
            this.analogstickYBox.Name = "analogstickYBox";
            this.analogstickYBox.ReadOnly = true;
            this.analogstickYBox.Size = new System.Drawing.Size(100, 20);
            this.analogstickYBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.accelerometerZBox);
            this.groupBox2.Controls.Add(this.accelerometerYBox);
            this.groupBox2.Controls.Add(this.accelerometerXBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 49);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accelerometers";
            // 
            // accelerometerXBox
            // 
            this.accelerometerXBox.Location = new System.Drawing.Point(7, 20);
            this.accelerometerXBox.Name = "accelerometerXBox";
            this.accelerometerXBox.ReadOnly = true;
            this.accelerometerXBox.Size = new System.Drawing.Size(100, 20);
            this.accelerometerXBox.TabIndex = 0;
            // 
            // accelerometerYBox
            // 
            this.accelerometerYBox.Location = new System.Drawing.Point(114, 20);
            this.accelerometerYBox.Name = "accelerometerYBox";
            this.accelerometerYBox.ReadOnly = true;
            this.accelerometerYBox.Size = new System.Drawing.Size(100, 20);
            this.accelerometerYBox.TabIndex = 1;
            // 
            // accelerometerZBox
            // 
            this.accelerometerZBox.Location = new System.Drawing.Point(221, 20);
            this.accelerometerZBox.Name = "accelerometerZBox";
            this.accelerometerZBox.ReadOnly = true;
            this.accelerometerZBox.Size = new System.Drawing.Size(100, 20);
            this.accelerometerZBox.TabIndex = 2;
            // 
            // NunchukUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonsBox);
            this.Controls.Add(this.label1);
            this.Name = "NunchukUserControl";
            this.Size = new System.Drawing.Size(343, 143);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox buttonsBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox analogstickYBox;
        private System.Windows.Forms.TextBox analogstickXBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox accelerometerZBox;
        private System.Windows.Forms.TextBox accelerometerYBox;
        private System.Windows.Forms.TextBox accelerometerXBox;
    }
}
