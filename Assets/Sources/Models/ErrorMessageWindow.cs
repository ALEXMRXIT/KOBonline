using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models
{
    public sealed class ErrorMessageWindow : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        private INetworkProcessor _networkProcessor;

        public static ErrorMessageWindow Instance;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;

            _button.onClick.AddListener(InternalOnClickHandler);
            gameObject.SetActive(false);
        }

        public void ShowWindow(string message)
        {
            gameObject.SetActive(true);
            _text.text = message;
        }

        private void InternalOnClickHandler()
        {
            _networkProcessor.SendPacketAsync(CheckErrorMessageService.ToPacket());
            gameObject.SetActive(false);
        }
    }
}