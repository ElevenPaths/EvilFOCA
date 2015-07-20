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
using System.IO;
using System.Diagnostics;

namespace evilfoca.Data
{
    public class BitWriter : IDisposable
    {
        uint data = 0;
        int dataLength = 0;
        Stream stream;

        internal Stream BaseStream
        {
            get { return stream; }
        }

        internal int BitsToAligment
        {
            get
            {
                return (32 - dataLength) % 8;
            }
        }

        internal BitWriter(Stream stream)
        {
            this.stream = stream;
        }

        internal void WriteBit(bool value)
        {
            WriteLSB(value ? 1 : 0, 1);
        }

        internal void WriteByte(byte value)
        {
            WriteLSB(value, 8);
        }

        internal void WriteInt16(Int16 value)
        {
            WriteLSB(value, 16);
        }

        internal void WriteInt32(Int32 value)
        {
            WriteLSB(value, 32);
        }

        internal void WriteString(string value)
        {
            string[] sp = value.Split('.');
            foreach (var item in sp)
            {
                byte[] array = System.Text.Encoding.ASCII.GetBytes(value);
                WriteByte((byte)array.Length);
                foreach (var i in array)
                    WriteByte(i);
            }

            WriteByte(0x00);
        }

        internal void WriteLSB(int value, int length)
        {
            Debug.Assert(value < 1 << length, "value does not fit in length");

            uint currentData = data | checked((uint)value << dataLength);
            int currentLength = dataLength + length;
            while (currentLength >= 8)
            {
                BaseStream.WriteByte((byte)currentData);
                currentData >>= 8;
                currentLength -= 8;
            }
            data = currentData;
            dataLength = currentLength;
        }

        internal void WriteMSB(int value, int length)
        {
            Debug.Assert(value < 1 << length, "value does not fit in length");

            int reversed = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                reversed <<= 1;
                reversed |= value & 1;
                value >>= 1;
            }
            WriteLSB(reversed, length);
        }

        internal void Align()
        {
            if (dataLength > 0)
            {
                BaseStream.WriteByte((byte)data);
                data = 0;
                dataLength = 0;
            }
        }

        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Flush();
                this.stream.Close();
            }
        }
    }

}
