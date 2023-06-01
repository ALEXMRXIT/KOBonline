using Assets.Sources.Enums;

namespace Assets.Sources.Network.OutPacket
{
    public static class SendUpgradeSkill
    {
        private readonly static byte _opcode = 0x0A;

        public static NetworkPacket ToPacket(long skillId, bool newSlot = false, int slotId = -1)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteLong(skillId);

            packet.InternalWriteBool(newSlot);
            if (newSlot)
                packet.WriteInt(slotId);

            return packet;
        }
    }
}