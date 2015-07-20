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
    partial class PanelTarget
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
            this.splitContainerAttacksNeigborAdvSpoof = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.imgClearTargets = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imgAddTargets = new System.Windows.Forms.PictureBox();
            this.listBoxTargets = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAttacksNeigborAdvSpoof)).BeginInit();
            this.splitContainerAttacksNeigborAdvSpoof.Panel2.SuspendLayout();
            this.splitContainerAttacksNeigborAdvSpoof.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgClearTargets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgAddTargets)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerAttacksNeigborAdvSpoof
            // 
            this.splitContainerAttacksNeigborAdvSpoof.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerAttacksNeigborAdvSpoof.Location = new System.Drawing.Point(2, 3);
            this.splitContainerAttacksNeigborAdvSpoof.Name = "splitContainerAttacksNeigborAdvSpoof";
            this.splitContainerAttacksNeigborAdvSpoof.Panel1Collapsed = true;
            // 
            // splitContainerAttacksNeigborAdvSpoof.Panel2
            // 
            this.splitContainerAttacksNeigborAdvSpoof.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainerAttacksNeigborAdvSpoof.Size = new System.Drawing.Size(606, 158);
            this.splitContainerAttacksNeigborAdvSpoof.SplitterDistance = 296;
            this.splitContainerAttacksNeigborAdvSpoof.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.imgClearTargets);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.imgAddTargets);
            this.splitContainer2.Panel1MinSize = 18;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBoxTargets);
            this.splitContainer2.Size = new System.Drawing.Size(606, 158);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // imgClearTargets
            // 
            this.imgClearTargets.Dock = System.Windows.Forms.DockStyle.Right;
            this.imgClearTargets.Image = global::evilfoca.Properties.Resources.Cancel;
            this.imgClearTargets.Location = new System.Drawing.Point(570, 0);
            this.imgClearTargets.Name = "imgClearTargets";
            this.imgClearTargets.Size = new System.Drawing.Size(18, 25);
            this.imgClearTargets.TabIndex = 11;
            this.imgClearTargets.TabStop = false;
            this.imgClearTargets.Click += new System.EventHandler(this.imgClearTargets_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Targets";
            // 
            // imgAddTargets
            // 
            this.imgAddTargets.Dock = System.Windows.Forms.DockStyle.Right;
            this.imgAddTargets.Image = global::evilfoca.Properties.Resources.Add;
            this.imgAddTargets.Location = new System.Drawing.Point(588, 0);
            this.imgAddTargets.Name = "imgAddTargets";
            this.imgAddTargets.Size = new System.Drawing.Size(18, 25);
            this.imgAddTargets.TabIndex = 10;
            this.imgAddTargets.TabStop = false;
            this.imgAddTargets.Click += new System.EventHandler(this.imgAddTargets_Click);
            // 
            // listBoxTargets
            // 
            this.listBoxTargets.AllowDrop = true;
            this.listBoxTargets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxTargets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTargets.FormattingEnabled = true;
            this.listBoxTargets.Location = new System.Drawing.Point(0, 0);
            this.listBoxTargets.Name = "listBoxTargets";
            this.listBoxTargets.Size = new System.Drawing.Size(606, 129);
            this.listBoxTargets.TabIndex = 4;
            this.listBoxTargets.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxTargets_DragDrop);
            this.listBoxTargets.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxTargets_DragEnter);
            // 
            // PanelTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerAttacksNeigborAdvSpoof);
            this.Name = "PanelTarget";
            this.Size = new System.Drawing.Size(611, 164);
            this.splitContainerAttacksNeigborAdvSpoof.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAttacksNeigborAdvSpoof)).EndInit();
            this.splitContainerAttacksNeigborAdvSpoof.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgClearTargets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgAddTargets)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerAttacksNeigborAdvSpoof;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox imgClearTargets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox imgAddTargets;
        private System.Windows.Forms.ListBox listBoxTargets;

    }
}
