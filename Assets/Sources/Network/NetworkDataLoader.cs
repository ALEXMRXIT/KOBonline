using System;

namespace Assets.Sources.Network
{
    public sealed class NetworkDataLoader
    {
        private int _countPackets;
        private int _loadsPackets;

        public void SetCount(int countPackets)
        {
            _countPackets = countPackets;
            _loadsPackets = 0;
        }

        public void Increment()
        {
            _loadsPackets++;
        }

        public bool IsDataLoader()
        {
            return _loadsPackets == _countPackets;
        }

        public void Reset()
        {
            _countPackets = 0;
            _loadsPackets = 0;
        }
    }
}