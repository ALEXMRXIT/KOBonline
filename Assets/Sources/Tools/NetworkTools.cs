using System;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Tools
{
    public static class NetworkTools
    {
        public static NetworkPacket BufferToNetworkPacket(this byte[] buffer, int extraBytes = 0)
        {
            return new NetworkPacket(1 + extraBytes, buffer);
        }
    }
}