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

namespace evilfoca.LLMNR
{
    //
    // Link-local Multicast Name Resolution
    // Ipv4 y IPv6
    //
    // http://en.wikipedia.org/wiki/Link-local_Multicast_Name_Resolution
    //

    class LLMNRAnswer
    {
        byte[] raw;
        public string computerName = string.Empty;
        public IPAddress ipAddress = null;
        public bool isPtrResponse = false;

        public LLMNRAnswer(byte[] LLMNRPayloadRaw)
        {
            raw = LLMNRPayloadRaw;
            ParsePacket();
        }

        private void ParsePacket()
        {
            MemoryStream ms = new MemoryStream(raw);
            BinaryReader br = new BinaryReader(ms);

            // transaction id
            br.ReadInt16();
            // flags
            br.ReadInt16();
            // questions
            Int16 questions = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // answers RRs
            Int16 answers = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // autority RRs
            br.ReadInt16();
            // additional RRs
            br.ReadInt16();

            // Parseamos solo si se hizo una solicitud y se obtuvo una respuesta
            if ((questions != 1) && (answers != 1))
                return;

            // query name
            string queryName = br.ReadString();
            br.ReadByte(); // 0x00 despues de string

            // type
            Int16 typeQuery = IPAddress.NetworkToHostOrder(br.ReadInt16());
            if ((typeQuery == 0x0c) || (typeQuery == 0x01))
                isPtrResponse = true;
            else
            {
                return;
                //isPtrResponse = false;
                //queryName = "";
            }


            //class
            Int16 classQuery = IPAddress.NetworkToHostOrder(br.ReadInt16());

            // answer name
            string answerName = br.ReadString();
            br.ReadByte(); // 0x00 despues de string

            // type answer
            Int16 typeAnswer = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // class answer
            Int16 classAnswer = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // time to live
            Int32 timeToLive = br.ReadInt32();
            // longitud de la ip, ¿ipv4 o ipv6?
            Int16 ipLen = IPAddress.NetworkToHostOrder(br.ReadInt16());

            if (ipLen == 4) // ipv4
            {
                byte[] ipV4 = new byte[ipLen];

                for (int i = 0; i < ipLen; i++)
                    ipV4[i] = br.ReadByte();
                this.ipAddress = new IPAddress(ipV4);

            }
            else if (ipLen == 16) // ipv6
            {
                byte[] ipV6 = new byte[ipLen];

                for (int i = 0; i < ipLen; i++)
                    ipV6[i] = br.ReadByte();
                this.ipAddress = new IPAddress(ipV6);
            }

            this.computerName = answerName;
        }

    }
}
