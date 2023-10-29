using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class ModeFightUI : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Text _fightButtonText;
        [SerializeField] private Button _pvpOnlyButton;
        [SerializeField] private Button _singleOnlyButton;
        [SerializeField] private Button _cancelFight;
        [SerializeField] private Button _chooseButton;
        [SerializeField] private SingleMode _singleMode;

        [Space]
        [SerializeField] private GameObject _panelChooseMode;
        [SerializeField] private GameObject _panelAll;

        public static ModeFightUI Instance;

        private INetworkProcessor _networkProcessor;
        private GameMode _gameMode;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;
        }

        private void Start()
        {
            _gameMode = GameMode.SingleMode;
            _fightButton.onClick.AddListener(InternalFightButtonHandler);
            _cancelFight.onClick.AddListener(InternalCancelButtonHandler);
            _pvpOnlyButton.onClick.AddListener(InternalPVPOnlyButtonHandler);
            _singleOnlyButton.onClick.AddListener(InternalSingleOnlyButtonHandler);
            _chooseButton.onClick.AddListener(InternalOnClickButtonHandlerChooseMode);
            InternalOnClickButtonHandlerChooseMode();

            UpdateUIMode();
        }

        public void UpdateUIMode()
        {
            if (_gameMode == GameMode.PVPMode)
                _fightButtonText.text = "FIGHT (PVP)";
            else
                _fightButtonText.text = "SINGLE (PVE)";
        }

        private void InternalFightButtonHandler()
        {
            if (_gameMode == GameMode.PVPMode)
                _networkProcessor.SendPacketAsync(TryEnterRoom.ToPacket(_gameMode, isEnter: true));
            else
                _singleMode.OpenOrClosePanel();
        }

        private void InternalPVPOnlyButtonHandler()
        {
            _gameMode = GameMode.PVPMode;
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
            UpdateUIMode();
        }

        private void InternalSingleOnlyButtonHandler()
        {
            _gameMode = GameMode.SingleMode;
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
            UpdateUIMode();
        }

        private void InternalOnClickButtonHandlerChooseMode()
        {
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }

        private void InternalCancelButtonHandler()
        {
            _networkProcessor.SendPacketAsync(TryEnterRoom.ToPacket(_gameMode, isEnter: false));
            _panelChooseMode.SetActive(false);
            _panelAll.SetActive(false);
        }
    }
}