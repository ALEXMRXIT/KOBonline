using System;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class CreateCharacter
    {
        private readonly static byte _opcode = 0x02;

        public static NetworkPacket ToPacket(string mame, PlayerSex playerSex, BaseClass baseClass)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteString(mame);
            packet.WriteInt((int)playerSex);
            packet.WriteInt((int)baseClass);

            return packet;
        }
    }
}