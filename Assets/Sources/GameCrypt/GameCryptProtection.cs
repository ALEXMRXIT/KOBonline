using System.Text;

#pragma warning disable

namespace Assets.Sources.GameCrypt
{
    public sealed class GameCryptProtection
    {
        public static byte[] KEY_CRYPT = new byte[16] { 45, 106, 43, 86, 53, 75, 78, 11, 54, 86, 32, 42, 79, 110, 99, 65 };

        public void EncryptBuffer(byte[] key, byte[] buffer)
        {
            uint tempValue = 0;
            for (int index = 0; index < buffer.Length; index++)
            {
                tempValue = (buffer[index] & (uint)byte.MaxValue) ^ key[(index & 15)] ^ tempValue;
                buffer[index] = (byte)tempValue;
            }
        }

        public void DecryptBuffer(byte[] key, byte[] buffer)
        {
            uint tempValue = 0;
            for (int index = 0; index < buffer.Length; ++index)
            {
                uint decodecUInt64 = buffer[index] & (uint)byte.MaxValue;
                buffer[index] = (byte)(decodecUInt64 ^ key[index & 15] ^ tempValue);
                tempValue = decodecUInt64;
            }
        }
    }
}