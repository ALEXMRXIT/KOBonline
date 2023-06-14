using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class UseSkill
    {
        private readonly static byte _opcode = 0x0B;

        public static NetworkPacket ToPacket(long _skillId)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteLong(_skillId);

            return packet;
        }
    }
}