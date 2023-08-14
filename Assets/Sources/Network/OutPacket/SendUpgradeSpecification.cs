using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class SendUpgradeSpecification
    {
        private readonly static byte _opcode = 0x0C;

        public static NetworkPacket ToPacket(Specification specification, byte count)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)specification);
            packet.WriteByte(count);

            return packet;
        }
    }
}