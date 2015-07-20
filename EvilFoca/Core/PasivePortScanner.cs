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
using evilfoca.Data;

namespace evilfoca.Core
{
    class PasivePortScanner
    {
        IList<Data.Neighbor> neighbors;

        public PasivePortScanner(IList<Data.Neighbor> neighbors)
        {
            this.neighbors = neighbors;
        }

        public void AnalyzePacket(Packet p)
        {
            if (p == null)
                return;
            if (p.PayloadPacket == null)
                return;
            if (p.PayloadPacket.PayloadPacket == null)
                return;

            if (p.PayloadPacket is IpPacket)
            {
                if (p.PayloadPacket.PayloadPacket is TcpPacket)
                    AnalyzeTCP(p);
                else if (p.PayloadPacket.PayloadPacket is UdpPacket)
                    AnalyzeUDP(p);
            }
        }

        private void AnalyzeTCP(Packet p)
        {
            if (!(p.PayloadPacket.PayloadPacket is TcpPacket))
                return;

            EthernetPacket ethernet = (EthernetPacket)p;
            IpPacket ip = (IpPacket)p.PayloadPacket;
            TcpPacket tcp = (TcpPacket)p.PayloadPacket.PayloadPacket;
            Neighbor neighbor = null;

            if (tcp.Syn && tcp.Ack)
            {
                neighbor = Program.CurrentProject.data.GetNeighbor(ethernet.SourceHwAddress);
                if (neighbor == null)
                    return;

                if (neighbor.ExistsIP(ip.SourceAddress))
                {
                    Port port = new Port(tcp.SourcePort, Protocol.TCP, ip.SourceAddress);
                    if (!(neighbor.ExistsPort(port)))
                    {
                        neighbor.AddPort(port);
                        Program.CurrentProject.data.AddNeighbor(neighbor);
                    }
                }
                else
                    return;
            }
        }

        private void AnalyzeUDP(Packet p)
        {
            // No implementado
        }
    }
}
