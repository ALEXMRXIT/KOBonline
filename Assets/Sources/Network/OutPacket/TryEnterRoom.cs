using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class TryEnterRoom
    {
        private readonly static byte _opcode = 0x03;

        public static NetworkPacket ToPacket(GameMode gameMode, bool isEnter)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.InternalWriteBool(isEnter);
            packet.WriteByte((byte)gameMode);

            return packet;
        }
    }
}