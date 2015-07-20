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

namespace evilfoca.ICMPv6
{
    //Packet Format:
    //int Flags;
    //byte[16] TargetAddress
    //options
    // 0x02 link layer
    // 0x01 Length 1, 8 bytes
    // MAC
    public class NeighborAdvertisement
    {
        const int Flags = 0x60000000;   //Solicitation, Override
        byte[] OptionMAC = new byte[] { 0x02, 0x01 };

        private IPAddress TargetAddress;
        private PhysicalAddress MAC;

        public NeighborAdvertisement(IPAddress TargetAddress, PhysicalAddress MAC)
        {
            this.TargetAddress = TargetAddress;
            this.MAC = MAC;
        }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[32];
            int pos = 4; //Los cuatro primeros bytes son del checksum
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(Flags)), 0, buffer, pos, sizeof(int));
            pos += 4;
            Buffer.BlockCopy(TargetAddress.GetAddressBytes(), 0, buffer, pos, TargetAddress.GetAddressBytes().Length);
            pos += TargetAddress.GetAddressBytes().Length;
            Buffer.BlockCopy(OptionMAC, 0, buffer, pos, OptionMAC.Length);
            pos += OptionMAC.Length;
            Buffer.BlockCopy(MAC.GetAddressBytes(), 0, buffer, pos, MAC.GetAddressBytes().Length);
            return buffer;
        }
    }
}
