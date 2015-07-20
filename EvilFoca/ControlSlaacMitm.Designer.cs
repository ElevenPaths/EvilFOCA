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
    partial class ControlSlaacMitm
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
            this.lbTarget1 = new System.Windows.Forms.Label();
            this.lbTarget2 = new System.Windows.Forms.Label();
            this.lbRoute = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Target 2:";
            // 
            // lbTarget1
            // 
            this.lbTarget1.AutoSize = true;
            this.lbTarget1.Location = new System.Drawing.Point(59, 3);
            this.lbTarget1.Name = "lbTarget1";
            this.lbTarget1.Size = new System.Drawing.Size(22, 13);
            this.lbTarget1.TabIndex = 2;
            this.lbTarget1.Text = "     ";
            // 
            // lbTarget2
            // 
            this.lbTarget2.AutoSize = true;
            this.lbTarget2.Location = new System.Drawing.Point(59, 20);
            this.lbTarget2.Name = "lbTarget2";
            this.lbTarget2.Size = new System.Drawing.Size(22, 13);
            this.lbTarget2.TabIndex = 3;
            this.lbTarget2.Text = "     ";
            // 
            // lbRoute
            // 
            this.lbRoute.AutoSize = true;
            this.lbRoute.Location = new System.Drawing.Point(239, 3);
            this.lbRoute.Name = "lbRoute";
            this.lbRoute.Size = new System.Drawing.Size(68, 13);
            this.lbRoute.TabIndex = 4;
            this.lbRoute.Text = "Route: None";
            // 
            // controlMitmAttack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbRoute);
            this.Controls.Add(this.lbTarget2);
            this.Controls.Add(this.lbTarget1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "controlMitmAttack";
            this.Size = new System.Drawing.Size(310, 40);
            this.Load += new System.EventHandler(this.controlMitmAttack_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTarget1;
        private System.Windows.Forms.Label lbTarget2;
        private System.Windows.Forms.Label lbRoute;
    }
}
