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
using evilfoca.ControlHelp;

namespace evilfoca.ControlHelp
{
    public partial class ControlHelp : UserControl
    {
        int initialWidth = 0;

        public ControlHelp()
        {
            InitializeComponent();
        }

        private void ControlHelp_Load(object sender, EventArgs e)
        {
            if (!Program.runtime)
                return;

            initialWidth = Program.formMain.splitContainerHelp.SplitterDistance;

            Default.PanelHelpDefault defaultPanel = new Default.PanelHelpDefault();
            AddControl(defaultPanel);
        }

        private void cbHide_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHide.Checked)
            {
                Program.formMain.splitContainerHelp.SplitterDistance = Program.formMain.splitContainerHelp.Size.Width + 660;
                cbHide.Text = "<\r\n\r\n<\r\n\r\n<\r\n\r\n<";
            }
            else
            {
                Program.formMain.splitContainerHelp.SplitterDistance = initialWidth;
                cbHide.Text = ">\r\n\r\n>\r\n\r\n>\r\n\r\n>";
            }
        }

        public void ClearControls()
        {
            for (int i = Displayer.Controls.Count - 1; i >= 0; i--)
            {
                Control c = Displayer.Controls[i];
                if (!(c is CheckBox))
                    Displayer.Controls.Remove(c);
            }
        }

        public void AddControl(Control c)
        {
            ClearControls();
            Displayer.Controls.Add(c);
            c.Dock = DockStyle.Fill;
        }
    }
}
