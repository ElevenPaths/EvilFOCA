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
using PacketDotNet;

namespace evilfoca.HTTP
{
    public class TcpReconstructorPacket
    {
        public int Length { get; set; }
        public byte[] Data { get; set; }
        public uint FirstSequenceNumber { get; set; }
        public uint LastSequenceNumber { get; set; }
        public DateTime CreationTime { get; set; }

        public uint ExpectedSequenceNumber
        {
            get
            {
                return FirstSequenceNumber + (uint)Length;
            }
        }


        public TcpReconstructorPacket(TcpPacket packet)
        {
            if (packet != null)
            {
                this.Data = packet.PayloadData;
                this.FirstSequenceNumber = this.LastSequenceNumber = packet.SequenceNumber;
                this.Length = this.Data.Length;
            }
            CreationTime = DateTime.Now;
        }


        private void InsertDataFirstPosition(byte[] newData)
        {
            if (newData != null && newData.Length > 0)
            {
                if (this.Data == null || this.Data.Length == 0)
                    this.Data = newData;
                else
                {
                    byte[] tempData = new byte[this.Data.Length + newData.Length];
                    Array.Copy(newData, tempData, newData.Length);
                    Array.Copy(this.Data, 0, tempData, newData.Length, this.Data.Length);
                    this.Data = tempData;
                    this.Length = this.Data.Length;
                }
            }
        }

        private void AppendData(byte[] newData)
        {
            if (newData != null && newData.Length > 0)
            {
                if (this.Data == null || this.Data.Length == 0)
                    this.Data = newData;
                else
                {
                    byte[] tempData = new byte[this.Data.Length + newData.Length];
                    Array.Copy(this.Data, tempData, this.Data.Length);
                    Array.Copy(newData, 0, tempData, this.Data.Length, newData.Length);
                    this.Data = tempData;
                    this.Length = this.Data.Length;
                }
            }
        }

        public void InsertPreviousTcpPacket(TcpPacket newPacket)
        {
            if (newPacket != null)
            {
                this.FirstSequenceNumber = newPacket.SequenceNumber;
                InsertDataFirstPosition(newPacket.PayloadData);
            }
            CreationTime = DateTime.Now;
        }

        public void AppendTcpPacket(TcpPacket newPacket)
        {
            if (newPacket != null)
            {
                this.LastSequenceNumber = newPacket.SequenceNumber;
                AppendData(newPacket.PayloadData);
            }
            CreationTime = DateTime.Now;
        }

        public void InsertPreviousTcpPacket(TcpReconstructorPacket newPacket)
        {
            if (newPacket != null)
            {
                this.FirstSequenceNumber = newPacket.FirstSequenceNumber;
                InsertDataFirstPosition(newPacket.Data);
            }
            CreationTime = DateTime.Now;
        }

        public void AppendTcpPacket(TcpReconstructorPacket newPacket)
        {
            if (newPacket != null)
            {
                this.LastSequenceNumber = newPacket.LastSequenceNumber;
                AppendData(newPacket.Data);
            }
            CreationTime = DateTime.Now;
        }
    }
}
