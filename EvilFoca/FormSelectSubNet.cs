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
using System;
using System.Windows.Forms;
using System.Net;

namespace evilfoca
{
    public partial class FormSelectSubNet : Form
    {
        private IPAddress _from;
        private IPAddress _to;

        public FormSelectSubNet()
        {
            InitializeComponent();
            this._from = null;
            this._to = null;
        }

        public IPAddress FromIP
        {
            get { return _from; }
        }
        public IPAddress ToIP
        {
            get { return _to; }
        }

        public bool IsMySubnetSelected
        {
            get { return rdbSubnet.Checked; }
        }


        private void btOk_Click(object sender, EventArgs e)
        {
            if (rdbCustom.Checked)
            {
                if (txtTo.IsValid && txtFrom.IsValid)
                {
                    this._from = txtFrom.IpValue;
                    this._to = txtTo.IpValue;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                    FormMain.ShowMessage("Invalid IP addresses", 1000, FormMessage.IconType.Info);
            }
            else
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void rdbCustom_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                txtFrom.Enabled = true;
                txtTo.Enabled = true;
            }
            else
            {
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
            }
        }

    }
}
