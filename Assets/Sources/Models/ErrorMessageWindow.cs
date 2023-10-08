using System;
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
        private bool _isRequest;
        private Action _action;

        public static ErrorMessageWindow Instance;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;

            _button.onClick.AddListener(InternalOnClickHandler);
            gameObject.SetActive(false);
        }

        public void ShowWindow(string message, bool isRequest = true, Action action = null)
        {
            gameObject.SetActive(true);
            _isRequest = isRequest;
            _text.text = message;
            _action = action;
        }

        private void InternalOnClickHandler()
        {
            if (_action != null)
                _action();

            if (_isRequest)
                _networkProcessor.SendPacketAsync(CheckErrorMessageService.ToPacket());
            gameObject.SetActive(false);
        }
    }
}