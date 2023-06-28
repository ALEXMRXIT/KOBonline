using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Models.Characters.Skills
{
    public sealed class VisualAbility : MonoBehaviour
    {
        [SerializeField] private Image _iconAbility;
        [SerializeField] private Text _secondsAbility;

        private int _originalTimeUse;
        private bool _status;
        private GameObject _effectAbility;

        public void SetEffectForAbility(GameObject obj) => _effectAbility = obj;

        public void SetAbilityIcon(Sprite sprite) => _iconAbility.sprite = sprite;

        public void SetAbilityText(string text) => _secondsAbility.text = text;

        public void SetOriginalIntTime(int timeUse) => _originalTimeUse = timeUse;

        public bool DecrementTime() => _status = (_originalTimeUse-- == 1 ? false : true);

        public int RemainingRunningTime() => _originalTimeUse;

        public bool AbilitiIfNotDeactivate() => _status;

        public void DestroyEffectForAbility()
        {
            if (_effectAbility != null)
                Destroy(_effectAbility);
        }
    }
}