using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class TakeItemFromPresentWinner
    {
        private readonly static byte _opcode = 0x10;

        public static NetworkPacket ToPacket(byte servceType, long itemId, int typePresent)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte(servceType);
            packet.WriteLong(itemId);
            packet.WriteInt(typePresent);

            return packet;
        }
    }
}