using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class TryEnterRoom
    {
        private readonly static byte _opcode = 0x03;

        public static NetworkPacket ToPacket(GameMode gameMode)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)gameMode);

            return packet;
        }
    }
}