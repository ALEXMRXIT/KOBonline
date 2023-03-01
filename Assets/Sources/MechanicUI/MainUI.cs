using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;

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
        [SerializeField] private Text _rankValue;
        [SerializeField] private Image _iconRank;

        [Space]
        [SerializeField] private Sprite[] _ranksSprite;

        public static MainUI Instance;

        private INetworkProcessor _networkProcessor;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;
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
            _rankValue.text = $"{playerContract.PlayerRank}";

            _iconRank.sprite = _ranksSprite[_networkProcessor.GetParentObject().GetRank.GetIndexByRankTable(playerContract.PlayerRank)];
        }
    }
}