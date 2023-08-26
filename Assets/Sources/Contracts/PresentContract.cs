using System;

namespace Assets.Sources.Contracts
{
    public sealed class PresentContract
    {
        public int PresentType { get; set; }
        public long ObjId { get; set; }
        public long Time { get; set; }
        public int Slot { get; set; }
        public int CostOfOneSecondGift { get; set; }
    }
}