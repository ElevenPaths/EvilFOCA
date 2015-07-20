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
using System.Net;

namespace evilfoca.LLMNR
{
    public class DNSAnswer
    {
        public DNSType? Type { get; internal set; }
        public DNSClass? Class { get; internal set; }
        public string Name { get; internal set; }
        public Int32 TTL { get; internal set; }
        public Int16 RDLength { get; internal set; }
        public byte[] RData { get; internal set; }

        public IPAddress IpAddressData
        {
            get
            {
                try
                {
                    if (RData.Length == 4 || RData.Length == 16)
                        return new IPAddress(RData);
                    else
                        return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public DNSAnswer()
        {
            RData = new byte[0];
        }
    }
}
