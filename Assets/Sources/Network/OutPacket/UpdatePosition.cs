using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Network;

namespace Assets.Sources.Network.OutPacket
{
    public static class UpdatePosition
    {
        private readonly static byte _opcode = 0x06;

        public static NetworkPacket ToPacket(Vector3 vector)
        {
            NetworkPacket packet = new NetworkPacket(_opcode);

            packet.WriteFloat(vector.x);
            packet.WriteFloat(vector.y);
            packet.WriteFloat(vector.z);

            return packet;
        }
    }
}