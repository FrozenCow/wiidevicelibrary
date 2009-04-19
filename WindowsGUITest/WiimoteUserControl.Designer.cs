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

namespace WindowsGUITest
{
    partial class WiimoteUserControl
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
            this.reportingmodeBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonsBox = new System.Windows.Forms.TextBox();
            this.accelerometersGroupBox = new System.Windows.Forms.GroupBox();
            this.accelerometerZBox = new System.Windows.Forms.TextBox();
            this.accelerometerYBox = new System.Windows.Forms.TextBox();
            this.accelerometerXBox = new System.Windows.Forms.TextBox();
            this.led1Check = new System.Windows.Forms.CheckBox();
            this.led2Check = new System.Windows.Forms.CheckBox();
            this.led3Check = new System.Windows.Forms.CheckBox();
            this.led4Check = new System.Windows.Forms.CheckBox();
            this.rumbleCheck = new System.Windows.Forms.CheckBox();
            this.batteryBar = new System.Windows.Forms.ProgressBar();
            this.updateBatteryButton = new System.Windows.Forms.Button();
            this.irBox = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.extensionBox = new System.Windows.Forms.GroupBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.wiimoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.accelerometersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reporting Mode";
            // 
            // reportingmodeBox
            // 
            this.reportingmodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.reportingmodeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportingmodeBox.FormattingEnabled = true;
            this.reportingmodeBox.Location = new System.Drawing.Point(89, 3);
            this.reportingmodeBox.Name = "reportingmodeBox";
            this.reportingmodeBox.Size = new System.Drawing.Size(308, 21);
            this.reportingmodeBox.TabIndex = 1;
            this.reportingmodeBox.SelectedIndexChanged += new System.EventHandler(this.reportingmodeBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Buttons";
            // 
            // buttonsBox
            // 
            this.buttonsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsBox.Location = new System.Drawing.Point(89, 30);
            this.buttonsBox.Name = "buttonsBox";
            this.buttonsBox.ReadOnly = true;
            this.buttonsBox.Size = new System.Drawing.Size(308, 20);
            this.buttonsBox.TabIndex = 3;
            // 
            // accelerometersGroupBox
            // 
            this.accelerometersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accelerometersGroupBox.Controls.Add(this.accelerometerZBox);
            this.accelerometersGroupBox.Controls.Add(this.accelerometerYBox);
            this.accelerometersGroupBox.Controls.Add(this.accelerometerXBox);
            this.accelerometersGroupBox.Location = new System.Drawing.Point(3, 3);
            this.accelerometersGroupBox.Name = "accelerometersGroupBox";
            this.accelerometersGroupBox.Size = new System.Drawing.Size(394, 49);
            this.accelerometersGroupBox.TabIndex = 4;
            this.accelerometersGroupBox.TabStop = false;
            this.accelerometersGroupBox.Text = "Accelerometers";
            // 
            // accelerometerZBox
            // 
            this.accelerometerZBox.Location = new System.Drawing.Point(186, 19);
            this.accelerometerZBox.Name = "accelerometerZBox";
            this.accelerometerZBox.ReadOnly = true;
            this.accelerometerZBox.Size = new System.Drawing.Size(84, 20);
            this.accelerometerZBox.TabIndex = 4;
            // 
            // accelerometerYBox
            // 
            this.accelerometerYBox.Location = new System.Drawing.Point(96, 19);
            this.accelerometerYBox.Name = "accelerometerYBox";
            this.accelerometerYBox.ReadOnly = true;
            this.accelerometerYBox.Size = new System.Drawing.Size(84, 20);
            this.accelerometerYBox.TabIndex = 3;
            // 
            // accelerometerXBox
            // 
            this.accelerometerXBox.Location = new System.Drawing.Point(6, 19);
            this.accelerometerXBox.Name = "accelerometerXBox";
            this.accelerometerXBox.ReadOnly = true;
            this.accelerometerXBox.Size = new System.Drawing.Size(84, 20);
            this.accelerometerXBox.TabIndex = 2;
            // 
            // led1Check
            // 
            this.led1Check.AutoSize = true;
            this.led1Check.Location = new System.Drawing.Point(3, 58);
            this.led1Check.Name = "led1Check";
            this.led1Check.Size = new System.Drawing.Size(50, 17);
            this.led1Check.TabIndex = 5;
            this.led1Check.Text = "Led1";
            this.led1Check.UseVisualStyleBackColor = true;
            this.led1Check.CheckedChanged += new System.EventHandler(this.ledCheck_CheckedChanged);
            // 
            // led2Check
            // 
            this.led2Check.AutoSize = true;
            this.led2Check.Location = new System.Drawing.Point(60, 58);
            this.led2Check.Name = "led2Check";
            this.led2Check.Size = new System.Drawing.Size(50, 17);
            this.led2Check.TabIndex = 6;
            this.led2Check.Text = "Led2";
            this.led2Check.UseVisualStyleBackColor = true;
            this.led2Check.CheckedChanged += new System.EventHandler(this.ledCheck_CheckedChanged);
            // 
            // led3Check
            // 
            this.led3Check.AutoSize = true;
            this.led3Check.Location = new System.Drawing.Point(117, 58);
            this.led3Check.Name = "led3Check";
            this.led3Check.Size = new System.Drawing.Size(50, 17);
            this.led3Check.TabIndex = 7;
            this.led3Check.Text = "Led3";
            this.led3Check.UseVisualStyleBackColor = true;
            this.led3Check.CheckedChanged += new System.EventHandler(this.ledCheck_CheckedChanged);
            // 
            // led4Check
            // 
            this.led4Check.AutoSize = true;
            this.led4Check.Location = new System.Drawing.Point(174, 58);
            this.led4Check.Name = "led4Check";
            this.led4Check.Size = new System.Drawing.Size(50, 17);
            this.led4Check.TabIndex = 8;
            this.led4Check.Text = "Led4";
            this.led4Check.UseVisualStyleBackColor = true;
            this.led4Check.CheckedChanged += new System.EventHandler(this.ledCheck_CheckedChanged);
            // 
            // rumbleCheck
            // 
            this.rumbleCheck.AutoSize = true;
            this.rumbleCheck.Location = new System.Drawing.Point(231, 58);
            this.rumbleCheck.Name = "rumbleCheck";
            this.rumbleCheck.Size = new System.Drawing.Size(62, 17);
            this.rumbleCheck.TabIndex = 9;
            this.rumbleCheck.Text = "Rumble";
            this.rumbleCheck.UseVisualStyleBackColor = true;
            this.rumbleCheck.CheckedChanged += new System.EventHandler(this.rumbleCheck_CheckedChanged);
            // 
            // batteryBar
            // 
            this.batteryBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.batteryBar.Location = new System.Drawing.Point(101, 81);
            this.batteryBar.Maximum = 255;
            this.batteryBar.Name = "batteryBar";
            this.batteryBar.Size = new System.Drawing.Size(296, 23);
            this.batteryBar.TabIndex = 10;
            // 
            // updateBatteryButton
            // 
            this.updateBatteryButton.Location = new System.Drawing.Point(3, 81);
            this.updateBatteryButton.Name = "updateBatteryButton";
            this.updateBatteryButton.Size = new System.Drawing.Size(92, 23);
            this.updateBatteryButton.TabIndex = 11;
            this.updateBatteryButton.Text = "Update Battery";
            this.updateBatteryButton.UseVisualStyleBackColor = true;
            this.updateBatteryButton.Click += new System.EventHandler(this.updateBatteryButton_Click);
            // 
            // irBox
            // 
            this.irBox.BackColor = System.Drawing.Color.Black;
            this.irBox.Location = new System.Drawing.Point(3, 63);
            this.irBox.Name = "irBox";
            this.irBox.Size = new System.Drawing.Size(400, 115);
            this.irBox.TabIndex = 12;
            this.irBox.TabStop = false;
            this.irBox.Paint += new System.Windows.Forms.PaintEventHandler(this.irBox_Paint);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.irBox);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.extensionBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(473, 371);
            this.flowLayoutPanel1.TabIndex = 13;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.reportingmodeBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonsBox);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 54);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.accelerometersGroupBox);
            this.panel2.Controls.Add(this.led1Check);
            this.panel2.Controls.Add(this.updateBatteryButton);
            this.panel2.Controls.Add(this.led2Check);
            this.panel2.Controls.Add(this.batteryBar);
            this.panel2.Controls.Add(this.led3Check);
            this.panel2.Controls.Add(this.rumbleCheck);
            this.panel2.Controls.Add(this.led4Check);
            this.panel2.Location = new System.Drawing.Point(3, 184);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 111);
            this.panel2.TabIndex = 14;
            // 
            // extensionBox
            // 
            this.extensionBox.Location = new System.Drawing.Point(3, 301);
            this.extensionBox.Name = "extensionBox";
            this.extensionBox.Size = new System.Drawing.Size(400, 5);
            this.extensionBox.TabIndex = 16;
            this.extensionBox.TabStop = false;
            this.extensionBox.Text = "Extension";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wiimoteToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(473, 24);
            this.menuStrip.TabIndex = 14;
            this.menuStrip.Text = "menuStrip1";
            // 
            // wiimoteToolStripMenuItem
            // 
            this.wiimoteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calibrateToolStripMenuItem,
            this.dumpToolStripMenuItem,
            this.disconnectToolStripMenuItem});
            this.wiimoteToolStripMenuItem.Name = "wiimoteToolStripMenuItem";
            this.wiimoteToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.wiimoteToolStripMenuItem.Text = "Wiimote";
            // 
            // calibrateToolStripMenuItem
            // 
            this.calibrateToolStripMenuItem.Name = "calibrateToolStripMenuItem";
            this.calibrateToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.calibrateToolStripMenuItem.Text = "Calibrate";
            this.calibrateToolStripMenuItem.Click += new System.EventHandler(this.calibrateToolStripMenuItem_Click);
            // 
            // dumpToolStripMenuItem
            // 
            this.dumpToolStripMenuItem.Name = "dumpToolStripMenuItem";
            this.dumpToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.dumpToolStripMenuItem.Text = "Dump Memory";
            this.dumpToolStripMenuItem.Click += new System.EventHandler(this.dumpToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // WiimoteUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.Name = "WiimoteUserControl";
            this.Size = new System.Drawing.Size(473, 395);
            this.accelerometersGroupBox.ResumeLayout(false);
            this.accelerometersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox reportingmodeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox buttonsBox;
        private System.Windows.Forms.GroupBox accelerometersGroupBox;
        private System.Windows.Forms.TextBox accelerometerZBox;
        private System.Windows.Forms.TextBox accelerometerYBox;
        private System.Windows.Forms.TextBox accelerometerXBox;
        private System.Windows.Forms.CheckBox led2Check;
        private System.Windows.Forms.CheckBox led3Check;
        private System.Windows.Forms.CheckBox led4Check;
        private System.Windows.Forms.CheckBox rumbleCheck;
        private System.Windows.Forms.ProgressBar batteryBar;
        private System.Windows.Forms.Button updateBatteryButton;
        private System.Windows.Forms.CheckBox led1Check;
        private System.Windows.Forms.PictureBox irBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox extensionBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem wiimoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calibrateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
