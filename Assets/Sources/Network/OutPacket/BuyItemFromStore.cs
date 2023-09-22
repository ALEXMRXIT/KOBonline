using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class BuyItemFromStore
    {
        private readonly static byte _opcode = 0x11;

        public static NetworkPacket ToPacket(string id)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteString(id);

            return packet;
        }
    }
}