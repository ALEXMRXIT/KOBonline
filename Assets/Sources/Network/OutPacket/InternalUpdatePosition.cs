using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class InternalUpdatePosition
    {
        private readonly static byte _opcode = 0x14;

        public static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            return packet;
        }
    }
}