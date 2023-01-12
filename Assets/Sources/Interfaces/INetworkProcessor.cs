using Assets.Sources.Enums;
using System.Threading.Tasks;
using Assets.Sources.Network;

public interface INetworkProcessor
{
    public bool IsConnected { get; }
    public Task SendPacketAsync(NetworkPacket packet,
        PacketImportance packetImportance = PacketImportance.None);
}