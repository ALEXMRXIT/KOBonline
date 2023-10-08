using System;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class LoadMessages
    {
        private readonly static byte _opcode = 0x08;

        public static NetworkPacket ToPacket(string characterName)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteInt(0);
            packet.WriteString(characterName);

            return packet;
        }

        public static NetworkPacket ToPacket(Channel channel,
            string characterName, string message, string[] _args = null)
        {
            NetworkPacket packet = new NetworkPacket(0x08);

            packet.WriteInt(1);
            packet.WriteString(characterName);
            packet.WriteByte((byte)channel);
            packet.WriteString(message);

            if (_args != null)
            {
                packet.InternalWriteBool(true);
                packet.WriteInt(_args.Length);

                for (int iterator = 0; iterator < _args.Length; iterator++)
                    packet.WriteString(_args[iterator]);
            }

            return packet;
        }
    }
}