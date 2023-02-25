namespace Assets.Sources.Models.Base
{
    public readonly struct BattleResultSources
    {
        public BattleResultSources(bool isWin, long experience, int gold, int rank)
        {
            IsCharacterWin = isWin;
            AddExperience = experience;
            AddGold = gold;
            AddRank = rank;
        }

        public readonly bool IsCharacterWin;
        public readonly long AddExperience;
        public readonly int AddGold;
        public readonly int AddRank;
    }
}