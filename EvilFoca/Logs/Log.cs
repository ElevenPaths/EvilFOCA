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

namespace evilfoca.Logs
{
    public class Log
    {
        public enum LogType
        {
            Neighbor,
            NeighborSpoofing,
            DNSHijacking,
            DHCPACKInjection,
            NetworkDiscovery,
            Core,
            DoS,
            WpadIPv4,
            WpadIPv6
        }


        public LogType logType;
        public string message;
        public DateTime datetime;

        public Log(string message, LogType logType)
        {
            datetime = DateTime.Now;
            this.logType = logType;
            this.message = message;
        }
    }
}
