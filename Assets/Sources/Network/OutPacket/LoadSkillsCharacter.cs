using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class LoadSkillsCharacter
    {
        private readonly static byte _opcode = 0x06;

        public static NetworkPacket ToPacket()
        {
            return new NetworkPacket(_opcode);
        }
    }
}