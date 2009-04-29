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
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.devicesBox = new System.Windows.Forms.ListBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.autoconnectBox = new System.Windows.Forms.CheckBox();
            this.wiidevicePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // devicesBox
            // 
            this.devicesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesBox.FormattingEnabled = true;
            this.devicesBox.Location = new System.Drawing.Point(0, 0);
            this.devicesBox.Name = "devicesBox";
            this.devicesBox.Size = new System.Drawing.Size(172, 472);
            this.devicesBox.TabIndex = 0;
            this.devicesBox.SelectedValueChanged += new System.EventHandler(this.devicesBox_SelectedValueChanged);
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(0, 517);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(172, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(8, 8);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.autoconnectBox);
            this.splitContainer1.Panel1.Controls.Add(this.connectButton);
            this.splitContainer1.Panel1.Controls.Add(this.devicesBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.wiidevicePanel);
            this.splitContainer1.Size = new System.Drawing.Size(729, 540);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 2;
            // 
            // autoconnectBox
            // 
            this.autoconnectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.autoconnectBox.Location = new System.Drawing.Point(0, 487);
            this.autoconnectBox.Name = "autoconnectBox";
            this.autoconnectBox.Size = new System.Drawing.Size(172, 24);
            this.autoconnectBox.TabIndex = 2;
            this.autoconnectBox.Text = "Auto connect";
            this.autoconnectBox.UseVisualStyleBackColor = true;
            // 
            // wiidevicePanel
            // 
            this.wiidevicePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wiidevicePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.wiidevicePanel.Location = new System.Drawing.Point(0, 0);
            this.wiidevicePanel.Name = "wiidevicePanel";
            this.wiidevicePanel.Padding = new System.Windows.Forms.Padding(3);
            this.wiidevicePanel.Size = new System.Drawing.Size(553, 540);
            this.wiidevicePanel.TabIndex = 0;
            this.wiidevicePanel.WrapContents = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 556);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "Devices Form";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox devicesBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox autoconnectBox;
        private System.Windows.Forms.FlowLayoutPanel wiidevicePanel;
    }
}

