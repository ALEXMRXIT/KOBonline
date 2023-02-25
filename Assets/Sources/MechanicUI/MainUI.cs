using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;

namespace Assets.Sources.MechanicUI
{
    public sealed class MainUI : MonoBehaviour
    {
        [SerializeField] private Text _characterHealth;
        [SerializeField] private Text _characterMana;
        [SerializeField] private Text _experience;
        [SerializeField] private Slider _fillExperience;
        [SerializeField] private Text _characterLevel;
        [SerializeField] private Text _characterName;

        public static MainUI Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateUI(PlayerContract playerContract)
        {
            _characterHealth.text = $"{playerContract.Health}/{playerContract.Health}";
            _characterMana.text = $"{playerContract.Mana}/{playerContract.Mana}";
            _experience.text = $"{playerContract.Experience}/{playerContract.NextExperience}";
            _fillExperience.value = Mathf.Clamp(playerContract.Experience /
                (float)playerContract.NextExperience, min: 0, max: 1);
            _characterLevel.text = $"{playerContract.Level}";
            _characterName.text = $"{playerContract.CharacterName}";
        }
    }
}