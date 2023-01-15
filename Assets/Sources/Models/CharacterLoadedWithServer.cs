using UnityEngine;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models
{
    public sealed class CharacterLoadedWithServer : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private GameObject _createCharacterPanel;
        [SerializeField] private GameObject _gameRunPanel;
        private INetworkProcessor _clientProcessor;

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
            _createCharacterPanel.SetActive(true);
        }

        public void EnableUIGameRun()
        {
            Debug.Log($"{nameof(EnableUIGameRun)} enable ui.");

            _blockPanel.SetActive(false);
            if (_createCharacterPanel.activeSelf)
                _createCharacterPanel.SetActive(false);

            _gameRunPanel.SetActive(true);
        }
    }
}