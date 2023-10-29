using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.MechanicUI
{
    public sealed class MainUI : MonoBehaviour
    {
        [SerializeField] private Text _characterHealth;
        [SerializeField] private Slider _characterEnergySlider;
        [SerializeField] private Text _experience;
        [SerializeField] private Slider _fillExperience;
        [SerializeField] private Text _characterLevel;
        [SerializeField] private Text _characterName;
        [SerializeField] private Text _rankValue;
        [SerializeField] private Image _iconRank;
        [Space, SerializeField] private Text _crownGoldText;
        [SerializeField] private Text _crownSilverText;
        [SerializeField] private Text _crownCopperText;
        [SerializeField] private Text _soulCrownGoldText;
        [SerializeField] private Text _soulCrownSilverText;
        [SerializeField] private Text _soulCrownCopperText;
        [SerializeField] private Text _onlineText;
        [SerializeField] private InviteMessageHandler _inviteMessageHandler;

        [Space]
        [SerializeField] private Sprite[] _ranksSprite;

        public static MainUI Instance;

        private INetworkProcessor _networkProcessor;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;
        }

        public void UpdateOnlineView(int count)
        {
            _onlineText.text = $"Online: {count}";
        }

        public void ShowInviteMessageCost(byte code, int energy, string inviteCharacterName)
        {
            _inviteMessageHandler.InviteShowInformationForEnergy(code, energy, inviteCharacterName);
        }
        
        public InviteMessageHandler GetInviteHandlerRef()
        {
            return _inviteMessageHandler;
        }

        public void UpdateUI(PlayerContract playerContract)
        {
            UpdateEnergy(playerContract);
            UpdateExperience(playerContract);
            UpdateLevel(playerContract);
            _characterName.text = $"{playerContract.CharacterName}";
            _rankValue.text = $"{playerContract.PlayerRank}";
            UpdateMoney(playerContract.Crowns, playerContract.SoulCrowns);

            _iconRank.sprite = _ranksSprite[_networkProcessor.GetParentObject().GetRank.GetIndexByRankTable(playerContract.PlayerRank)];
        }

        public void UpdateLevel(PlayerContract playerContract)
        {
            _characterLevel.text = $"Lvl. {playerContract.Level}";
        }

        public void UpdateExperience(PlayerContract playerContract)
        {
            _experience.text = $"{playerContract.Experience}/{playerContract.NextExperience}";
            _fillExperience.value = Mathf.Clamp(playerContract.Experience / (float)playerContract.NextExperience, min: 0, max: 1);
        }

        public void UpdateEnergy(PlayerContract playerContract)
        {
            _characterHealth.text = $"{playerContract.Energy}/100";
            _characterEnergySlider.value = Mathf.Clamp(playerContract.Energy / 100f, 0f, 1.0f);
        }

        public void UpdateMoney(int crowns, int soulCrowns)
        {
            int[] goldSplit = Parser.SplitIntToMoney(crowns);

            _crownGoldText.text = goldSplit[2].ToString();
            _crownSilverText.text = goldSplit[1].ToString();
            _crownCopperText.text = goldSplit[0].ToString();

            goldSplit = Parser.SplitIntToMoney(soulCrowns);

            _soulCrownGoldText.text = goldSplit[2].ToString();
            _soulCrownSilverText.text = goldSplit[1].ToString();
            _soulCrownCopperText.text = goldSplit[0].ToString();
        }

        public Sprite[] GetAllSpriteRankWithMainMenu()
        {
            return _ranksSprite;
        }
    }
}