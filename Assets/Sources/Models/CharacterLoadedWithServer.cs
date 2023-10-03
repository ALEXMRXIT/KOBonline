using System;
using UnityEngine;
using Assets.Sources.UI;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Smile;

namespace Assets.Sources.Models
{
    public sealed class CharacterLoadedWithServer : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private GameObject _createCharacterPanel;
        [SerializeField] private GameObject _gameRunPanel;
        [SerializeField] private GameObject _gameSkillsPanel;
        [SerializeField] private SmileAPI _smileAPI;
        [SerializeField] private ChatManager _chatManager;
        [SerializeField] private SkillManager _skillManager;
        [SerializeField] private SpecificationManager _specificationManager;
        [SerializeField] private RankTableHandler _rankTableHandler;
        [SerializeField] private PresentManager _presentManager;
        [SerializeField] private SettingsHandler _settingsHandler;
        [SerializeField] private GameObject _experiencePanel;

        private INetworkProcessor _clientProcessor;
        private GameObject _tempModelForOnlyCreateScene;
        private bool _isWillCreateCharacter = false;

        public static CharacterLoadedWithServer Instance;

        private void Start()
        {
            Instance = this;
            _clientProcessor = ClientProcessor.Instance;
            _smileAPI.Initialized();
            _smileAPI.HideSmile();

            _clientProcessor.SendPacketAsync(LoadCharacter.ToPacket());
        }

        public void EnableUICreateCharacter()
        {
            Debug.Log($"{nameof(EnableUICreateCharacter)} enabled ui.");

            _blockPanel.SetActive(false);
            _gameSkillsPanel.SetActive(false);
            _gameRunPanel.SetActive(false);
            _experiencePanel.SetActive(false);
            _specificationManager.OpenOrClosePanel();
            _rankTableHandler.OpenOrClosePanel();
            _presentManager.HiddenViewModel();
            _presentManager.OpenOrCloseWindow();
            _settingsHandler.OpenOrClosePanel();

            _isWillCreateCharacter = true;

            _createCharacterPanel.SetActive(true);
            _tempModelForOnlyCreateScene = CustomerCreateLogic.Instance.ShowModelForCreateVisualPlayer();
        }

        public void EnableUIGameRun()
        {
            Debug.Log($"{nameof(EnableUIGameRun)} enable ui.");

            if (!_blockPanel.activeSelf)
            {
                _blockPanel.SetActive(true);
                _gameSkillsPanel.SetActive(true);
                _gameRunPanel.SetActive(true);
                _specificationManager.OpenOrClosePanel();
                _rankTableHandler.OpenOrClosePanel();
            }

            if (_tempModelForOnlyCreateScene != null)
            {
                DestroyImmediate(_tempModelForOnlyCreateScene);
                _tempModelForOnlyCreateScene = null;
            }

            StartCoroutine(InternalStartLoadOtherConfigAsync());
        }

        private IEnumerator InternalStartLoadOtherConfigAsync()
        {
            _experiencePanel.SetActive(true);
            yield return new WaitUntil(() => _clientProcessor.GetParentObject().IsLoadedCharacterModel);
            yield return _chatManager.GetMessagesWithChat(_clientProcessor);

            if (!_clientProcessor.GetParentObject().IsFirstLoadedSkillData)
            {
                _clientProcessor.SendPacketAsync(LoadSkillsCharacter.ToPacket());
                yield return new WaitUntil(() => _clientProcessor.GetParentObject().IsLoadedSkillCharacter);
            }

            yield return _skillManager.Initialize();
            yield return _specificationManager.LoadCharacterSpecification(_clientProcessor);
            yield return _rankTableHandler.LoadRankTable(_clientProcessor);

            if (_isWillCreateCharacter)
                _presentManager.OpenOrCloseWindow();

            _clientProcessor.GetParentObject().GetPresentManager = _presentManager.GetInstance();
            yield return _presentManager.InternalLoadPresentWithCharacter(_clientProcessor);
            _settingsHandler.InitializeSettingsWindow();

            yield return new WaitForSecondsRealtime(2f);

            _blockPanel.SetActive(false);
            _gameSkillsPanel.SetActive(false);
            _createCharacterPanel.SetActive(false);
            _gameRunPanel.SetActive(true);

            _clientProcessor.SendPacketAsync(CheckErrorMessageService.ToPacket());
        }
    }
}