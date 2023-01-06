using System;

namespace Assets.Sources.Tools
{
    public static class MemoryBuffer
    {
        public static unsafe byte[] Copy(byte[] source, int srcOffset, byte[] destination, int destOffset, int size)
        {
            fixed (byte* src = source, dst = destination)
                Copy(src, srcOffset, dst, destOffset, size);

            return destination;
        }

        public static unsafe void Copy(byte* src, int srcOffset, byte* dst, int dstOffset, int size)
        {
            int index = 0;

            src += srcOffset;
            dst += dstOffset;

            while ((size - index) >= sizeof(long))
            {
                *(long*)(dst + index) = *(long*)(src + index);
                index += sizeof(long);
            }

            while ((size - index) >= sizeof(int))
            {
                *(int*)(dst + index) = *(int*)(src + index);
                index += sizeof(int);
            }

            while ((size - index) >= sizeof(short))
            {
                *(short*)(dst + index) = *(short*)(src + index);
                index += sizeof(short);
            }

            while (index < size)
                *(dst + index) = *(src + index++);
        }

        public static unsafe void Copy(byte* w, int size, byte* dst, ref int offset)
        {
            int index = 0;

            dst += offset;
            offset += size;

            while ((size - index) >= sizeof(long))
            {
                *(long*)(dst + index) = *(long*)(w + index);
                index += sizeof(long);
            }

            while ((size - index) >= sizeof(int))
            {
                *(int*)(dst + index) = *(int*)(w + index);
                index += sizeof(int);
            }

            while ((size - index) >= sizeof(short))
            {
                *(short*)(dst + index) = *(short*)(w + index);
                index += sizeof(short);
            }

            while (index < size)
                *(dst + index) = *(w + index++);
        }

        public static unsafe void UnsafeCopy(short* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        public static unsafe short[] SpecialCopy(short* src, int length)
        {
            short[] destiny = new short[length];

            fixed (short* dst = destiny)
            {
                int index = 0;

                while ((length - index) >= (sizeof(long) / sizeof(short)))
                {
                    *(long*)(dst + index) = *(long*)(src + index);
                    index += sizeof(long) / sizeof(short);
                }

                while ((length - index) >= (sizeof(int) / sizeof(short)))
                {
                    *(int*)(dst + index) = *(int*)(src + index);
                    index += sizeof(int) / sizeof(short);
                }

                while ((length - index) > 0)
                    *(dst + index) = *(src + index++);
            }

            return destiny;
        }

        public static unsafe void UnsafeCopy(int* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        public static unsafe void UnsafeCopy(double* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        public static unsafe void UnsafeCopy(long* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        public static unsafe void UnsafeCopy(char* w, int size, byte* dst, ref int offset)
        {
            Copy((byte*)w, size, dst, ref offset);
        }

        public static byte[] Cut(byte[] source, int startIndex, int size)
        {
            return Copy(source, startIndex, new byte[size], 0, size);
        }

        public static byte[] Replace(byte[] buffer, int index, byte[] replacement, int size)
        {
            return Copy(replacement, 0, buffer, index, size);
        }

        public static byte[] Extend(ref byte[] source, int sourceIndex, int neededLength)
        {
            return source = Copy(source, 0, new byte[neededLength], sourceIndex, neededLength);
        }

        public static byte[] Extend(ref byte[] source, int additionalLength)
        {
            return source = Copy(source, 0, new byte[source.Length + additionalLength], 0, source.Length);
        }

        public static unsafe byte[] InsertIndexBuffer(byte[] buffer, int index, byte value)
        {
            fixed (byte* ptrBuffer = buffer)
                *(ptrBuffer + index) = value;

            return buffer;
        }

        public static string ToString(byte[] buffer)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Buffer dump, length: {0}{1}Index   |---------------------------------------------|  |--------------|{1}", buffer.Length, Environment.NewLine);

            int index = 0;

            while (index < buffer.Length)
            {
                string hex = string.Empty,
                       data = string.Empty;

                int i;
                for (i = 0; (i < 16) && ((index + i) < buffer.Length); i++)
                {
                    hex += $"{buffer[index + i]:X2} ";

                    if ((buffer[i + index] > 31) && (buffer[i + index] < 127))
                        data += (char)buffer[i + index];
                    else
                        data += ".";
                }

                while (i < 16)
                {
                    hex += "   ";
                    i++;
                }

                sb.Append($"{index.ToString("X5")}   {hex} {data}{Environment.NewLine}");
                index += 16;
            }

            sb.Append("        |---------------------------------------------|  |--------------|");

            return sb.ToString();
        }

        public static unsafe void GetTrimmedString(byte* src, int srcOffset, ref string dst, int bytesCount)
        {
            bytesCount = srcOffset + bytesCount;

            while ((srcOffset < bytesCount) && (src[srcOffset] != 0))
                dst += (char)src[srcOffset++];
        }

        public static unsafe string GetTrimmedString(byte* src, ref int srcOffset, int maxLength)
        {
            string dst = string.Empty;

            while ((src[srcOffset] != 0) && ((srcOffset + sizeof(char)) < maxLength))
            {
                dst += (char)src[srcOffset];
                srcOffset += sizeof(char);
            }

            srcOffset += sizeof(char);

            return dst;
        }

        public static TU[] Copy<TU>(TU[] source, long srcOffset, TU[] destination, long dstOffset, long length)
        {
            if ((length > (source.Length - srcOffset)) || (length > (destination.Length - dstOffset)))
                throw new InvalidOperationException();

            length += srcOffset;

            while (srcOffset < length)
                destination[dstOffset++] = source[srcOffset++];

            return destination;
        }
    }
}