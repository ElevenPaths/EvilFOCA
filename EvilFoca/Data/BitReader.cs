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

namespace evilfoca.Data
{
    public class BitReader : IDisposable
    {
        uint readData = 0;
        int startPosition = 0;
        int endPosition = 0;

        internal int InBuffer
        {
            get
            {
                return endPosition - startPosition;
            }
        }

        private Stream stream;
        internal Stream BaseStream
        {
            get { return stream; }
        }

        internal BitReader(Stream stream)
        {
            this.stream = stream;
        }


        void EnsureData(int bitCount)
        {
            int readBits = bitCount - InBuffer;
            while (readBits > 0)
            {
                int b = BaseStream.ReadByte();
                if (b < 0) throw new Exception("Unexpected end of stream");
                readData |= checked((uint)b << endPosition);
                endPosition += 8;
                readBits -= 8;
            }
        }

        internal bool ReadBit()
        {
            return ReadLSB(1) > 0;
        }

        internal int ReadLSB(int bitCount)
        {
            EnsureData(bitCount);

            int result = (int)(readData >> startPosition) & ((1 << bitCount) - 1);
            startPosition += bitCount;

            if (endPosition == startPosition)
            {
                endPosition = startPosition = 0;
                readData = 0;
            }
            else if (startPosition >= 8)
            {
                readData >>= startPosition;
                endPosition -= startPosition;
                startPosition = 0;
            }

            return result;
        }

        internal int ReadMSB(int bitCount)
        {
            int result = 0;

            for (int i = 0; i < bitCount; i++)
            {
                result <<= 1;
                if (ReadBit()) result |= 1;
            }

            return result;
        }

        internal string ReadStringLSB()
        {
            int length = ReadByte();
            List<byte> stringBytes = new List<byte>();
            while (length != 0x00)
            {
                for (int i = 0; i < length; i++)
                {
                    stringBytes.Add((byte)ReadLSB(8));
                };
                length = (byte)ReadByte();
                if (length != 0x00)
                    stringBytes.Add(0x2E);
            }

            return System.Text.Encoding.ASCII.GetString(stringBytes.ToArray());
        }

        internal int ReadByte()
        {
            return ReadLSB(8);
        }

        internal void Align()
        {
            endPosition = startPosition = 0;
            readData = 0;
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
