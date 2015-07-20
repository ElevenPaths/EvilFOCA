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
    partial class FormInterfaces
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInterfaces));
            this.dgvInterfaces = new System.Windows.Forms.DataGridView();
            this.Interface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IPAddresses = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btExit = new System.Windows.Forms.Button();
            this.btAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInterfaces)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInterfaces
            // 
            this.dgvInterfaces.AllowUserToAddRows = false;
            this.dgvInterfaces.AllowUserToDeleteRows = false;
            this.dgvInterfaces.AllowUserToResizeRows = false;
            this.dgvInterfaces.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvInterfaces.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvInterfaces.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInterfaces.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Interface,
            this.IPAddresses});
            this.dgvInterfaces.Location = new System.Drawing.Point(0, 0);
            this.dgvInterfaces.MultiSelect = false;
            this.dgvInterfaces.Name = "dgvInterfaces";
            this.dgvInterfaces.ReadOnly = true;
            this.dgvInterfaces.RowHeadersVisible = false;
            this.dgvInterfaces.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInterfaces.Size = new System.Drawing.Size(537, 194);
            this.dgvInterfaces.TabIndex = 0;
            // 
            // Interface
            // 
            this.Interface.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Interface.HeaderText = "Interface";
            this.Interface.Name = "Interface";
            this.Interface.ReadOnly = true;
            this.Interface.Width = 250;
            // 
            // IPAddresses
            // 
            this.IPAddresses.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IPAddresses.HeaderText = "IP Addresses";
            this.IPAddresses.Name = "IPAddresses";
            this.IPAddresses.ReadOnly = true;
            // 
            // btExit
            // 
            this.btExit.Image = global::evilfoca.Properties.Resources.Cancel;
            this.btExit.Location = new System.Drawing.Point(277, 198);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(107, 42);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "&Exit";
            this.btExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btAccept
            // 
            this.btAccept.Image = global::evilfoca.Properties.Resources.Ok;
            this.btAccept.Location = new System.Drawing.Point(152, 198);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(107, 42);
            this.btAccept.TabIndex = 1;
            this.btAccept.Text = "&Continue";
            this.btAccept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // FormInterfaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 249);
            this.ControlBox = false;
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.dgvInterfaces);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormInterfaces";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select an interface";
            this.Load += new System.EventHandler(this.FormInterfaces_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInterfaces)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInterfaces;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interface;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPAddresses;
    }
}