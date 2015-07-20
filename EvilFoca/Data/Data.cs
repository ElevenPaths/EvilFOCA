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
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using evilfoca.HTTP;
using PacketDotNet;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

namespace evilfoca.Data
{
    public enum Platform
    {
        Unknow = 0,
        Windows = 1,
        Mac = 2,
        Linux = 3,
        OpenBSD = 4,
        Solaris = 5,
        AIX = 6
    }

    public class Data
    {
        private object neighborLock = new object();
        public SynchronizedCollection<Packet> stackPacketsFIFO = new SynchronizedCollection<Packet>();
        public EventHandler<NeighborEventArgs> NewNeighbor;
        private WinPcapDevice device;

        public Settings settings = new Settings();
        private SynchronizedCollection<Attack> attacks = new SynchronizedCollection<Attack>();
        internal static SynchronizedCollection<Neighbor> neighbors = new SynchronizedCollection<Neighbor>();


        public static SynchronizedCollection<string> SlaacReqList { get; private set; }
        public static SynchronizedCollection<string> WpadReqList { get; private set; }
        public SynchronizedCollection<TcpReconstructorPacket> ReconstructedPackets { get; private set; }


        public Data()
        {
            SlaacReqList = new SynchronizedCollection<string>();
            WpadReqList = new SynchronizedCollection<string>();
            ReconstructedPackets = new SynchronizedCollection<TcpReconstructorPacket>();
        }

        #region attacks_control_methods

        public void AddAttack(Attack attack)
        {
            attacks.Add(attack);
        }

        public SynchronizedCollection<Attack> GetAttacks()
        {
            return attacks;
        }

        #endregion

        #region neighbors_control_methods

        public bool ExistsNeighbor(Neighbor neighbor)
        {
            return neighbors.Contains(neighbor);
        }

        public bool ExistsNeighbor(PhysicalAddress physicalAddress)
        {
            return neighbors.Count(n => n.physicalAddress.Equals(physicalAddress)) > 0;
        }


        public void AddNeighbor(Neighbor neighbor)
        {

            NeighborEventArgs ea;

            if (neighbor.physicalAddress == null)
                throw new Exception("Neighbor without physical address");
            if (neighbor.physicalAddress.Equals(device.Interface.MacAddress))
                return;
            lock (neighborLock)
            {

                if (ExistsNeighbor(neighbor))
                {
                    Neighbor newNeighbor = GetNeighbor(neighbor.physicalAddress);
                    newNeighbor.Combine(neighbor);
                    ea = new NeighborEventArgs(newNeighbor);
                    ea.Tipo = NeighborOperacionTreeView.Actualizar;
                    NewNeighbor(newNeighbor, ea);
                    return;
                }

                if (neighbor.GetIPs().Count == 0)
                    throw new Exception("Neighbor without ips");

                neighbors.Add(neighbor);
                ea = new NeighborEventArgs(neighbor);
                ea.Tipo = NeighborOperacionTreeView.Añadir;
                NewNeighbor(neighbor, ea);

            }
            ReverseResolutionAsync(neighbor); // Resolucion de nombre a partir de IP
            _CheckIfNeighborRoutesPackets(neighbor);

        }

        public void RemoveNeighbor(Neighbor neighbor)
        {
            neighbors.Remove(neighbor);
        }

        public void RemoveNeighbor(PhysicalAddress physicalAddress)
        {
            for (int n = neighbors.Count - 1; n >= 0; n--)
            {
                Neighbor neighbor = neighbors[n];

                if (neighbor.physicalAddress.Equals(physicalAddress))
                    neighbors.Remove(neighbor);
            }
        }

        public Neighbor GetNeighbor(PhysicalAddress physicalAddress)
        {
            for (int n = neighbors.Count - 1; n >= 0; n--)
            {
                Neighbor neighbor = neighbors[n];

                if (neighbor.physicalAddress.Equals(physicalAddress))
                    return neighbor;
            }

            return null;
        }

        public IPAddress GetIPv4FromNeighbor(Neighbor neighbor)
        {
            for (int i = 0; i < neighbor.GetIPs().Count; i++)
            {
                if (neighbor.GetIPs()[i].GetAddressBytes().Length == 4)
                    return neighbor.GetIPs()[i];
            }
            return null;
        }

        public IPAddress GetIPv6FromNeighbor(Neighbor neighbor)
        {
            for (int i = 0; i < neighbor.GetIPs().Count; i++)
            {
                if (neighbor.GetIPs()[i].GetAddressBytes().Length == 16)
                    return neighbor.GetIPs()[i];
            }
            return null;
        }

        public Neighbor GetNeighbor(IPAddress ip)
        {
            for (int n = neighbors.Count - 1; n >= 0; n--)
            {
                Neighbor neighbor = neighbors[n];

                for (int i = 0; i < neighbor.GetIPs().Count; i++)
                {
                    if (neighbor.GetIPs()[i].Equals(ip))
                        return neighbor;
                }
            }

            return null;
        }

