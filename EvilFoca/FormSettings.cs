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

namespace evilfoca
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormInterfaces fInterfaces = new FormInterfaces(false);
            fInterfaces.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveConf();
            Close();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            LoadConf();
        }

        private void LoadConf()
        {
            cbDiscoverRouteAdvertisement.Checked = Program.CurrentProject.data.settings.DiscoverWithRouterAdvertisement;
            cbDiscoverARPSweeps.Checked = Program.CurrentProject.data.settings.DiscoverWithArpScan;
            cbDiscoverICMPMulticast.Checked = Program.CurrentProject.data.settings.DiscoverWithPingMulticastIpv6;
        }

        private void SaveConf()
        {
            Program.CurrentProject.data.settings.DiscoverWithRouterAdvertisement = cbDiscoverRouteAdvertisement.Checked;
            Program.CurrentProject.data.settings.DiscoverWithPingMulticastIpv6 = cbDiscoverICMPMulticast.Checked;
            Program.CurrentProject.data.settings.DiscoverWithArpScan = cbDiscoverARPSweeps.Checked;

            Program.CurrentProject.data.settings.Save();
        }
    }
}
