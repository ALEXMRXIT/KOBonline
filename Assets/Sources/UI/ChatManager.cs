using TMPro;
using System;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

#pragma warning disable

namespace Assets.Sources.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public sealed class ChatManager : MonoBehaviour
    {
        [SerializeField] private string _animationName;
        [SerializeField] private GameObject _prefabBlockMessage;
        [SerializeField] private Transform _spawnContent;
        [SerializeField] private SelectChannel _selectChannel;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _sendButton;

        private Animator _chatAnimator;
        private Button _chatButton;
        private bool _stateChat = false;
        private Stack<GameObject> _chatStack = new Stack<GameObject>();
        private INetworkProcessor _networkProcessor;

        private readonly string[] _colorChannel =
        {
            "#FFF583"
        };

        public static ChatManager Instance;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;

            _chatAnimator = GetComponent<Animator>();
            _chatButton = GetComponent<Button>();
        }

        private void Start()
        {
            _chatButton.onClick.AddListener(InternalButtonOnHandler);
            _sendButton.onClick.AddListener(InternalButtonOnSendhandler);
        }

        public IEnumerator GetMessagesWithChat(INetworkProcessor networkProcessor)
        {
            ClientProcessor clientProcessor = networkProcessor.GetParentObject();

            if (clientProcessor == null)
                throw new NullReferenceException(nameof(ClientProcessor));

            clientProcessor.SendPacketAsync(LoadMessages.ToPacket(clientProcessor.CharacterContract.CharacterName));
            yield return new WaitUntil(() => clientProcessor.IsChatMessageLoaded);
            CloseChat();
        }

        public void OpenChat() => _chatAnimator.SetBool(_animationName, false);
        public void CloseChat() => _chatAnimator.SetBool(_animationName, true);

        public void AddMessageWithChat(ChatUserData chatUserData)
        {
            if (_chatStack.Count > 50)
                Destroy(_chatStack.Pop());

            StringBuilder stringBuilder = new StringBuilder();

            string color = InternalGetColorWithChannel(chatUserData.MessageChannel);
            stringBuilder.Append($"<mark>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}]" +
                $"</mark><sprite={chatUserData.MessageRankId}</sprite><color={color}>" +
                $"{chatUserData.MessageCharacterName}: {chatUserData.Message}</color>");

            GameObject messagePanel = Instantiate(_prefabBlockMessage, _spawnContent);
            _chatStack.Push(messagePanel);

            if (!messagePanel.TryGetComponent(out LinkTmpText linkTmpText))
                throw new MissingComponentException(nameof(LinkTmpText));

            linkTmpText.SetText(stringBuilder.ToString());
        }

        private string InternalGetColorWithChannel(Channel channel)
        {
            return _colorChannel[(int)channel];
        }

        private void InternalButtonOnHandler()
        {
            _stateChat = !_stateChat;

            if (_stateChat) OpenChat();
            else CloseChat();
        }

        private void InternalButtonOnSendhandler()
        {
            Channel channel = _selectChannel.GetSelectedChannel();
            string characterName = _networkProcessor.GetParentObject().CharacterContract.CharacterName;
            string message = _inputField.text;

            _networkProcessor.GetParentObject().SendPacketAsync(LoadMessages.ToPacket(channel, characterName, message));
        }
    }
}