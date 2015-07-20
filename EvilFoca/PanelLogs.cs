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

namespace evilfoca
{
    public partial class PanelLogs : UserControl
    {
        internal SynchronizedCollection<Logs.Log> logs;
        //
        public PanelLogs()
        {
            InitializeComponent();
            logs = new SynchronizedCollection<Logs.Log>();
            listViewExLogs.FullRowSelect = true;
        }

        public void AddLog(Logs.Log log)
        {
            logs.Add(log);

            listViewExLogs.Invoke(new MethodInvoker(delegate
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = log;
                lvi.Text = log.datetime.ToShortTimeString();
                lvi.SubItems.Add(log.logType.ToString());
                lvi.SubItems.Add(log.message);

                listViewExLogs.Items.Add(lvi);
                lvi.EnsureVisible();
            }));
        }
    }
}
