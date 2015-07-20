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

namespace evilfoca.Data
{
    public enum Protocol
    {
        TCP,
        UDP
    }

    public class Port
    {
        public Protocol protocol;
        public int port;
        public IPAddress ip;

        public Port(int port, Protocol protocol, IPAddress ip)
        {
            this.port = port;
            this.protocol = protocol;
            this.ip = ip;
        }

        public override string ToString()
        {
            return port.ToString() + "/" + protocol.ToString();
        }
    }
}
