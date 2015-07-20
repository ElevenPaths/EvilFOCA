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
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

namespace evilfoca.Core
{
    public class NetworkDiscovery
    {
        private WinPcapDevice Device;
        public delegate void DiscoverFinishedDelegate(object sender, EventArgs myArgs);
        public event DiscoverFinishedDelegate OnDiscoverFinish;


        public bool IsScanning
        {
            get;
            private set;
        }

        public NetworkDiscovery()
        {
        }

        public void SetDevice(WinPcapDevice device)
        {
            Device = device;
        }

        public void StartNetworkDiscoveryJustOneTime()
        {
            try
            {
                IsScanning = true;
                // Fuerza el network discovery sin tener en cuenta la configuración
                _StartNetworkDiscovery();

            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                DiscoverFinished();
            }

        }

        public void StartNetworkDiscoveryJustOneTime(object ipRange)
        {
            try
            {
                IPAddress[] range = ipRange as IPAddress[];
                if (range != null && range.Length >= 2)
                {
                    IsScanning = true;
                    // Fuerza el network discovery sin tener en cuenta la configuración
                    _StartNetworkDiscovery(range[0], range[1]);
                }

            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                DiscoverFinished();
            }

        }

        private void DiscoverFinished()
        {
            IsScanning = false;
            if (OnDiscoverFinish != null)
                OnDiscoverFinish(this, null);
        }

        private void _StartNetworkDiscovery(IPAddress from, IPAddress to)
        {
            IPAddress ipLocalLink = Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Device);

            if (ipLocalLink == null)
            {
#if DEBUG
                System.Windows.Forms.MessageBox.Show("DEBUG: Excepción en el hilo de NetworkDiscovery");
#endif
            }

            {
                // Si lanzamos paquetes de router advertisement vamos a localizar más equipos, pero les vamos a
                // modificar la puerta de enlace predeterminada Ipv6 por la nuestra.
                if (Program.CurrentProject.data.settings.DiscoverWithRouterAdvertisement)
                    HostDiscoveryRouterAdvertisement();
                if (Program.CurrentProject.data.settings.DiscoverWithPingMulticastIpv6)
                    HostDiscoveryPingMulticast();
                if (Program.CurrentProject.data.settings.DiscoverWithArpScan)
                    HostDiscoveryArpIpRange(from, to);
            }
        }

        private void _StartNetworkDiscovery()
        {
            IPAddress ipLocalLink = Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Device);

            if (ipLocalLink == null)
            {
#if DEBUG
                System.Windows.Forms.MessageBox.Show("DEBUG: Excepción en el hilo de NetworkDiscovery");
#endif
                throw new Exception();
            }

