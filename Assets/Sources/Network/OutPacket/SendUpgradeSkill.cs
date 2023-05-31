using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class SendUpgradeSkill
    {
        private readonly static byte _opcode = 0x0A;

        public static NetworkPacket ToPacket(int skillId)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteInt(skillId);

            return packet;
        }
    }
}