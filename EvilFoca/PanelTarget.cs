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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace evilfoca
{
    public partial class PanelTarget : UserControl
    {
        public PanelTargets.TypeList typeList = PanelTargets.TypeList.Ipv6;

        public PanelTarget()
        {
            InitializeComponent();
        }

        private void imgClearTargets_Click(object sender, EventArgs e)
        {
            listBoxTargets.Items.Clear();
        }

        private void imgAddTargets_Click(object sender, EventArgs e)
        {
            SynchronizedCollection<IPAddress> ipsFiltered = new SynchronizedCollection<IPAddress>();
            foreach (IPAddress ip in listBoxTargets.Items)
            {
                ipsFiltered.Add(ip);
            }

            SynchronizedCollection<IPAddress> ipsPrevFiltered = new SynchronizedCollection<IPAddress>();
            foreach (IPAddress ip in listBoxTargets.Items)
            {
                ipsPrevFiltered.Add(ip);
            }

            FormSelectIPs fSel = new FormSelectIPs(typeList, Data.Data.neighbors, true, null, ipsPrevFiltered);
            fSel.ShowDialog();

            SynchronizedCollection<IPAddress> lstIps = fSel.GetSelectedIPs();

            if (lstIps.Count > 0)
                listBoxTargets.Items.Clear();

            foreach (IPAddress ip in lstIps)
            {
                listBoxTargets.Items.Add(ip);
            }
        }

        private void listBoxTargets_DragDrop(object sender, DragEventArgs e)
        {
            if (typeList == PanelTargets.TypeList.Ipv6)
            {
                if (e.Data.GetDataPresent("System.Net.IPAddress", false))
                {
                    IPAddress ip = (IPAddress)e.Data.GetData("System.Net.IPAddress", false);

                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        FormMain.ShowMessage("You can't use an IPv4 in this attack", 1000, FormMessage.IconType.Error);
                    else
                    {
                        listBoxTargets.Items.Add(ip);
                    }
                }
            }
            else if (typeList == PanelTargets.TypeList.Ipv4)
            {
                if (e.Data.GetDataPresent("System.Net.IPAddress", false))
                {
                    IPAddress ip = (IPAddress)e.Data.GetData("System.Net.IPAddress", false);

                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        FormMain.ShowMessage("You can't use an IPv6 in this attack", 1000, FormMessage.IconType.Error);
                    else
                    {
                        listBoxTargets.Items.Add(ip);
                    }
                }
            }
        }

        private void listBoxTargets_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        public List<IPAddress> GetTargetsList()
        {
            List<IPAddress> lstIps = new List<IPAddress>();

            for (int i = 0; i < listBoxTargets.Items.Count; i++)
                lstIps.Add(listBoxTargets.Items[i] as IPAddress);
            return lstIps;
        }

        public void ClearTargets()
        {
            listBoxTargets.Items.Clear();
        }
    }
}
