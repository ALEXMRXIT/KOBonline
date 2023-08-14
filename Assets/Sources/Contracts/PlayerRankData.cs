using Assets.Sources.Enums;

namespace Assets.Sources.Contracts
{
    public sealed class PlayerRankData
    {
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public int PlayerRank { get; set; }
        public int NumberWinners { get; set; }
        public int NumberLosses { get; set; }
    }
}