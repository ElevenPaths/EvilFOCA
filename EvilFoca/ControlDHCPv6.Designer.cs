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
    partial class ControlDHCPv6
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDns = new System.Windows.Forms.Label();
            this.lblIpRange = new System.Windows.Forms.Label();
            this.lblIpRangeValue = new System.Windows.Forms.Label();
            this.lblDnsValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDns
            // 
            this.lblDns.AutoSize = true;
            this.lblDns.Location = new System.Drawing.Point(3, 3);
            this.lblDns.Name = "lblDns";
            this.lblDns.Size = new System.Drawing.Size(33, 13);
            this.lblDns.TabIndex = 0;
            this.lblDns.Text = "DNS:";
            // 
            // lblIpRange
            // 
            this.lblIpRange.AutoSize = true;
            this.lblIpRange.Location = new System.Drawing.Point(3, 22);
            this.lblIpRange.Name = "lblIpRange";
            this.lblIpRange.Size = new System.Drawing.Size(55, 13);
            this.lblIpRange.TabIndex = 1;
            this.lblIpRange.Text = "IP Range:";
            // 
            // lblIpRangeValue
            // 
            this.lblIpRangeValue.AutoSize = true;
            this.lblIpRangeValue.Location = new System.Drawing.Point(64, 22);
            this.lblIpRangeValue.Name = "lblIpRangeValue";
            this.lblIpRangeValue.Size = new System.Drawing.Size(0, 13);
            this.lblIpRangeValue.TabIndex = 2;
            // 
            // lblDnsValue
            // 
            this.lblDnsValue.AutoSize = true;
            this.lblDnsValue.Location = new System.Drawing.Point(64, 3);
            this.lblDnsValue.Name = "lblDnsValue";
            this.lblDnsValue.Size = new System.Drawing.Size(0, 13);
            this.lblDnsValue.TabIndex = 3;
            // 
            // controlDHCPv6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDnsValue);
            this.Controls.Add(this.lblIpRangeValue);
            this.Controls.Add(this.lblIpRange);
            this.Controls.Add(this.lblDns);
            this.Name = "controlDHCPv6";
            this.Size = new System.Drawing.Size(310, 40);
            this.Load += new System.EventHandler(this.controlDHCPv6_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDns;
        private System.Windows.Forms.Label lblIpRange;
        private System.Windows.Forms.Label lblIpRangeValue;
        private System.Windows.Forms.Label lblDnsValue;
    }
}
