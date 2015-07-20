/* 
Evil FOCA
Copyright (C) 2015 ElevenPaths

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace evilfoca
{
    partial class ControlDNSHijacking
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
            this.label2 = new System.Windows.Forms.Label();
            this.lbDomain = new System.Windows.Forms.Label();
            this.lbIp = new System.Windows.Forms.Label();
            this.lbSpoofs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Domain:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Resolve as:";
            // 
            // lbDomain
            // 
            this.lbDomain.AutoSize = true;
            this.lbDomain.Location = new System.Drawing.Point(55, 3);
            this.lbDomain.Name = "lbDomain";
            this.lbDomain.Size = new System.Drawing.Size(25, 13);
            this.lbDomain.TabIndex = 2;
            this.lbDomain.Text = "      ";
            // 
            // lbIp
            // 
            this.lbIp.AutoSize = true;
            this.lbIp.Location = new System.Drawing.Point(72, 20);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(31, 13);
            this.lbIp.TabIndex = 3;
            this.lbIp.Text = "        ";
            // 
            // lbSpoofs
            // 
            this.lbSpoofs.Location = new System.Drawing.Point(210, 0);
            this.lbSpoofs.Name = "lbSpoofs";
            this.lbSpoofs.Size = new System.Drawing.Size(100, 23);
            this.lbSpoofs.TabIndex = 4;
            this.lbSpoofs.Text = "Spoofs: 0";
            this.lbSpoofs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // controlDNSHijacking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSpoofs);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.lbDomain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "controlDNSHijacking";
            this.Size = new System.Drawing.Size(310, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbDomain;
        private System.Windows.Forms.Label lbIp;
        private System.Windows.Forms.Label lbSpoofs;
    }
}
