using Assets.Sources.Models;

namespace Assets.Sources.Network
{
    public abstract class NetworkBasePacket
    {
        public abstract PacketImplementCodeResult RunImpl();
    }
}