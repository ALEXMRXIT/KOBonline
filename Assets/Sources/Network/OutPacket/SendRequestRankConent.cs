using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class SendRequestRankConent
    {
        private readonly static byte _opcode = 0x0D;

        public static NetworkPacket ToPacket(ContentType contentType)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)contentType);

            return packet;
        }
    }
}