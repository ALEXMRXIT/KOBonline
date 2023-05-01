using UnityEngine;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models
{
    public sealed class CharacterLoadedWithServer : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private GameObject _createCharacterPanel;
        [SerializeField] private GameObject _gameRunPanel;
        [SerializeField] private GameObject _gameSkillsPanel;

        private INetworkProcessor _clientProcessor;
        private GameObject _tempModelForOnlyCreateScene;

        public static CharacterLoadedWithServer Instance;

        private void Start()
        {
            Instance = this;
            _clientProcessor = ClientProcessor.Instance;

            _clientProcessor.SendPacketAsync(LoadCharacter.ToPacket());
        }

        public void EnableUICreateCharacter()
        {
            Debug.Log($"{nameof(EnableUICreateCharacter)} enabled ui.");

            _blockPanel.SetActive(false);
            _gameSkillsPanel.SetActive(false);
            _gameRunPanel.SetActive(false);

            _createCharacterPanel.SetActive(true);
            _tempModelForOnlyCreateScene = CustomerCreateLogic.Instance.ShowModelForCreateVisualPlayer();
        }

        public void EnableUIGameRun()
        {
            Debug.Log($"{nameof(EnableUIGameRun)} enable ui.");

            if (_tempModelForOnlyCreateScene != null)
            {
                Destroy(_tempModelForOnlyCreateScene);
                _tempModelForOnlyCreateScene = null;
            }

            StartCoroutine(InternalStartLoadOtherConfigAsync());
        }

        private IEnumerator InternalStartLoadOtherConfigAsync()
        {
            //_clientProcessor.SendPacketAsync(LoadSkillsCharacter.ToPacket());
            yield return new WaitUntil(() => !_clientProcessor.GetParentObject().IsLoadedSkillCharacter);

            _blockPanel.SetActive(false);
            _gameSkillsPanel.SetActive(false);
            _createCharacterPanel.SetActive(false);

            _gameRunPanel.SetActive(true);
        }
    }
}