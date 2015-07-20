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
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gbAutodiscover = new System.Windows.Forms.GroupBox();
            this.cbDiscoverICMPMulticast = new System.Windows.Forms.CheckBox();
            this.cbDiscoverARPSweeps = new System.Windows.Forms.CheckBox();
            this.cbDiscoverRouteAdvertisement = new System.Windows.Forms.CheckBox();
            this.gbAutodiscover.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.Location = new System.Drawing.Point(30, 272);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 35);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Image = global::evilfoca.Properties.Resources.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(291, 272);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 35);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // button1
            // 
            this.button1.Image = global::evilfoca.Properties.Resources.Interface;
            this.button1.Location = new System.Drawing.Point(30, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(336, 46);
            this.button1.TabIndex = 6;
            this.button1.Text = "Select interface";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbAutodiscover
            // 
            this.gbAutodiscover.Controls.Add(this.cbDiscoverICMPMulticast);
            this.gbAutodiscover.Controls.Add(this.cbDiscoverARPSweeps);
            this.gbAutodiscover.Controls.Add(this.cbDiscoverRouteAdvertisement);
            this.gbAutodiscover.Location = new System.Drawing.Point(30, 74);
            this.gbAutodiscover.Name = "gbAutodiscover";
            this.gbAutodiscover.Size = new System.Drawing.Size(336, 183);
            this.gbAutodiscover.TabIndex = 7;
            this.gbAutodiscover.TabStop = false;
            this.gbAutodiscover.Text = "Active computers discovery";
            // 
            // cbDiscoverICMPMulticast
            // 
            this.cbDiscoverICMPMulticast.Location = new System.Drawing.Point(19, 67);
            this.cbDiscoverICMPMulticast.Name = "cbDiscoverICMPMulticast";
            this.cbDiscoverICMPMulticast.Size = new System.Drawing.Size(311, 32);
            this.cbDiscoverICMPMulticast.TabIndex = 3;
            this.cbDiscoverICMPMulticast.Text = "Discover computers with IPv6 using ICMP multicast";
            this.cbDiscoverICMPMulticast.UseVisualStyleBackColor = true;
            // 
            // cbDiscoverARPSweeps
            // 
            this.cbDiscoverARPSweeps.Location = new System.Drawing.Point(19, 105);
            this.cbDiscoverARPSweeps.Name = "cbDiscoverARPSweeps";
            this.cbDiscoverARPSweeps.Size = new System.Drawing.Size(311, 32);
            this.cbDiscoverARPSweeps.TabIndex = 2;
            this.cbDiscoverARPSweeps.Text = "Discover computers with IPv4 using ARP sweeps";
            this.cbDiscoverARPSweeps.UseVisualStyleBackColor = true;
            // 
            // cbDiscoverRouteAdvertisement
            // 
            this.cbDiscoverRouteAdvertisement.Location = new System.Drawing.Point(19, 29);
            this.cbDiscoverRouteAdvertisement.Name = "cbDiscoverRouteAdvertisement";
            this.cbDiscoverRouteAdvertisement.Size = new System.Drawing.Size(311, 32);
            this.cbDiscoverRouteAdvertisement.TabIndex = 1;
            this.cbDiscoverRouteAdvertisement.Text = "Discover computers with IPv6 using router advertisement packets";
            this.cbDiscoverRouteAdvertisement.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 324);
            this.ControlBox = false;
            this.Controls.Add(this.gbAutodiscover);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.gbAutodiscover.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gbAutodiscover;
        private System.Windows.Forms.CheckBox cbDiscoverRouteAdvertisement;
        private System.Windows.Forms.CheckBox cbDiscoverICMPMulticast;
        private System.Windows.Forms.CheckBox cbDiscoverARPSweeps;
    }
}