            {
                // Si lanzamos paquetes de router advertisement vamos a localizar más equipos, pero les vamos a
                // modificar la puerta de enlace predeterminada Ipv6 por la nuestra.
                if (Program.CurrentProject.data.settings.DiscoverWithRouterAdvertisement)
                    HostDiscoveryRouterAdvertisement();
                if (Program.CurrentProject.data.settings.DiscoverWithPingMulticastIpv6)
                    HostDiscoveryPingMulticast();
                if (Program.CurrentProject.data.settings.DiscoverWithArpScan)
                    HostDiscoveryArpNetmask();
            }
        }

        private void HostDiscoveryRouterAdvertisement()
        {
            //
            // Host discovery enviando router advertisements
            // http://dev.metasploit.com/redmine/projects/framework/repository/revisions/67120d4263806eaedcad03761439509eda5cba12/entry/modules/auxiliary/scanner/discovery/ipv6_neighbor_router_advertisement.rb
            //

            EthernetPacket ethernet = new EthernetPacket(Device.Interface.MacAddress,
                                                                new PhysicalAddress(new byte[] { 0x33, 0x33, 0x0, 0x0, 0x0, 0x1 }),
                                                                EthernetPacketType.IpV6);

            IPAddress[] ipsMultiCast = new IPAddress[] { 
                                                            new IPAddress(new byte[] { 0xff, 0x02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 })
                                                        };

            foreach (IPAddress ipMulticast in ipsMultiCast)
            {
                IPv6Packet ipv6 = new IPv6Packet(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Device), ipMulticast);
                ipv6.HopLimit = 255;

                ICMPv6Packet routerAdvertisement = new ICMPv6Packet(new ByteArraySegment(new ICMPv6.NeighborRouterAdvertisement(Device.Interface.MacAddress).GetBytes()));

                ethernet.PayloadPacket = ipv6;
                ipv6.PayloadPacket = routerAdvertisement;
                Program.CurrentProject.data.SendPacket(ethernet);
            }
        }

        private void HostDiscoveryPingMulticast()
        {
            //
            // Host discovery enviando pings a las direcciones multicast
            // http://dev.metasploit.com/redmine/projects/framework/repository/revisions/67120d4263806eaedcad03761439509eda5cba12/entry/modules/auxiliary/scanner/discovery/ipv6_multicast_ping.rb
            //
            // Nota: Parece que los windows no responden a los pings mutlicasts. Los linux (backtrack) si lo hacen
            //

            IPAddress ipLocalLink = Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Device);

            EthernetPacket ethernet = new EthernetPacket(Device.Interface.MacAddress,
                                                            new PhysicalAddress(new byte[] { 0x33, 0x33, 0x0, 0x0, 0x0, 0x1 }),
                                                            EthernetPacketType.IpV6);
            ICMPv6Packet icmp = new ICMPv6Packet(new ByteArraySegment(new byte[40]))
            {
                Type = ICMPv6Types.EchoRequest,
                PayloadData = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwabcdefghi")
            };

            IPAddress[] ipsMultiCast = new IPAddress[] { 
                                                            new IPAddress(new byte[] { 0xff, 0x01, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }), 
                                                            new IPAddress(new byte[] { 0xff, 0x01, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 }), 
                                                            new IPAddress(new byte[] { 0xff, 0x02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }), 
                                                            new IPAddress(new byte[] { 0xff, 0x02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 })
                                                        };

            foreach (IPAddress ipMulticast in ipsMultiCast)
            {
                IPv6Packet ipv6 = new IPv6Packet(
                                                    ipLocalLink,
                                                    ipMulticast
                                                );
                ethernet.PayloadPacket = ipv6;
                ipv6.PayloadPacket = icmp;
                Program.CurrentProject.data.SendPacket(ethernet);
            }
        }

        private void HostDiscoveryArpNetmask()
        {
            IPAddress localIpv4 = Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice());

            if ((Program.CurrentProject.data.GetDevice() == null) || (localIpv4 == null))
            {
#if DEBUG
                MessageBox.Show("Debug: La interfaz no tiene dirección IPv4", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                return;
            }

            Sockaddr netmask = Program.CurrentProject.data.GetNetmaskFromIP(Program.CurrentProject.data.GetDevice(), localIpv4);
            SendARPIps(localIpv4, netmask);
        }

        private void HostDiscoveryArpIpRange(IPAddress ipFrom, IPAddress ipTo)
        {
            IPAddress localIpv4 = Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice());

            if ((Program.CurrentProject.data.GetDevice() == null) || (localIpv4 == null))
            {
#if DEBUG
                MessageBox.Show("Debug: La interfaz no tiene dirección IPv4", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                return;
            }

            Sockaddr netmask = Program.CurrentProject.data.GetNetmaskFromIP(Program.CurrentProject.data.GetDevice(), localIpv4);
            SendARPIps(ipFrom, ipTo);
        }

        private void SendARPIps(IPAddress from, IPAddress to)
        {
            IPAddress localIpv4 = Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice());

            string[] rangeFromSplit = from.ToString().Split('.');
            string[] rangeToSplit = to.ToString().Split('.');

            int[] rangeFrom = new int[] {  int.Parse(rangeFromSplit[0]),
                                       int.Parse(rangeFromSplit[1]),
                                       int.Parse(rangeFromSplit[2]),
                                       int.Parse(rangeFromSplit[3])
                                    };

            int[] rangeTo = new int[] {  int.Parse(rangeToSplit[0]),
                                     int.Parse(rangeToSplit[1]),
                                     int.Parse(rangeToSplit[2]),
                                     int.Parse(rangeToSplit[3])
                                    };
            while (string.Format("{0}.{1}.{2}.{3}", rangeFrom[0], rangeFrom[1], rangeFrom[2], rangeFrom[3]) !=
                      string.Format("{0}.{1}.{2}.{3}", rangeTo[0], rangeTo[1], rangeTo[2], rangeTo[3]))
            {
                string sIp = string.Format("{0}.{1}.{2}.{3}", rangeFrom[0], rangeFrom[1], rangeFrom[2], rangeFrom[3]);

                EthernetPacket ethernet = new EthernetPacket(Program.CurrentProject.data.GetDevice().Interface.MacAddress,
                                                    new PhysicalAddress(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }),
                                                    EthernetPacketType.Arp);
                ethernet.PayloadPacket = new ARPPacket(ARPOperation.Request,
                                                        new PhysicalAddress(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 }),
                                                        IPAddress.Parse(sIp),
                                                        Program.CurrentProject.data.GetDevice().Interface.MacAddress,
                                                        localIpv4);
                Program.CurrentProject.data.SendPacket(ethernet);

                if (rangeFrom[3] >= 255)
                {
                    rangeFrom[3] = 0;

                    if (rangeFrom[2] >= 255)
                    {
                        rangeFrom[2] = 0;
                        if (rangeFrom[1] >= 255)
                        {
                            rangeFrom[1] = 0;
                            rangeFrom[0]++;
                            if (rangeFrom[0] >= 255)
                                break; // Esto no debería pasar nunca.
                        }
                        else
                            rangeFrom[1]++;
                    }
                    else
                        rangeFrom[2]++;
                }
                else
                    rangeFrom[3]++;
            }

            // return lstIps;
        }

        private void SendARPIps(IPAddress ip, Sockaddr netmask)
        {
            IPAddress firstIP = new IPAddress(ip.Address & netmask.ipAddress.Address);
            IPAddress lastIP = new IPAddress((uint)((int)ip.Address | (int)~netmask.ipAddress.Address));

            SendARPIps(firstIP, lastIP);
        }
    }
}
