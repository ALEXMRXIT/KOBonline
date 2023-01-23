using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class LoadSceneFightingSuccess
    {
        private readonly static byte _opcode = 0x05;

        public static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            

            return packet;
        }
    }
}