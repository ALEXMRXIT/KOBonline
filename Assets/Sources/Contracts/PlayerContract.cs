using Assets.Sources.Enums;

namespace Assets.Sources.Contracts
{
    public sealed class PlayerContract
    {
        public string AccountName { get; set; }
        public long ObjId { get; set; }
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public PlayerSex Sex { get; set; }
        public int PlayerRank { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Endurance { get; set; }
        public long Experience { get; set; }
        public BaseClass CharacterBaseClass { get; set; }
    }
}