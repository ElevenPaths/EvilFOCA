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

namespace evilfoca.DNS
{
    class IPv6Response
    {
        IPv6Query.Type type;
        Int16 transID;
        byte[] dnsName;
        IPAddress ipv4;

        public IPv6Response(IPv6Query.Type type, Int16 transID, byte[] dnsName, IPAddress ipv4)
        {
            this.type = type;
            this.transID = transID;
            this.dnsName = dnsName;
            this.ipv4 = ipv4;
        }

        public byte[] GeneratePacket()
        {
            byte[] raw = null;
            if (type == IPv6Query.Type.Ipv6)
                raw = new byte[32 + dnsName.Length + 4 + 4 + 4];
            else
                raw = new byte[32 + dnsName.Length];

            using (MemoryStream ms = new MemoryStream(raw))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((Int16)transID);
                    bw.Write((byte)0x81);
                    bw.Write((byte)0x80);

                    bw.Write((byte)0x00);
                    bw.Write((byte)0x01);
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x01); //AnswerCount
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);

                    for (int i = 0; i < dnsName.Length; i++)
                        bw.Write((byte)dnsName[i]);

                    if (type == IPv6Query.Type.Ipv6)
                    {
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x1c);
                    }
                    else if (type == IPv6Query.Type.Ipv4)
                    {
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x01);
                    }

                    bw.Write((byte)0x00);
                    bw.Write((byte)0x01);

                    // ANSWER
                    bw.Write((byte)0xc0);
                    bw.Write((byte)0x0c);

                    if (type == IPv6Query.Type.Ipv6)
                    {
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x1c);
                    }
                    else if (type == IPv6Query.Type.Ipv4)
                    {
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x01);
                    }

                    bw.Write((byte)0x00);
                    bw.Write((byte)0x01);

                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x0e);
                    bw.Write((byte)0x10);

                    bw.Write((byte)0x00);

                    if (type == IPv6Query.Type.Ipv6)
                    {
                        //length IPv6
                        bw.Write((byte)0x10);

                        //IPAddress
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x64);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0x00);
                        bw.Write((byte)0xff);
                        bw.Write((byte)0xff);
                    }
                    else
                        //length IPv4
                        bw.Write((byte)0x04);

                    bw.Write((byte)ipv4.GetAddressBytes()[0]);
                    bw.Write((byte)ipv4.GetAddressBytes()[1]);
                    bw.Write((byte)ipv4.GetAddressBytes()[2]);
                    bw.Write((byte)ipv4.GetAddressBytes()[3]);
                }
            }
            return raw;
        }
    }
}
