using UnityEngine;
using Assets.Sources.UI.Models;

namespace Assets.Sources.UI
{
    public sealed class SlotBattle : MonoBehaviour
    {
        private SkillBattle _skillBattle;

        public void SetSkill(SkillBattle skillBattle)
        {
            _skillBattle = skillBattle;
        }

        public SkillBattle GetSkillSlot()
        {
            return _skillBattle;
        }
    }
}