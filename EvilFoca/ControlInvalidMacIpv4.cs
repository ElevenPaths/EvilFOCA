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
using evilfoca.Data;

namespace evilfoca
{
    public partial class ControlInvalidMacIpv4 : UserControl
    {
        Attack attack;

        public ControlInvalidMacIpv4(Attack attack)
        {
            this.attack = attack;
            InitializeComponent();
        }


        private void ControlInvalidMacIpv4_Load(object sender, EventArgs e)
        {
            InvalidMacSpoofAttackIpv4Attack dosAttack = (InvalidMacSpoofAttackIpv4Attack)attack;
            lbInfo.Text = "IP " + dosAttack.t1.ip + " has not connectivity with " + dosAttack.t2.ip;
        }
    }
}
