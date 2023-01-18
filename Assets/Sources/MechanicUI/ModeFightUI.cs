using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class ModeFightUI : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _chooseModeButton;
        [SerializeField] private Button _pvpOnlyButton;
        [SerializeField] private Button _singleOnlyButton;

        [Space]
        [SerializeField] private GameObject _panelChooseMode;

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
            _gameMode = GameMode.PVPMode;
            _fightButton.onClick.AddListener(FightButtonHandler);
            _chooseModeButton.onClick.AddListener(ChooseModeButtonHandler);
        }

        private void FightButtonHandler()
        {
            _networkProcessor.SendPacketAsync(TryEnterRoom.ToPacket(_gameMode));
        }

        private void ChooseModeButtonHandler()
        {
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }

        private void PVPOnlyButtonHandler()
        {
            _gameMode = GameMode.PVPMode;
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }

        private void SingleOnlyButtonHandler()
        {
            _gameMode = GameMode.SingleMode;
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }
    }
}