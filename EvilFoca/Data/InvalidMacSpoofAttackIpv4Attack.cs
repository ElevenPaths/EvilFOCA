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
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace evilfoca.Data
{
    class InvalidMacSpoofAttackIpv4Attack : Attack
    {
        public Target t1;
        public Target t2;
        public PhysicalAddress invalidMac;

        public InvalidMacSpoofAttackIpv4Attack(Target t1, Target t2, AttackType attackType)
            : base(attackType)
        {
            invalidMac = PhysicalAddress.Parse("FA-BA-DA-FA-BA-DA".ToUpper());
            this.t1 = t1;
            this.t2 = t2;
        }
    }
}
