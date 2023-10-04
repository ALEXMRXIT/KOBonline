using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class AuthLogic : MonoBehaviour
    {
        [SerializeField] private Text _errorText;
        [SerializeField] private InputField _loginInput;
        [SerializeField] private InputField _passwordInput;
        [SerializeField] private Button _authButton;
        [SerializeField] private Button _registrationButton;

        private INetworkProcessor _networkProcessor;

        public static AuthLogic Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _authButton.onClick.AddListener(() => ButtonHandleAuth(AuthType.Auth));
            _registrationButton.onClick.AddListener(() => ButtonHandleAuth(AuthType.Registration));
            _networkProcessor = ClientProcessor.Instance;
        }

        private void ButtonHandleAuth(AuthType authType)
        {
            if (!ValidInput(_loginInput.text, _passwordInput.text))
            {
                _passwordInput.text = string.Empty;
                return;
            }

            _networkProcessor.SendPacketAsync(AuthClient.AuthToPacket(_loginInput.text,
                _passwordInput.text, authType, Application.version));
            _passwordInput.text = string.Empty;
            _errorText.text = string.Empty;
        }

        private bool ValidInput(string login, string password)
        {
            if (!_networkProcessor.IsConnected)
            {
                ShowErrorMessage($"Server connection error, please check your internet connection and try again.");
                return false;
            }

            if (string.IsNullOrEmpty(_loginInput.text))
            {
                ShowErrorMessage($"Login cannot be empty.");
                return false;
            }

            if (_loginInput.text.Length < 6)
            {
                ShowErrorMessage($"Login cannot be less than 6 symbols long.");
                return false;
            }

            if (string.IsNullOrEmpty(_passwordInput.text))
            {
                ShowErrorMessage($"Password cannot be empty.");
                return false;
            }

            if (_passwordInput.text.Length < 4)
            {
                ShowErrorMessage($"Password cannot be less than 4 symbols long.");
                return false;
            }

            return true;
        }

        public void ShowErrorMessage(string message)
        {
            _errorText.text = message;
        }
    }
}