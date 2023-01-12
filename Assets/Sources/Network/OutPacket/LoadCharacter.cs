using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class LoadCharacter
    {
        private readonly static byte _opcode = 0x01;

        public static NetworkPacket ToPacket()
        {
            return new NetworkPacket(_opcode);
        }
    }
}