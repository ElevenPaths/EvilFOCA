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
using evilfoca.Data;

namespace evilfoca
{
    public partial class ControlMitmAttack : UserControl
    {
        Attack attack;

        public ControlMitmAttack(Attack attack)
        {
            this.attack = attack;
            InitializeComponent();
        }

        public void UpdateData()
        {
            MitmAttack mitmAttack = (MitmAttack)attack;

            lbTarget1.Text = mitmAttack.t1.ip.ToString() + " (" + mitmAttack.t1.packetsSend.ToString() + ")";
            lbTarget2.Text = mitmAttack.t2.ip.ToString() + " (" + mitmAttack.t2.packetsSend.ToString() + ")";

            if (
                ((mitmAttack.t1.packetsSend == 0) && (mitmAttack.t2.packetsSend > 0))
                ||
                ((mitmAttack.t1.packetsSend > 0) && (mitmAttack.t2.packetsSend == 0))
                )
            {
                lbRoute.Text = "Route: Half";
            }
            else if ((mitmAttack.t1.packetsSend > 0) && (mitmAttack.t2.packetsSend > 0))
            {
                lbRoute.Text = "Route: Full";
            }
            else
            {
                if (!(lbRoute.Text == "Route: None"))
                    lbRoute.Text = "Route: None";
            }
        }

        private void controlMitmAttack_Load(object sender, EventArgs e)
        {
            if (((MitmAttack)attack).t1 != null)
                lbTarget1.Text = ((MitmAttack)attack).t1.ip.ToString();
            if (((MitmAttack)attack).t2 != null)
                lbTarget2.Text = ((MitmAttack)attack).t2.ip.ToString();
        }
    }
}
