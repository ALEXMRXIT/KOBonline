using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class InternalUpdatePosition
    {
        private readonly static byte _opcode = 0x14;

        public static NetworkPacket ToPacket(float x, float y, float z)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteFloat(x);
            packet.WriteFloat(y);
            packet.WriteFloat(z);

            return packet;
        }
    }
}