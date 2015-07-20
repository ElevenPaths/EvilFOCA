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
    partial class FormSelectIPs
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
            this.listViewNeighbors = new evilfoca.Controls.ListViewEx.ListViewEx();
            this.ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btSelect = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.titulo = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.titulo.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewNeighbors
            // 
            this.listViewNeighbors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ip,
            this.version});
            this.listViewNeighbors.GridLines = true;
            this.listViewNeighbors.Location = new System.Drawing.Point(0, 21);
            this.listViewNeighbors.Name = "listViewNeighbors";
            this.listViewNeighbors.Size = new System.Drawing.Size(312, 271);
            this.listViewNeighbors.TabIndex = 3;
            this.listViewNeighbors.UseCompatibleStateImageBehavior = false;
            this.listViewNeighbors.View = System.Windows.Forms.View.Details;
            // 
            // ip
            // 
            this.ip.Text = "IP";
            this.ip.Width = 220;
            // 
            // version
            // 
            this.version.Text = "Version";
            this.version.Width = 71;
            // 
            // btSelect
            // 
            this.btSelect.Image = global::evilfoca.Properties.Resources.Ok;
            this.btSelect.Location = new System.Drawing.Point(51, 297);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(98, 35);
            this.btSelect.TabIndex = 4;
            this.btSelect.Text = "&Accept";
            this.btSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // btCancel
            // 
            this.btCancel.Image = global::evilfoca.Properties.Resources.Cancel;
            this.btCancel.Location = new System.Drawing.Point(155, 297);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(98, 35);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "&Cancel";
            this.btCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // titulo
            // 
            this.titulo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.titulo.Controls.Add(this.label3);
            this.titulo.Location = new System.Drawing.Point(0, 0);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(312, 21);
            this.titulo.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(286, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Select IP(s)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSelectIPs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 338);
            this.ControlBox = false;
            this.Controls.Add(this.titulo);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.listViewNeighbors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormSelectIPs";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FormSelectIPs_Load);
            this.titulo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListViewEx.ListViewEx listViewNeighbors;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ColumnHeader ip;
        private System.Windows.Forms.ColumnHeader version;
        private System.Windows.Forms.Panel titulo;
        private System.Windows.Forms.Label label3;

    }
}