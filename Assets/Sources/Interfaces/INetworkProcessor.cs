using UnityEngine;
using Assets.Sources.Enums;
using System.Threading.Tasks;
using Assets.Sources.Network;

namespace Assets.Sources.Interfaces
{
    public interface INetworkProcessor
    {
        public bool IsConnected { get; }
        public Task SendPacketAsync(NetworkPacket packet,
            PacketImportance packetImportance = PacketImportance.None);
        public ClientProcessor GetParentObject();
    }
}