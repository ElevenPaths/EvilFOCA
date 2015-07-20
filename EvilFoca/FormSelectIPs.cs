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
using System.Net;
using evilfoca.Data;

namespace evilfoca
{
    public partial class FormSelectIPs : Form
    {
        PanelTargets.TypeList type;
        IList<Neighbor> lstNeighbors;
        SynchronizedCollection<IPAddress> lstSelectedIps;
        SynchronizedCollection<IPAddress> lstIpsFiltered;
        SynchronizedCollection<IPAddress> lstPrevIpsFiltered;
        bool multiSelect;

        public FormSelectIPs(PanelTargets.TypeList type, IList<Neighbor> lstNeighbors, bool multiSelect, SynchronizedCollection<IPAddress> lstIpsFiltered, SynchronizedCollection<IPAddress> lstPrevIpsFiltered)
        {
            this.lstPrevIpsFiltered = lstPrevIpsFiltered;
            this.lstIpsFiltered = lstIpsFiltered;
            this.multiSelect = multiSelect;
            this.lstNeighbors = lstNeighbors;
            lstSelectedIps = new SynchronizedCollection<IPAddress>();
            this.type = type;
            InitializeComponent();
        }

        private void FormSelectIPs_Load(object sender, EventArgs e)
        {
            listViewNeighbors.FullRowSelect = true;
            listViewNeighbors.MultiSelect = multiSelect;

            foreach (Neighbor neighbor in lstNeighbors)
            {
                foreach (IPAddress ip in neighbor.GetIPs())
                {
                    if ((lstIpsFiltered != null) && (lstIpsFiltered.Contains(ip)))
                        continue;

                    ListViewItem lvi = new ListViewItem(ip.ToString());
                    lvi.Tag = ip;

                    if (lstPrevIpsFiltered != null)
                    {
                        if (lstPrevIpsFiltered.Where(I => I.Equals(ip)).Count() > 0)
                            lvi.Selected = true;
                    }

                    if ((type == PanelTargets.TypeList.Ipv4) && (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        lvi.SubItems.Add("IPv4");
                        listViewNeighbors.Items.Add(lvi);
                    }
                    else if ((type == PanelTargets.TypeList.Ipv6) && (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
                    {
                        lvi.SubItems.Add("IPv6");
                        listViewNeighbors.Items.Add(lvi);
                    }
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            lstSelectedIps.Clear();
            this.Close();
        }

        public SynchronizedCollection<IPAddress> GetSelectedIPs()
        {
            return lstSelectedIps;
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listViewNeighbors.SelectedItems)
            {
                IPAddress ip = (IPAddress)lvi.Tag;
                lstSelectedIps.Add(ip);
            }

            this.Close();
        }

    }
}
