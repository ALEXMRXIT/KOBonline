using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class ServiceGetPresents
    {
        private readonly static byte _opcode = 0x0E;

        public static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(_opcode);



            return packet;
        }
    }
}