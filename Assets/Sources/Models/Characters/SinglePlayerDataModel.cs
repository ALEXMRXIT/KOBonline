using System;

namespace Assets.Sources.Models
{
    public sealed class SinglePlayerDataModel
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public long Time { get; set; }
        public int Crowns { get; set; }
        public int Experience { get; set; }
    }
}