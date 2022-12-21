using System.Threading.Tasks;
using Assets.Sources.Network;

public interface INetworkProcessor
{
    public Task SendPacketAsync(NetworkPacket packet);
}