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
using System.Threading;

namespace evilfoca
{
    public partial class FormMessage : Form
    {
        public enum IconType
        {
            OK = 0,
            Error = 1,
            Info = 2
        }

        int speed = 10;
        private string message = string.Empty;
        double dec = 0.002;
        int retardoMS = 1;

        public IconType iconType = IconType.OK;


        public FormMessage(string message, int retardoMS, IconType iconType)
        {
            if (retardoMS == 0)
                retardoMS = 1;
            this.retardoMS = retardoMS;

            this.iconType = iconType;
            this.message = message;
            InitializeComponent();
        }


        private void FormMessage_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            timerRetardo.Interval = retardoMS;
            timerClose.Interval = speed;
            this.lbMessage.Text = message;

            pcIcon.Image = imageList1.Images[(int)iconType];
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            this.Opacity -= dec;

            if (this.Opacity < 0.90)
            {
                this.dec = 0.10;
            }

            if (this.Opacity < 0.20)
            {
                this.Close();
            }
        }

        private void timerRetardo_Tick(object sender, EventArgs e)
        {
            timerClose.Enabled = true;
            timerRetardo.Enabled = false;
        }


    }
}
