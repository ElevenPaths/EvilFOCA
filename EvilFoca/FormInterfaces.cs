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
using SharpPcap;
using SharpPcap.WinPcap;
using SharpPcap.LibPcap;

namespace evilfoca
{
    public partial class FormInterfaces : Form
    {
        public WinPcapDevice device;
        private bool IsSplashSetting;

        public FormInterfaces(bool exitButton)
            : this(exitButton, false)
        {
        }

        public FormInterfaces(bool exitButton, bool splashSetting)
        {
            InitializeComponent();
            this.IsSplashSetting = splashSetting;
            if (exitButton)
            {
                btAccept.Text = "&Continue";
                btExit.Text = "&Exit";
            }
            else
            {
                btAccept.Text = "&Accept";
                btExit.Text = "&Cancel";
            }
        }

        private void FormInterfaces_Load(object sender, EventArgs e)
        {
            LoadInterfaces();
            LoadSettigsInterface();
            this.TopMost = true;
        }

        private void LoadSettigsInterface()
        {
            int defaultInterface = Program.CurrentProject.data.settings.Interface;

            if (defaultInterface != -1)
            {
                if (defaultInterface + 1 <= dgvInterfaces.Rows.Count)
                    dgvInterfaces.Rows[defaultInterface].Selected = true;
            }
        }

        private void SaveSettingsInterface()
        {
            for (int i = 0; i < dgvInterfaces.Rows.Count; i++)
            {
                if (dgvInterfaces.Rows[i].Selected == true && i != Program.CurrentProject.data.settings.Interface)
                {
                    if (IsSplashSetting)
                    {
                        Program.CurrentProject.data.settings.Interface = i;
                        Program.CurrentProject.data.settings.Save();
                    }
                    else
                    {
                        if (MessageBox.Show("To apply new interface setting is necessary to restart application. Do you want to restart Evil Foca?", "Restart needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Program.CurrentProject.data.settings.Interface = i;
                            Program.CurrentProject.data.settings.Save();
                            Application.Restart();
                        }
                    }
                    return;
                }
            }
        }

        private void LoadInterfaces()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            foreach (var dev in devices)
            {
                if (dev is WinPcapDevice && (dev as WinPcapDevice).Addresses.Count > 0)
                {
                    try
                    {
                        string addresses = string.Empty;
                        //Primero mostramos las ipv4, despues las ipv6
                        foreach (PcapAddress address in ((WinPcapDevice)dev).Addresses.Where(A => A.Addr.ipAddress != null && A.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                        {
                            addresses += string.Format("[{0}] ", address.Addr.ipAddress.ToString());
                        }
                        foreach (PcapAddress address in ((WinPcapDevice)dev).Addresses.Where(A => A.Addr.ipAddress != null && A.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
                        {
                            addresses += string.Format("[{0}] ", address.Addr.ipAddress.ToString());
                        }
                        dgvInterfaces.Rows.Add(new object[] 
                                            { 
                                                ((WinPcapDevice)dev).Interface.FriendlyName,
                                                addresses
                                            });
                        dgvInterfaces.Rows[dgvInterfaces.Rows.GetLastRow(DataGridViewElementStates.None)].Tag = dev;
                    }
                    catch { }
                }
            }
        }

        private void btAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.device = (WinPcapDevice)dgvInterfaces.SelectedRows[0].Tag;
            this.Close();
            SaveSettingsInterface();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.device = null;
            this.Close();
        }
    }
}
