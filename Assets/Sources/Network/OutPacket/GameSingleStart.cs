using System;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class GameSingleStart
    {
        private readonly static byte _opcode = 0x19;

        public static NetworkPacket ToPacket(int index)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteInt(index);

            return packet;
        }
    }
}