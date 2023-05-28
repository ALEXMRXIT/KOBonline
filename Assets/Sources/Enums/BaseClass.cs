namespace Assets.Sources.Enums
{
    public enum BaseClass : int
    {
        Warrior = 0,
        Mage = 1
    }

    public enum SkillType : byte
    {
        Active,
        Passive
    }

    public enum SkillUseType : byte
    {
        Target,
        Self
    }

    public enum SkillDamageType : byte
    {
        PhysicDamage,
        MagicDamage
    }
}