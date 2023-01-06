using Assets.Sources.Models;
using Assets.Sources.Network;

namespace Assets.Sources.Interfaces
{
    public interface IPacketHandlerImplementation
    {
        public PacketImplementCodeResult ExecuteImplement
            (NetworkPacket networkPacket, ClientProcessor clientProcessor);
    }
}