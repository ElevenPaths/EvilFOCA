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

namespace evilfoca.DNS
{
    class IPv6Query
    {
        public enum Type
        {
            Unknow,
            Ipv6,
            Ipv4

        }

        byte[] raw;
        public string name = string.Empty;
        public byte[] nameDnsFormat;
        public Type type = Type.Unknow;
        public Int16 transID = 0x0;

        public IPv6Query(byte[] raw)
        {
            this.raw = raw;
            ParsePacket();
        }

        private void ParsePacket()
        {
            MemoryStream ms = new MemoryStream(raw);
            BinaryReader br = new BinaryReader(ms);
            StringBuilder sbName = new StringBuilder();

            List<byte> b = new List<byte>();

            transID = br.ReadInt16();
            br.ReadBytes(10);
            byte readedByte = 0x00;
            do
            {
                readedByte = br.ReadByte();
                b.Add(readedByte);

                byte[] buff = br.ReadBytes(readedByte);
                for (int i = 0; i < buff.Length; i++)
                    b.Add(buff[i]);

                sbName.Append(ASCIIEncoding.ASCII.GetString(buff) + ".");
                
            } while (readedByte != 0x00);

            nameDnsFormat = new byte[b.Count];
            for (int i = 0; i < b.Count; i++)
            {
                nameDnsFormat[i] = b[i];
            }

            sbName = new StringBuilder(sbName.ToString().Substring(0, sbName.ToString().Length - 2));

            byte[] buffType = br.ReadBytes(2);
            if (buffType[0] == 0x00 && buffType[1] == 0x1c)
                type = Type.Ipv6;
            else if (buffType[0] == 0x00 && buffType[1] == 0x01)
                type = Type.Ipv4;

            name = sbName.ToString();
        }
    }
}
