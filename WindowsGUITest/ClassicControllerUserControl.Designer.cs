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
    partial class ClassicControllerUserControl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.leftTriggerBox = new System.Windows.Forms.TextBox();
            this.rightTriggerBox = new System.Windows.Forms.TextBox();
            this.leftStickXBox = new System.Windows.Forms.TextBox();
            this.leftStickYBox = new System.Windows.Forms.TextBox();
            this.rightStickXBox = new System.Windows.Forms.TextBox();
            this.rightStickYBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buttons";
            // 
            // buttonsBox
            // 
            this.buttonsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsBox.Location = new System.Drawing.Point(52, 4);
            this.buttonsBox.Name = "buttonsBox";
            this.buttonsBox.ReadOnly = true;
            this.buttonsBox.Size = new System.Drawing.Size(180, 20);
            this.buttonsBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rightTriggerBox);
            this.groupBox1.Controls.Add(this.leftTriggerBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Triggers";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.leftStickYBox);
            this.groupBox2.Controls.Add(this.leftStickXBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 50);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LeftStick";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rightStickYBox);
            this.groupBox3.Controls.Add(this.rightStickXBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 146);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 50);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RightStick";
            // 
            // leftTriggerBox
            // 
            this.leftTriggerBox.Location = new System.Drawing.Point(7, 20);
            this.leftTriggerBox.Name = "leftTriggerBox";
            this.leftTriggerBox.ReadOnly = true;
            this.leftTriggerBox.Size = new System.Drawing.Size(100, 20);
            this.leftTriggerBox.TabIndex = 0;
            // 
            // rightTriggerBox
            // 
            this.rightTriggerBox.Location = new System.Drawing.Point(114, 20);
            this.rightTriggerBox.Name = "rightTriggerBox";
            this.rightTriggerBox.ReadOnly = true;
            this.rightTriggerBox.Size = new System.Drawing.Size(100, 20);
            this.rightTriggerBox.TabIndex = 1;
            // 
            // leftStickXBox
            // 
            this.leftStickXBox.Location = new System.Drawing.Point(7, 20);
            this.leftStickXBox.Name = "leftStickXBox";
            this.leftStickXBox.ReadOnly = true;
            this.leftStickXBox.Size = new System.Drawing.Size(100, 20);
            this.leftStickXBox.TabIndex = 0;
            // 
            // leftStickYBox
            // 
            this.leftStickYBox.Location = new System.Drawing.Point(114, 20);
            this.leftStickYBox.Name = "leftStickYBox";
            this.leftStickYBox.ReadOnly = true;
            this.leftStickYBox.Size = new System.Drawing.Size(100, 20);
            this.leftStickYBox.TabIndex = 1;
            // 
            // rightStickXBox
            // 
            this.rightStickXBox.Location = new System.Drawing.Point(7, 20);
            this.rightStickXBox.Name = "rightStickXBox";
            this.rightStickXBox.ReadOnly = true;
            this.rightStickXBox.Size = new System.Drawing.Size(100, 20);
            this.rightStickXBox.TabIndex = 0;
            // 
            // rightStickYBox
            // 
            this.rightStickYBox.Location = new System.Drawing.Point(114, 20);
            this.rightStickYBox.Name = "rightStickYBox";
            this.rightStickYBox.ReadOnly = true;
            this.rightStickYBox.Size = new System.Drawing.Size(100, 20);
            this.rightStickYBox.TabIndex = 1;
            // 
            // ClassicControllerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonsBox);
            this.Controls.Add(this.label1);
            this.Name = "ClassicControllerUserControl";
            this.Size = new System.Drawing.Size(235, 203);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox buttonsBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox rightTriggerBox;
        private System.Windows.Forms.TextBox leftTriggerBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox leftStickYBox;
        private System.Windows.Forms.TextBox leftStickXBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox rightStickYBox;
        private System.Windows.Forms.TextBox rightStickXBox;
    }
}
