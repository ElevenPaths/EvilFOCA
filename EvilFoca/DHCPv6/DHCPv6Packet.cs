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
using System.Net.NetworkInformation;

namespace evilfoca.DHCPv6
{
    public enum DHCPv6Type
    {
        Solicit = 1,
        Advertise = 2,
        Request = 3,
        Confirm = 4,
        Renew = 5,
        Rebind = 6,
        Reply = 7,
        Release = 8,
        Reconfigure = 10,
        InformationRequest = 11
    }

    public enum DHCPv6OptionCode
    {
        ClientIdentifier = 1,
        ServerIdentifier = 2,
        IANA = 3,
        IPAddress = 5,
        OptionRequest = 6,
        ElapsedTime = 8,
        DNS = 23,
        VendorClass = 16,
        DomainList = 24,
        FQDM = 39

    }

    public class DHCPv6Packet
    {
        public DHCPv6Type MessageType { get; set; }
        public byte[] TransactionId { get; set; }
        public Dictionary<DHCPv6OptionCode, DHCPv6Option> Options { get; set; }

        public DHCPv6Packet()
        {
            Options = new Dictionary<DHCPv6OptionCode, DHCPv6Option>();
        }

        public void ParsePacket(byte[] PacketData)
        {
            using (MemoryStream ms = new MemoryStream(PacketData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    MessageType = (DHCPv6Type)br.ReadByte();
                    TransactionId = br.ReadBytes(3);

                    do
                    {
                        DHCPv6Option opt = new DHCPv6Option();
                        opt.Option = (DHCPv6OptionCode)IPAddress.NetworkToHostOrder(br.ReadInt16());
                        opt.Length = IPAddress.NetworkToHostOrder(br.ReadInt16());
                        opt.Value = br.ReadBytes(opt.Length);
                        Options.Add(opt.Option, opt);
                    } while (br.BaseStream.Position < br.BaseStream.Length);
                }
            }
        }

        public byte[] BuildPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)MessageType);
                    writer.Write(TransactionId);

                    foreach (var item in Options.Values)
                    {
                        writer.Write(IPAddress.HostToNetworkOrder((short)item.Option));
                        writer.Write(IPAddress.NetworkToHostOrder((short)item.Length));
                        writer.Write(item.Value);
                    }

                }
                return ms.ToArray();
            }
        }

        public void AddServerIdentifierOption(PhysicalAddress mac)
        {
            this.Options.Add(DHCPv6OptionCode.ServerIdentifier, new DHCPv6Option()
            {
                Option = DHCPv6OptionCode.ServerIdentifier,
                Length = 14,
                Value = CreateServerIdentifierValue(mac)
            }
           );
        }

        public void AddIANAOption(IPAddress ip, byte[] IAID)
        {
            this.Options.Add(DHCPv6OptionCode.IANA, new DHCPv6Option()
            {
                Option = DHCPv6OptionCode.IANA,
                Length = 40,
                Value = CreateIANAValue(ip, IAID)
            }
           );
        }
        public void AddDNSOption(IPAddress DNSIp)
        {
            this.Options.Add(DHCPv6OptionCode.DNS, new DHCPv6Option()
            {
                Option = DHCPv6OptionCode.DNS,
                Length = 16,
                Value = CreateDNSValue(DNSIp)
            }
           );
        }

        public void AddDomainSearchListOption(string[] domains)
        {
            DHCPv6Option opt = new DHCPv6Option();
            opt.Option = DHCPv6OptionCode.DomainList;
            opt.Value = CreateDomainSearchListValue(domains);
            opt.Length = (short)opt.Value.Length;
            Options.Add(DHCPv6OptionCode.DomainList, opt);
        }

        private byte[] CreateServerIdentifierValue(PhysicalAddress mac)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)0x00); // duid type
                    writer.Write((byte)0x01); // duid type
                    writer.Write((byte)0x00); // hardware type
                    writer.Write((byte)0x01); // hardware type

                    writer.Write(DateTo4BytesString(DateTime.Now));
                    writer.Write(mac.GetAddressBytes());//mac
                }
                return ms.ToArray();
            }
        }

        private byte[] CreateDomainSearchListValue(string[] values)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    foreach (var item in values)
                        writer.Write(item);
                }
                return ms.ToArray();
            }
        }

        private byte[] CreateIANAValue(IPAddress ip, byte[] IAID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(IAID[0]);
                    writer.Write(IAID[1]);
                    writer.Write(IAID[2]);
                    writer.Write(IAID[3]);

                    writer.Write(IPAddress.NetworkToHostOrder((int)32512)); // t1
                    writer.Write(IPAddress.NetworkToHostOrder((int)65024)); // t2

                    writer.Write((byte)0x00);
                    writer.Write((byte)0x05);
                    writer.Write(IPAddress.NetworkToHostOrder((short)24)); // len
                    //// IP?
                    {
                        writer.Write(ip.GetAddressBytes());
                    }
                    writer.Write(IPAddress.NetworkToHostOrder((int)131072)); // prefered lifetime
                    writer.Write(IPAddress.NetworkToHostOrder((int)131072)); // valid lifetime
                }
                return ms.ToArray();
            }
        }

        private byte[] DateTo4BytesString(DateTime date)
        {
            DateTime time = DateTime.Parse("2000/01/01 00:00:00 GMT");
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int32)date.Subtract(time).TotalSeconds));
        }

        private byte[] CreateDNSValue(IPAddress ip)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    //// IP
                    writer.Write(ip.GetAddressBytes());
                }
                return ms.ToArray();
            }
        }
    }
}
