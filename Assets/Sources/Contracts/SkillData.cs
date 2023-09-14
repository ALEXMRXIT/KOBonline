using Assets.Sources.Enums;

namespace Assets.Sources.Contracts
{
    public sealed class SkillData
    {
        public long SkillId { get; set; }
        public long CharacterId { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int SlotId { get; set; }
        public bool WorksInNonCombat { get; set; }
    }
}