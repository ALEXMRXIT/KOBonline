using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;

namespace Assets.Sources.MechanicUI
{
    public sealed class MainUI : MonoBehaviour
    {
        [SerializeField] private Text _characterHealth;
        [SerializeField] private Text _characterMana;
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
            _characterLevel.text = $"{playerContract.Level}";
            _characterName.text = $"{playerContract.CharacterName}";
        }
    }
}