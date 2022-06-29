using System;
using System.Text;

namespace i3PackDx.Tools
{
    public class Reader
    {
        public byte[] buffer;
        public int offset = 0;

        public Reader(byte[] buffer)
        {
            this.buffer = buffer;
        }

        public long GetCurrentPostion() => offset;

        public byte ReadByte() => buffer[offset++];

        public byte[] ReadBytes(int Length)
        {
            byte[] data = new byte[Length];
            Array.Copy(buffer, offset, data, 0, Length);
            offset += Length;
            return data;
        }

        public short ReadShort()
        {
            short data = BitConverter.ToInt16(buffer, offset);
            offset += 2;
            return data;
        }

        public ushort ReadUShort()
        {
            ushort data = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
            return data;
        }

        public int ReadInt()
        {
            int data = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            return data;
        }

        public uint ReadUInt()
        {
            uint data = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            return data;
        }

        public double ReadDouble()
        {
            double data = BitConverter.ToDouble(buffer, offset);
            offset += 8;
            return data;
        }

        public float ReadFloat()
        {
            float data = BitConverter.ToSingle(buffer, offset);
            offset += 4;
            return data;
        }

        public long ReadLong()
        {
            long data = BitConverter.ToInt64(buffer, offset);
            offset += 8;
            return data;
        }

        public ulong ReadULong()
        {
            ulong data = BitConverter.ToUInt64(buffer, offset);
            offset += 8;
            return data;
        }

        public string ReadString(int Length)
        {
            string data = "";
            try
            {
                data = Encoding.GetEncoding(Initial.Encoding).GetString(buffer, offset, Length);
                int length = data.IndexOf((char)0);
                if (length != -1)
                    data = data.Substring(0, length);
                offset += Length;
            }
            catch
            {
            }
            return data;
        }

        public string ReadString()
        {
            string data = "";
            try
            {
                data = Encoding.Unicode.GetString(buffer, offset, (buffer.Length - offset));
                int idx = data.IndexOf(char.MinValue);
                if (idx != -1)
                    data = data.Substring(0, idx);
                data += data.Length + 1;
            }
            catch
            {
            }
            return data;
        }
    }
}