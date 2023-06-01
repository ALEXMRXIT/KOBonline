using Assets.Sources.Enums;

namespace Assets.Sources.Contracts
{
    public sealed class SkillContract
    {
        public long Id;
        public BaseClass Class;
        public string Name;
        public string Description;
        public SkillType TypeSkill;
        public SkillUseType UseType;
        public bool Invoke;
        public long IDInvoke;
        public SkillDamageType DamageType;
        public int Distance;
        public int[] BaseDamage;
        public float[] MultiplyDamage;
        public bool Buff;
        public bool DeBuff;
        public float[] PrecentPhysAtk;
        public float[] PrecentMagAtk;
        public int[] AddPhysAtk;
        public int[] AddMagAtk;
        public float[] PrecentPhysDef;
        public float[] PrecentMagDef;
        public int[] AddPhysDef;
        public int[] AddMagDef;
        public int[] AddStrength;
        public int[] AddAgility;
        public int[] AddIntelligence;
        public int[] AddEndurance;
        public int[] AddLuck;
        public float[] AttackSpeed;
        public float[] AddCriteRate;
        public float[] AddCritDamageMultiply;
        public float[] AddMissRate;
        public float[] AddHitRate;
        public int[] HealthRegeneration;
        public int[] ManaRegeneration;
        public int[] AddHealth;
        public int[] AddMana;
        public int TimeUse;
        public int Recharge;
        public int TimeDelay;
        public int[] Experience;
        public int[] Mana;
    }
}