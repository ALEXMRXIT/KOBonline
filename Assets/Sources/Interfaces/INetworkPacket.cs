using System;

namespace Assets.Sources.Interfaces
{
    public interface INetworkPacket
    {
        public unsafe void WriteByte(byte value);
        public void WriteByte(params byte[] value);
        public void WriteByteArray(byte[] values);
        public unsafe void WriteShort(short value);
        public unsafe void WriteShort(params short[] values);
        public unsafe void WriteInt(int value);
        public unsafe void WriteInt(params int[] values);
        public unsafe void WriteIntArray(int[] values);
        public unsafe void WriteDouble(double values);
        public unsafe void WriteFloat(float value);
        public unsafe void WriteDouble(params double[] values);
        public unsafe void WriteLong(long value);
        public unsafe void WriteLong(params long[] values);
        public unsafe void WriteString(string value);
        public void InternalWriteBool(bool value);
        public void InternalWriteDateTime(DateTime value);
        public unsafe byte ReadByte();
        public unsafe byte[] ReadBytesArray(int length);
        public byte[] ReadByteArrayAlt(int length);
        public unsafe short ReadShort();
        public unsafe int ReadInt();
        public unsafe double ReadDouble();
        public unsafe float ReadFloat();
        public unsafe long ReadLong();
        public unsafe string ReadString();
        public bool InternalReadBool();
        public DateTime InternalReadDateTime();
        public unsafe void Prepare(int headerSize);
        public byte[] GetBuffer();
        public byte[] GetBuffer(int skipFirstBytesCount);
        public void MoveOffset(int size);
        public unsafe byte FirstOpcode { get; }
        public unsafe int SecondOpcode { get; }
        public int Length { get; }
    }
}