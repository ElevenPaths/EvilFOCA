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
    partial class PanelLogs
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
            this.listViewExLogs = new evilfoca.Controls.ListViewEx.ListViewEx();
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Module = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewExLogs
            // 
            this.listViewExLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewExLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Time,
            this.Module,
            this.Message});
            this.listViewExLogs.GridLines = true;
            this.listViewExLogs.Location = new System.Drawing.Point(3, 3);
            this.listViewExLogs.MultiSelect = false;
            this.listViewExLogs.Name = "listViewExLogs";
            this.listViewExLogs.Size = new System.Drawing.Size(700, 175);
            this.listViewExLogs.TabIndex = 3;
            this.listViewExLogs.UseCompatibleStateImageBehavior = false;
            this.listViewExLogs.View = System.Windows.Forms.View.Details;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 80;
            // 
            // Module
            // 
            this.Module.Text = "Module";
            this.Module.Width = 140;
            // 
            // Message
            // 
            this.Message.Text = "Message";
            this.Message.Width = 600;
            // 
            // PanelLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewExLogs);
            this.Name = "PanelLogs";
            this.Size = new System.Drawing.Size(618, 181);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Module;
        private System.Windows.Forms.ColumnHeader Message;
        public Controls.ListViewEx.ListViewEx listViewExLogs;
    }
}
