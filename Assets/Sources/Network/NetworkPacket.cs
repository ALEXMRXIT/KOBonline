using System;
using System.Text;
using Assets.Sources.Tools;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Network
{
    public struct NetworkPacket
    {
        private const int DefaultOverflowValue = 128;
        private byte[] _buffer;
        private int _offset;
        private readonly bool _receivedPacket;

        public NetworkPacket(int headerOffset, byte[] buffer)
        {
            _receivedPacket = true;
            _buffer = buffer;
            _offset = headerOffset;
        }

        public NetworkPacket(params byte[] opcodes)
        {
            _receivedPacket = false;
            _buffer = opcodes;
            _offset = opcodes.Length;
        }

        public unsafe void WriteByte(byte value)
        {
            ValidateBufferSize(sizeof(byte));

            fixed (byte* buffer = _buffer)
                *(buffer + _offset++) = value;
        }

        public void WriteByte(params byte[] value)
        {
            WriteByteArray(value);
        }

        public void WriteByteArray(byte[] values)
        {
            int length = values.Length;

            ValidateBufferSize(length);

            MemoryBuffer.Copy(values, 0, _buffer, _offset, length);
            _offset += length;
        }

        public unsafe void WriteShort(short value)
        {
            ValidateBufferSize(sizeof(short));

            fixed (byte* buffer = _buffer)
                *(short*)(buffer + _offset) = value;

            _offset += sizeof(short);
        }

        public unsafe void WriteShort(params short[] values)
        {
            int length = values.Length * sizeof(short);

            ValidateBufferSize(length);

            fixed (byte* buffer = _buffer)
            {
                fixed (short* ptrValues = values)
                    MemoryBuffer.UnsafeCopy(ptrValues, length, buffer, ref _offset);
            }
        }

        public unsafe void WriteInt(int value)
        {
            ValidateBufferSize(sizeof(int));

            fixed (byte* buffer = _buffer)
                *(int*)(buffer + _offset) = value;

            _offset += sizeof(int);
        }

        public unsafe void WriteInt(params int[] values)
        {
            int length = values.Length * sizeof(int);

            ValidateBufferSize(Length);

            fixed (byte* buffer = _buffer)
            {
                fixed (int* ptrValues = values)
                    MemoryBuffer.UnsafeCopy(ptrValues, length, buffer, ref _offset);
            }
        }

        public unsafe void WriteIntArray(int[] values)
        {
            int length = values.Length * sizeof(int);

            ValidateBufferSize(Length);

            fixed (byte* buffer = _buffer)
            {
                fixed (int* ptrValues = values)
                    MemoryBuffer.UnsafeCopy(ptrValues, length, buffer, ref _offset);
            }
        }

        public unsafe void WriteDouble(double values)
        {
            ValidateBufferSize(sizeof(double));

            fixed (byte* buffer = _buffer)
                *(double*)(buffer + _offset) = values;

            _offset += sizeof(double);
        }

        public unsafe void WriteFloat(float value)
        {
            ValidateBufferSize(sizeof(float));

            fixed (byte* buffer = _buffer)
                *(float*)(buffer + _offset) = value;

            _offset += sizeof(float);
        }

        public unsafe void WriteDouble(params double[] values)
        {
            int length = values.Length * sizeof(double);

            ValidateBufferSize(length);

            fixed (byte* buffer = _buffer)
            {
                fixed (double* ptrValues = values)
                    MemoryBuffer.UnsafeCopy(ptrValues, length, buffer, ref _offset);
            }
        }

        public unsafe void WriteLong(long value)
        {
            ValidateBufferSize(sizeof(long));

            fixed (byte* buffer = _buffer)
                *(long*)(buffer + _offset) = value;

            _offset += sizeof(long);
        }

        public unsafe void WriteLong(params long[] values)
        {
            int length = values.Length * sizeof(long);

            ValidateBufferSize(length);

            fixed (byte* buffer = _buffer)
            {
                fixed (long* ptrValue = values)
                    MemoryBuffer.UnsafeCopy(ptrValue, length, buffer, ref _offset);
            }
        }

        public unsafe void WriteString(string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);

            WriteInt(buffer.Length);
            WriteByteArray(buffer);
        }

        public void InternalWriteBool(bool value)
        {
            WriteByte(value ? (byte)0x01 : (byte)0x00);
        }

        public void InternalWriteDateTime(DateTime value)
        {
            WriteLong(value.Ticks);
        }

        public unsafe byte ReadByte()
        {
            fixed (byte* buffer = _buffer)
                return *(buffer + _offset++);
        }

        public unsafe byte[] ReadBytesArray(int length)
        {
            byte[] destination = new byte[length];

            fixed (byte* buffer = _buffer, ptrDestination = destination)
                MemoryBuffer.Copy(buffer, length, ptrDestination, ref _offset);

            return destination;
        }

        public byte[] ReadByteArrayAlt(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(GetBuffer(), _offset, result, 0, length);
            _offset += length;
            return result;
        }

        public unsafe short ReadShort()
        {
            fixed (byte* buffer = _buffer)
            {
                short value = *(short*)(buffer + _offset);
                _offset += sizeof(short);

                return value;
            }
        }

        public unsafe int ReadInt()
        {
            fixed (byte* buffer = _buffer)
            {
                int value = *(int*)(buffer + _offset);
                _offset += sizeof(int);

                return value;
            }
        }

        public unsafe double ReadDouble()
        {
            fixed (byte* buffer = _buffer)
            {
                double value = *(double*)(buffer + _offset);
                _offset += sizeof(double);

                return value;
            }
        }

        public unsafe float ReadFloat()
        {
            fixed (byte* buffer = _buffer)
            {
                float value = *(float*)(buffer + _offset);
                _offset += sizeof(float);

                return value;
            }
        }

        public unsafe long ReadLong()
        {
            fixed (byte* buffer = _buffer)
            {
                long value = *(long*)(buffer + _offset);
                _offset += sizeof(long);

                return value;
            }
        }

        public unsafe string ReadString()
        {
            byte[] buffer = new byte[ReadInt()];

            for (int iterator = 0; iterator < buffer.Length; iterator++)
                buffer[iterator] = ReadByte();

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        public bool InternalReadBool()
        {
            return ReadByte() == 0x01;
        }

        public DateTime InternalReadDateTime()
        {
            return new DateTime(ReadLong());
        }

        private void ValidateBufferSize(int nextValueLength)
        {
            if ((_offset + nextValueLength) > _buffer.Length)
                MemoryBuffer.Extend(ref _buffer, nextValueLength + DefaultOverflowValue);
        }

        public unsafe void Prepare(int headerSize)
        {
            _offset += headerSize;

            MemoryBuffer.Extend(ref _buffer, headerSize, _offset);

            fixed (byte* buffer = _buffer)
            {
                if (headerSize == sizeof(short))
                    *(short*)buffer = (short)_offset;
                else
                    *(int*)buffer = _offset;
            }
        }

        public byte[] GetBuffer()
        {
            return _buffer;
        }

        public byte[] GetBuffer(int skipFirstBytesCount)
        {
            return MemoryBuffer.Copy(_buffer, skipFirstBytesCount,
                new byte[_buffer.Length - skipFirstBytesCount], 0, _buffer.Length - skipFirstBytesCount);
        }

        public void MoveOffset(int size)
        {
            _offset += size;
        }

        public unsafe byte FirstOpcode
        {
            get
            {
                fixed (byte* buf = _buffer)
                    return *buf;
            }
        }

        public unsafe int SecondOpcode
        {
            get
            {
                fixed (byte* buf = _buffer)
                    return *(buf + 1);
            }
        }

        public int Length => _receivedPacket ? _buffer.Length : _offset;

        public override string ToString()
        {
            return MemoryBuffer.ToString(_buffer);
        }
    }
}