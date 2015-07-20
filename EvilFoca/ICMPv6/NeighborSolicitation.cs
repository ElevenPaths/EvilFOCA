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
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace evilfoca.ICMPv6
{
    class NeighborSolicitation
    {
        byte[] raw;
        public IPAddress solicitedAddress = null;
        public PhysicalAddress sourceAddress = null;

        public NeighborSolicitation(byte[] nbSolicitation)
        {
            raw = nbSolicitation;
            ParsePacket();
        }

        private void ParsePacket()
        {
            MemoryStream ms = new MemoryStream(raw);
            BinaryReader br = new BinaryReader(ms);

            // type
            br.ReadByte(); // Debe ser 135 (0x87)
            // code
            br.ReadByte();
            // checksum
            br.ReadInt16();
            // reserver
            br.ReadInt32();
            // target address
            solicitedAddress = new IPAddress(br.ReadBytes(16));
            // type source link address
            br.ReadByte();
            // longitud mac
            br.ReadByte(); // Siempre serán 8 bytes, una MAC.
            sourceAddress = new PhysicalAddress(br.ReadBytes(8));
        }
    }
}
