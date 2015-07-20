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

namespace evilfoca.Data
{
    public class NeighborEventArgs : EventArgs
    {
        public Neighbor Neighbor;
        public NeighborOperacionTreeView Tipo;


        public NeighborEventArgs(Neighbor Neighbor)
        {
            this.Neighbor = Neighbor;
        }
    }
    public enum RouteStatus
    {
        Unknow,
        Verifing,
        CanRoute,
        CantRoute
    }
    public enum NeighborOperacionTreeView
    {
        Añadir,
        Actualizar,
        Eliminar
    }

    public class Neighbor
    {
        public PhysicalAddress physicalAddress;
        public string computerName = string.Empty;
        private SynchronizedCollection<IPAddress> ips;
        public Platform osPlatform;
        public RouteStatus canRoutePackets;
        private SynchronizedCollection<Port> ports;

        public Neighbor()
        {
            ports = new SynchronizedCollection<Port>();
            canRoutePackets = RouteStatus.Unknow;
            osPlatform = Platform.Unknow;
            ips = new SynchronizedCollection<IPAddress>();
        }

        #region ips_control_methods

        public bool ExistsIP(IPAddress ip)
        {
            return ips.Contains(ip);
        }

        public void RemoveIP(IPAddress ip)
        {
            ips.Remove(ip);
        }

        public void AddIP(IPAddress ip)
        {
            if (ExistsIP(ip))
                return;

            // --- FILTROS IPV6 ---
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                if (ip.ToString() == "::") // IP Local IPv6. No se almacenan
                    return;
                if (ip.ToString().ToLower().StartsWith("ff02:")) // Dirección multicast ipv6. No se almacenan.
                    return;
            }
            // --- FILTROS IPV4 ---
            else if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                // multicast ipv4, desde la 224.0.0.0 a la 239.255.255.255
                int firstOct = int.Parse(ip.ToString().Split(new char[] { '.' })[0]);
                if ((firstOct >= 224) && (firstOct <= 239))
                    return; // multicast ipv5

                if (ip.ToString() == "255.255.255.255")
                    return;
            }

            ips.Add(ip);
        }

        public IPAddress GetIPv4()
        {
            return ips.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        public IPAddress GetIPv6()
        {
            return ips.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
        }

        public IList<IPAddress> GetIPs()
        {
            return ips;
        }

        #endregion

        #region ports

        public bool ExistsPort(Port port)
        {
            foreach (Port p in ports)
            {
                if (p.port == port.port && p.protocol == port.protocol && p.ip.Equals(port.ip))
                    return true;
            }

            return false;
        }

        public void AddPort(Port port)
        {
            if (ExistsPort(port))
                return;

            ports.Add(port);
        }

        public SynchronizedCollection<Port> GetPorts(IPAddress ip)
        {
            return new SynchronizedCollection<Port>(ports.Where(P => P.ip.Equals(ip)).ToList());
        }

        #endregion

        public override string ToString()
        {
            if (computerName == string.Empty)
            {
                return physicalAddress.ToString();
            }
            else
            {
                return physicalAddress.ToString() + " (" + computerName + ")";
            }
        }

        public override bool Equals(object obj)
        {
            Neighbor n = obj as Neighbor;
            return this.physicalAddress.Equals(n.physicalAddress);
        }

        public override int GetHashCode()
        {
            return this.physicalAddress.GetHashCode();
        }


        public void Combine(Neighbor n)
        {
            foreach (var ip in n.ips)
                this.AddIP(ip);

            foreach (var port in n.ports)
                this.AddPort(port);

            if (string.IsNullOrEmpty(this.computerName))
                this.computerName = n.computerName;
            else if (!this.computerName.Equals(n.computerName) && !string.IsNullOrEmpty(n.computerName))
                this.computerName = string.Format("{0} - {1})", this.computerName, n.computerName);
        }
    }
}
