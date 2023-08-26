using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class RequestUsePresent
    {
        private readonly static byte _opcode = 0x0F;

        public static NetworkPacket ToPacket(int slot)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteInt(slot);

            return packet;
        }
    }
}