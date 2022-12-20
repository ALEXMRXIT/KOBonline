using Assets.Sources.Models;

namespace Assets.Sources.Interfaces
{
    public interface IPacketHandlerImplementation
    {
        public PacketImplementCodeResult ExecuteImplement(INetworkPacket networkPacket);
    }
}