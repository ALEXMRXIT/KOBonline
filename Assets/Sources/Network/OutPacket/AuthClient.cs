using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class AuthClient
    {
        private readonly static byte _opcode = 0x00;

        public static NetworkPacket AuthToPacket(string login,
            string password, AuthType authType)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)authType);
            packet.WriteString(login);
            packet.WriteString(password);

            return packet;
        }
    }
}