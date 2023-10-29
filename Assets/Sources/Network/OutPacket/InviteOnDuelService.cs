using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class InviteOnDuelService
    {
        private readonly static byte _opcode = 0x17;

        public static NetworkPacket ToPacket(byte code, string characterName)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            if (string.IsNullOrEmpty(characterName))
                characterName = string.Empty;

            packet.WriteByte(code);
            packet.WriteString(characterName);

            return packet;
        }
    }
}