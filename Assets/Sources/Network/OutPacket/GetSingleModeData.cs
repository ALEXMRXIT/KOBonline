using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class GetSingleModeData
    {
        private readonly static byte _opcode = 0x18;

        public static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            return packet;
        }
    }
}