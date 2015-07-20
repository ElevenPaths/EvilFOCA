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
using PacketDotNet;

namespace evilfoca.Core
{
    public static class OSDetector
    {
        public static void AnalyzePacket(Packet p)
        {
            if (p == null)
                return;

            if (
                    (p.PayloadPacket != null && p.PayloadPacket is IPv6Packet)
                        &&
                    (p.PayloadPacket.PayloadPacket != null && p.PayloadPacket.PayloadPacket is ICMPv6Packet)
                )
            {
                AnalyzeICMPv6Packet(p);
            }
        }

        private static void AnalyzeICMPv6Packet(Packet packet)
        {
            if (!(packet.PayloadPacket.PayloadPacket is ICMPv6Packet))
                return;

            try
            {
                ICMPv6Types type = ((ICMPv6Packet)packet.PayloadPacket.PayloadPacket).Type;
                if (type == ICMPv6Types.NeighborAdvertisement)
                {
                    ushort payloadLen = ((IPv6Packet)packet.PayloadPacket).PayloadLength;
                    byte[] flags = new byte[4];

                    flags[0] = (packet.PayloadPacket.PayloadPacket).Bytes[4];
                    flags[1] = (packet.PayloadPacket.PayloadPacket).Bytes[5];
                    flags[2] = (packet.PayloadPacket.PayloadPacket).Bytes[6];
                    flags[3] = (packet.PayloadPacket.PayloadPacket).Bytes[7];

                    Data.Neighbor neighbor = Program.CurrentProject.data.GetNeighbor(((EthernetPacket)packet).SourceHwAddress);
                    if (neighbor != null)
                    {
                        // Si el payload UDP es 24 bytes, y los flags son 0x40000000 (sol) puede ser un linux, y no un windows (0x60000000, sol ovr).
                        if ((flags[0] == 0x40) && (flags[1] == 0x00) && (flags[2] == 0x00) && (flags[3] == 0x00))
                        {
                            // Linux
                            if (neighbor.osPlatform == Data.Platform.Unknow)
                                neighbor.osPlatform = Data.Platform.Linux;
                        }
                        else if ((flags[0] == 0x60) && (flags[1] == 0x00) && (flags[2] == 0x00) && (flags[3] == 0x00))
                        {
                            // Windows
                            if (neighbor.osPlatform == Data.Platform.Unknow)
                                neighbor.osPlatform = Data.Platform.Windows;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
}
