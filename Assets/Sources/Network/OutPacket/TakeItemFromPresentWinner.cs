using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class TakeItemFromPresentWinner
    {
        private readonly static byte _opcode = 0x10;

        public static NetworkPacket ToPacket(long itemId)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteLong(itemId);

            return packet;
        }
    }
}