        public PhysicalAddress GetNeighborMAC(IPAddress ip)
        {
            foreach (Neighbor neighbor in neighbors)
            {
                if (neighbor.ExistsIP(ip))
                    return neighbor.physicalAddress;
            }
            return null;
        }

        public void CheckIfNeighborRoutesPackets(Neighbor n)
        {
            _CheckIfNeighborRoutesPackets(n);
        }

        private void _CheckIfNeighborRoutesPackets(Neighbor n)
        {
            if (n == null)
                return;

            IPAddress IpSrc = Program.CurrentProject.data.GetIPv4FromDevice(device);
            if (IpSrc == null)
                return;

            n.canRoutePackets = RouteStatus.Verifing;
            EthernetPacket eth = new EthernetPacket(Program.CurrentProject.data.GetDevice().MacAddress, n.physicalAddress, EthernetPacketType.IpV4);

            IPAddress IpDst = IPAddress.Parse("8.8.8.8");
            eth.PayloadPacket = new IPv4Packet(IpSrc, IpDst);
            eth.PayloadPacket.PayloadPacket = new TcpPacket((ushort)31337, (ushort)53);

            ((IPv4Packet)eth.PayloadPacket).UpdateIPChecksum();
            ((TcpPacket)eth.PayloadPacket.PayloadPacket).Syn = true;
            ((TcpPacket)eth.PayloadPacket.PayloadPacket).UpdateTCPChecksum();

            Thread t = new Thread(new ParameterizedThreadStart(CheckIfRouted));
            t.IsBackground = true;
            t.Start(n);
            Program.CurrentProject.data.SendPacket(eth);
        }

        private void CheckIfRouted(object o)
        {
            // Timeout de 10 segundos hasta recibir el paquete de respuesta
            System.Threading.Thread.Sleep(10000);

            if (o is Neighbor)
            {
                Neighbor n = (Neighbor)o;
                if (n.canRoutePackets == RouteStatus.Verifing)
                    n.canRoutePackets = RouteStatus.CantRoute;
            }
        }

        #endregion neighbor_functions

        #region Utils_methods

        public IPAddress GetIPv6LocalLinkFromDevice(WinPcapDevice device)
        {
            if (device == null)
                return null;

            foreach (PcapAddress address in ((WinPcapDevice)device).Addresses.Where(A => A.Addr.ipAddress != null && A.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
            {
                if (address.Addr.ipAddress.IsIPv6LinkLocal)
                    return address.Addr.ipAddress;

            }

            return null;
        }

        public IPAddress GetIPv4FromDevice(WinPcapDevice device)
        {
            foreach (PcapAddress address in ((WinPcapDevice)device).Addresses.Where(A => A.Addr.ipAddress != null && A.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
            {
                return address.Addr.ipAddress;
            }

            return null;
        }

        public Sockaddr GetNetmaskFromIP(WinPcapDevice device, IPAddress ipAddress)
        {
            foreach (PcapAddress address in device.Addresses.Where(A => A.Addr.ipAddress.Equals(ipAddress)))
            {
                return address.Netmask;
            }

            return null;
        }
        #endregion

        private void ReverseResolution(object oNeighbor)
        {
            Neighbor neighbor = (Neighbor)oNeighbor;
            if (neighbor == null)
                return;

            if (string.IsNullOrEmpty(neighbor.computerName))
            {
                IList<IPAddress> ips = new List<IPAddress>(neighbor.GetIPs());
                foreach (IPAddress ip in ips)
                {
                    try
                    {
                        Dns.GetHostEntry(ip);
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void ReverseResolutionAsync(Neighbor n)
        {
            Thread t = new Thread(new System.Threading.ParameterizedThreadStart(ReverseResolution));
            t.IsBackground = true;
            t.Start(n);
        }

        #region Device_methods

        public void SetDevice(WinPcapDevice device)
        {
            this.device = device;
        }

        public WinPcapDevice GetDevice()
        {
            return this.device;
        }

        public void SendPacket(Packet p)
        {
            if (p == null)
                return;
            if (device == null)
                return;
            try
            {
                if (device.Opened)
                    device.SendPacket(p);
            }
            catch (SharpPcap.PcapException)
            {
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Windows.Forms.MessageBox.Show("DEBUG: Error enviando paquete: " + ex.Message);
#endif
            }
        }

        #endregion

        #region Utils

        public int GetImageNumberOS(Platform platform)
        {
            if ((platform == Platform.Unknow) || (platform == Platform.AIX) || (platform == Platform.Mac) || (platform == Platform.OpenBSD) || (platform == Platform.Solaris))
                return -1;
            else if (platform == Platform.Windows)
                return 9;
            else if (platform == Platform.Linux)
                return 10;

            return -1;
        }


        #endregion
    }
}
