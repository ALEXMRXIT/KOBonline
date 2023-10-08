using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class OnValueChangeToggleChannel
    {
        private readonly static byte _opcode = 0x16;

        public static NetworkPacket ToPacket(Channel channel, bool isOn)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteByte((byte)channel);
            packet.InternalWriteBool(isOn);

            return packet;
        }
    }
}