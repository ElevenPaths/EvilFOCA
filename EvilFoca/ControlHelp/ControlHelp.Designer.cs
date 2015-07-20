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
namespace evilfoca.ControlHelp
{
    partial class ControlHelp
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
            this.Displayer = new System.Windows.Forms.Panel();
            this.cbHide = new System.Windows.Forms.CheckBox();
            this.Displayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // Displayer
            // 
            this.Displayer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Displayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Displayer.Controls.Add(this.cbHide);
            this.Displayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Displayer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Displayer.Location = new System.Drawing.Point(0, 0);
            this.Displayer.Margin = new System.Windows.Forms.Padding(0);
            this.Displayer.Name = "Displayer";
            this.Displayer.Size = new System.Drawing.Size(242, 268);
            this.Displayer.TabIndex = 0;
            // 
            // cbHide
            // 
            this.cbHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cbHide.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbHide.Font = new System.Drawing.Font("Perpetua", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHide.Location = new System.Drawing.Point(-1, -1);
            this.cbHide.Margin = new System.Windows.Forms.Padding(0);
            this.cbHide.Name = "cbHide";
            this.cbHide.Size = new System.Drawing.Size(20, 269);
            this.cbHide.TabIndex = 0;
            this.cbHide.Text = ">\r\n\r\n\r\n>\r\n\r\n\r\n>\r\n\r\n\r\n>";
            this.cbHide.UseVisualStyleBackColor = true;
            this.cbHide.Visible = false;
            this.cbHide.CheckedChanged += new System.EventHandler(this.cbHide_CheckedChanged);
            // 
            // ControlHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Displayer);
            this.Name = "ControlHelp";
            this.Size = new System.Drawing.Size(242, 268);
            this.Load += new System.EventHandler(this.ControlHelp_Load);
            this.Displayer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Displayer;
        private System.Windows.Forms.CheckBox cbHide;

    }
}
