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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;

namespace evilfoca
{
    public partial class FormAddNeighbor : Form
    {
        public IPAddress ip;
        public PhysicalAddress mac;

        public FormAddNeighbor()
        {
            ip = null;
            mac = null;

            InitializeComponent();
        }


        private void btMac_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress.Parse(tbIP.Text); // Fuerza una comprobación de una IP bien formada con el try catch

                tbMAC.Text = "";
                try
                {
                    tbMAC.Text = Utils.GetMacUsingARP(tbIP.Text);

                    tbMAC.BackColor = Color.White;
                    tbMAC.ForeColor = Color.Black;
                    tbIP.BackColor = Color.White;
                    tbIP.ForeColor = Color.Black;
                }
                catch
                {
                }
            }
            catch
            {
                FormMain.ShowMessage("Invalid IP Address", 1000, FormMessage.IconType.Error);
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            bool errorIP = false;
            bool errorMAC = false;

            tbMAC.BackColor = Color.White;
            tbMAC.ForeColor = Color.Black;
            tbIP.BackColor = Color.White;
            tbIP.ForeColor = Color.Black;

            try
            {
                this.ip = IPAddress.Parse(tbIP.Text);
            }
            catch
            {
                tbIP.BackColor = Color.Red;
                tbIP.ForeColor = Color.White;
                errorIP = true;
            }

            try
            {
                this.mac = PhysicalAddress.Parse(tbMAC.Text.ToUpper().Replace(':', '-'));
            }
            catch
            {

                tbMAC.BackColor = Color.Red;
                tbMAC.ForeColor = Color.White;
                errorMAC = true;
            }


            if ((errorMAC == false) && (errorIP == true))
            {
                FormMain.ShowMessage("Invalid IP address.", 1000, FormMessage.IconType.Error);
            }
            else if ((errorMAC == true) && (errorIP == false))
            {
                FormMain.ShowMessage("Invalid MAC address.", 1000, FormMessage.IconType.Error);
            }
            else if ((errorMAC == true) && (errorIP == true))
            {
                FormMain.ShowMessage("Invalid IP and MAC address.", 1000, FormMessage.IconType.Error);
            }
            else if ((errorMAC == false) && (errorIP == false))
                this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.mac = null;
            this.ip = null;

            this.Close();
        }

        private void FormAddNeighbor_Load(object sender, EventArgs e)
        {
        }
    }
}
