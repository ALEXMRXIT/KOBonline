using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Base;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.Models.Characters
{
    public sealed class InformationEndedBattle : MonoBehaviour
    {
        [SerializeField] private Color _colorWin;
        [SerializeField] private Color _colorDefeat;
        [SerializeField] private Color _colorTimeOut;
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _experienceText;
        [SerializeField] private Text _copperText;
        [SerializeField] private Text _silverText;
        [SerializeField] private Text _goldText;
        [SerializeField] private GameObject _rankGreen;
        [SerializeField] private GameObject _rankRed;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private GameObject _panelForPresents;
        [SerializeField] private Image _slotForPresent;
        [SerializeField] private Sprite[] _presents;

        public static InformationEndedBattle Instance;

        private INetworkProcessor _networkProcessor;

        private void Awake()
        {
            _networkProcessor = ClientProcessor.Instance;
            _acceptButton.onClick.AddListener(InternalButtonHandler);
            Instance = this;
            _panelForPresents.SetActive(false);
        }

        private void InternalButtonHandler()
        {
            _networkProcessor.SendPacketAsync(ChangeSession.ToPacket(SessionStatus.SessionGameMenu));
        }

        public void ShowResultBattle(BattleResultSources battleResultSources)
        {
            gameObject.SetActive(true);

            if (!battleResultSources.IsRoundTimeOut)
            {
                if (battleResultSources.IsCharacterWin)
                {
                    _titleText.color = _colorWin;
                    _titleText.text = "Victory!!";
                }
                else
                {
                    _titleText.color = _colorDefeat;
                    _titleText.text = "Defeat!!";
                }
            }
            else
            {
                _titleText.color = _colorTimeOut;
                _titleText.text = "Time out!!";
            }

            _experienceText.text = $"+{battleResultSources.AddExperience}";

            int[] goldSplit = Parser.SplitIntToMoney(battleResultSources.AddGold);
            _goldText.text = goldSplit[2].ToString();
            _silverText.text = goldSplit[1].ToString();
            _copperText.text = goldSplit[0].ToString();

            if (battleResultSources.IsCharacterWin && !battleResultSources.IsRoundTimeOut)
            {
                _rankGreen.SetActive(true);
                _rankGreen.GetComponent<Text>().text = $"+{battleResultSources.AddRank}";
            }
            else
            {
                _rankRed.SetActive(true);
                _rankRed.GetComponent<Text>().text = battleResultSources.AddRank < 0 ?
                    ($"{battleResultSources.AddRank}") : ($"-{battleResultSources.AddRank}");
            }

            if (battleResultSources.PresentType > -1)
            {
                _panelForPresents.SetActive(true);
                _slotForPresent.sprite = _presents[battleResultSources.PresentType];
            }
        }
    }
}