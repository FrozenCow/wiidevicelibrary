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
    partial class BalanceBoardUserControl
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
            this.ledCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.topLeftWeightBox = new System.Windows.Forms.TextBox();
            this.topRightWeightBox = new System.Windows.Forms.TextBox();
            this.bottomLeftWeightBox = new System.Windows.Forms.TextBox();
            this.bottomRightWeightBox = new System.Windows.Forms.TextBox();
            this.totalWeightBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.averageWeightBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.balanceBox = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.balanceBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ledCheck
            // 
            this.ledCheck.AutoSize = true;
            this.ledCheck.Location = new System.Drawing.Point(4, 5);
            this.ledCheck.Name = "ledCheck";
            this.ledCheck.Size = new System.Drawing.Size(44, 17);
            this.ledCheck.TabIndex = 0;
            this.ledCheck.Text = "Led";
            this.ledCheck.UseVisualStyleBackColor = true;
            this.ledCheck.CheckedChanged += new System.EventHandler(this.ledCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Button";
            // 
            // buttonBox
            // 
            this.buttonBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBox.Location = new System.Drawing.Point(161, 3);
            this.buttonBox.Name = "buttonBox";
            this.buttonBox.ReadOnly = true;
            this.buttonBox.Size = new System.Drawing.Size(115, 20);
            this.buttonBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.averageWeightBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.bottomRightWeightBox);
            this.groupBox1.Controls.Add(this.bottomLeftWeightBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.topRightWeightBox);
            this.groupBox1.Controls.Add(this.totalWeightBox);
            this.groupBox1.Controls.Add(this.topLeftWeightBox);
            this.groupBox1.Location = new System.Drawing.Point(4, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 128);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weight Sensors";
            // 
            // topLeftWeightBox
            // 
            this.topLeftWeightBox.Location = new System.Drawing.Point(7, 20);
            this.topLeftWeightBox.Name = "topLeftWeightBox";
            this.topLeftWeightBox.ReadOnly = true;
            this.topLeftWeightBox.Size = new System.Drawing.Size(100, 20);
            this.topLeftWeightBox.TabIndex = 0;
            // 
            // topRightWeightBox
            // 
            this.topRightWeightBox.Location = new System.Drawing.Point(114, 20);
            this.topRightWeightBox.Name = "topRightWeightBox";
            this.topRightWeightBox.ReadOnly = true;
            this.topRightWeightBox.Size = new System.Drawing.Size(100, 20);
            this.topRightWeightBox.TabIndex = 1;
            // 
            // bottomLeftWeightBox
            // 
            this.bottomLeftWeightBox.Location = new System.Drawing.Point(7, 47);
            this.bottomLeftWeightBox.Name = "bottomLeftWeightBox";
            this.bottomLeftWeightBox.ReadOnly = true;
            this.bottomLeftWeightBox.Size = new System.Drawing.Size(100, 20);
            this.bottomLeftWeightBox.TabIndex = 2;
            // 
            // bottomRightWeightBox
            // 
            this.bottomRightWeightBox.Location = new System.Drawing.Point(114, 47);
            this.bottomRightWeightBox.Name = "bottomRightWeightBox";
            this.bottomRightWeightBox.ReadOnly = true;
            this.bottomRightWeightBox.Size = new System.Drawing.Size(100, 20);
            this.bottomRightWeightBox.TabIndex = 3;
            // 
            // totalWeightBox
            // 
            this.totalWeightBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.totalWeightBox.Location = new System.Drawing.Point(80, 73);
            this.totalWeightBox.Name = "totalWeightBox";
            this.totalWeightBox.ReadOnly = true;
            this.totalWeightBox.Size = new System.Drawing.Size(186, 20);
            this.totalWeightBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Total Weight";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Average";
            // 
            // averageWeightBox
            // 
            this.averageWeightBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.averageWeightBox.Location = new System.Drawing.Point(80, 99);
            this.averageWeightBox.Name = "averageWeightBox";
            this.averageWeightBox.ReadOnly = true;
            this.averageWeightBox.Size = new System.Drawing.Size(186, 20);
            this.averageWeightBox.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.balanceBox);
            this.groupBox2.Location = new System.Drawing.Point(0, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 211);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Balance";
            // 
            // balanceBox
            // 
            this.balanceBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.balanceBox.BackColor = System.Drawing.Color.Black;
            this.balanceBox.Location = new System.Drawing.Point(7, 20);
            this.balanceBox.Name = "balanceBox";
            this.balanceBox.Size = new System.Drawing.Size(259, 185);
            this.balanceBox.TabIndex = 0;
            this.balanceBox.TabStop = false;
            this.balanceBox.Paint += new System.Windows.Forms.PaintEventHandler(this.balanceBox_Paint);
            // 
            // BalanceBoardUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ledCheck);
            this.Name = "BalanceBoardUserControl";
            this.Size = new System.Drawing.Size(279, 380);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.balanceBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ledCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox buttonBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox bottomRightWeightBox;
        private System.Windows.Forms.TextBox bottomLeftWeightBox;
        private System.Windows.Forms.TextBox topRightWeightBox;
        private System.Windows.Forms.TextBox topLeftWeightBox;
        private System.Windows.Forms.TextBox totalWeightBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox averageWeightBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox balanceBox;
    }
}
