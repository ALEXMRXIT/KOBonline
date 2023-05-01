using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class ChangeSession
    {
        private readonly static byte _opcode = 0x07;

        public static NetworkPacket ToPacket(SessionStatus sessionStatus)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)sessionStatus);

            return packet;
        }
    }
}