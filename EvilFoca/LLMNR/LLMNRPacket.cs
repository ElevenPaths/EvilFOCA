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
using System.IO;
using System.Net;
using evilfoca.Data;

namespace evilfoca.LLMNR
{
    public enum DNSType
    {
        A = 1,
        CNAME = 5,
        SOA = 6,
        PTR = 12,
        TXT = 16,
        AAAA = 28
    }

    public enum DNSClass
    {
        IN = 1
    }

    public class LLMNRPacket
    {
        //Header Fields
        public byte[] Id { get; private set; }
        public Int16 OpCode { get; private set; }
        public bool C { get; set; }
        public bool TC { get; set; }
        public bool T { get; set; }
        public bool IsResponse { get; set; }
        public Int16 RCode { get; private set; }
        public Int16 QDCOUNT { get; private set; }
        public Int16 ANCOUNT { get; private set; }
        public Int16 NSCOUNT { get; private set; }
        public Int16 ARCOUNT { get; private set; }

        public DNSQuery Query { get; private set; }
        public List<DNSAnswer> AnswerList { get; private set; }

        public LLMNRPacket()
        {
            AnswerList = new List<DNSAnswer>();
        }

        public void ParsePacket(byte[] LLMNRPayloadRaw)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(LLMNRPayloadRaw))
                {
                    using (BitReader bitReader = new BitReader(ms))
                    {
                        // transaction id
                        this.Id = new byte[] { (byte)bitReader.ReadByte(), (byte)bitReader.ReadByte() };
                        // flags
                        IsResponse = bitReader.ReadBit(); //1bit
                        OpCode = (short)bitReader.ReadLSB(4); //4bits
                        C = bitReader.ReadBit(); //1bit
                        TC = bitReader.ReadBit(); //1bit
                        T = bitReader.ReadBit(); //1bit
                        bitReader.ReadLSB(4); //4bits ZZZZ
                        RCode = (short)bitReader.ReadLSB(4); //4bits

                        // questions
                        QDCOUNT = IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                        // answers RRs
                        ANCOUNT = IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                        // autority RRs
                        NSCOUNT = IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                        // additional RRs
                        ARCOUNT = IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));

                        this.Query = new DNSQuery();

                        // query name
                        this.Query.Name = bitReader.ReadStringLSB();
                        // type
                        this.Query.Type = (DNSType?)IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                        //class
                        this.Query.Class = (DNSClass?)IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));

                        for (int i = 0; i < this.ANCOUNT; i++)
                        {
                            DNSAnswer answer = new DNSAnswer();

                            // answer name
                            answer.Name = bitReader.ReadStringLSB();
                            // type answer
                            answer.Type = (DNSType)IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                            // class answer
                            answer.Class = (DNSClass?)IPAddress.NetworkToHostOrder(bitReader.ReadLSB(16));
                            // time to live
                            answer.TTL = bitReader.ReadLSB(32);
                            // longitud RData
                            answer.RDLength = IPAddress.NetworkToHostOrder((short)bitReader.ReadLSB(16));
                            List<byte> arrayList = new List<byte>();
                            for (int index = 0; index < answer.RDLength; index++)
                                arrayList.Add((byte)bitReader.ReadByte());
                            answer.RData = arrayList.ToArray();

                            this.AnswerList.Add(answer);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public byte[] BuildPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BitWriter writer = new BitWriter(ms))
                {
                    //Id
                    writer.WriteByte(this.Id[0]);
                    writer.WriteByte(this.Id[1]);

                    // flags
                    writer.WriteByte(0x80);
                    writer.WriteByte(0x00);

                    // questions
                    writer.WriteInt16(IPAddress.HostToNetworkOrder(QDCOUNT));
                    // answers RRs
                    writer.WriteInt16(IPAddress.HostToNetworkOrder((short)AnswerList.Count));
                    // autority RRs
                    writer.WriteInt16(IPAddress.HostToNetworkOrder(NSCOUNT));
                    // additional RRs
                    writer.WriteInt16(IPAddress.HostToNetworkOrder(ARCOUNT));

                    //QUERY
                    if (this.Query != null)
                    {
                        // query name
                        writer.WriteString(Query.Name);
                        // type
                        writer.WriteInt16(IPAddress.HostToNetworkOrder((Query.Type.HasValue ? (short)Query.Type.Value : (short)0)));
                        //class
                        writer.WriteInt16(IPAddress.HostToNetworkOrder((Query.Class.HasValue ? (short)Query.Class.Value : (short)0)));
                    }
                    foreach (var item in AnswerList)
                    {

                        // answer name
                        writer.WriteString(item.Name);
                        // type answer
                        writer.WriteInt16(IPAddress.HostToNetworkOrder((item.Type.HasValue ? (short)item.Type.Value : (short)0)));
                        // class answer
                        writer.WriteInt16(IPAddress.HostToNetworkOrder((item.Class.HasValue ? (short)item.Class.Value : (short)0)));
                        // time to live
                        writer.WriteByte(0x00);
                        writer.WriteByte(0x00);
                        writer.WriteByte(0x00);
                        writer.WriteByte(0x1e);

                        // longitud RData
                        writer.WriteInt16(IPAddress.HostToNetworkOrder((short)item.RData.Length));
                        //RDATA
                        for (int index = 0; index < item.RData.Length; index++)
                            writer.WriteByte(item.RData[index]);
                    }
                }
                return ms.ToArray();
            }

        }
    }
}
