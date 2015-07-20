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
using System.Net;
using System.Net.NetworkInformation;

namespace evilfoca.Data
{

    public class Target
    {
        public IPAddress ip;
        public PhysicalAddress mac;
        public int packetsSend = 0;
        public string description = string.Empty;


        public Target(IPAddress ip, PhysicalAddress mac)
        {
            this.ip = ip;
            this.mac = mac;
        }

        public Target(string description)
        {
            this.ip = null;
            this.mac = null;

            this.description = description;
        }
    }
}
