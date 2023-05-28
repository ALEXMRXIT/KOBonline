using TMPro;
using System;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.UI.Models;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Smile;

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
        [SerializeField] private ScrollRect _scrollRectChat;
        [SerializeField] private SmileAPI _smileAPI;
        [SerializeField] private Animator _panelAnimator;
        [SerializeField] private Scrollbar _scrollbar;

        private Animator _chatAnimator;
        private Button _chatButton;
        private bool _stateChat = false;
        private Queue<RectTransform> _chatStack = new Queue<RectTransform>();
        private INetworkProcessor _networkProcessor;
        private RectTransform _rectContent;

        private readonly string[] _colorChannel =
        {
            "#FFF583",
            "#F80000"
        };

        public static ChatManager Instance;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;

            _chatAnimator = GetComponent<Animator>();
            _chatButton = GetComponent<Button>();
            _rectContent = _spawnContent.gameObject.GetComponent<RectTransform>();
        }

        private void OnEnable() => _smileAPI.OnSmileClickhandler += InternalChatManagerOnSmileClickhandler;
        private void OnDisable() => _smileAPI.OnSmileClickhandler -= InternalChatManagerOnSmileClickhandler;

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

        public void OpenChat()
        {
            _stateChat = false;
            _chatAnimator.SetBool(_animationName, true);
            _panelAnimator.SetBool(_animationName, true);
            _scrollbar.value = 0f;
        }

        public void CloseChat()
        {
            _stateChat = true;
            _chatAnimator.SetBool(_animationName, false);
            _panelAnimator.SetBool(_animationName, false);
            _scrollbar.value = 0f;
        }

        public void AddMessageWithChat(ChatUserData chatUserData)
        {
            if (_chatStack.Count > 50)
            {
                RectTransform rectTransform = _chatStack.Dequeue();
                float delta = rectTransform.sizeDelta.y;

                foreach (RectTransform rectUpdate in _chatStack)
                    rectUpdate.gameObject.transform.localPosition = new Vector2(
                        rectUpdate.gameObject.transform.localPosition.x, rectUpdate.gameObject.transform.localPosition.y + delta);

                Destroy(rectTransform.gameObject);
            }

            StringBuilder stringBuilder = new StringBuilder();

            ClientProcessor clientProcessor = _networkProcessor.GetParentObject();
            string color = InternalGetColorWithChannel(chatUserData.MessageChannel);

            if (chatUserData.MessageChannel != Channel.Server)
            {
                stringBuilder.Append($"<mark>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}]" +
                    $"</mark><sprite={InternalGetConversionRankId(chatUserData, clientProcessor.GetRank.GetIndexByRankTable(chatUserData.MessageRankId))}" +
                    $"><color={color}>{chatUserData.MessageCharacterName}: {chatUserData.Message}</color>");
            }
            else
            {
                stringBuilder.Append($"<color={color}>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}] " +
                    $"{chatUserData.Message}</color>");
            }

            GameObject messagePanel = Instantiate(_prefabBlockMessage, _spawnContent);

            if (!messagePanel.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
                throw new MissingComponentException(nameof(TextMeshProUGUI));

            if (!messagePanel.TryGetComponent(out RectTransform rect))
                throw new MissingComponentException(nameof(RectTransform));
            _chatStack.Enqueue(rect);

            float sizeDeltaAll = 0;
            foreach (RectTransform rectTransform in _chatStack)
                sizeDeltaAll += rectTransform.sizeDelta.y;

            float rectXAxis = rect.gameObject.transform.localPosition.x;
            float rectYAxis = rect.gameObject.transform.localPosition.y;

            rect.gameObject.transform.localPosition = new Vector2(rectXAxis, rectYAxis - sizeDeltaAll);
            textMeshProUGUI.text = stringBuilder.ToString();
            Canvas.ForceUpdateCanvases();

            sizeDeltaAll = 0;
            foreach (RectTransform rectTransform in _chatStack)
                sizeDeltaAll += rectTransform.sizeDelta.y;

            if (sizeDeltaAll >= -360)
                _rectContent.sizeDelta = new Vector2(_rectContent.sizeDelta.x, sizeDeltaAll);

            if (_scrollbar.value < 0.1f)
                _scrollbar.value = 0f;
        }

        private string InternalGetColorWithChannel(Channel channel)
        {
            return _colorChannel[(int)channel];
        }

        private void InternalButtonOnHandler()
        {
            if (_stateChat) OpenChat();
            else CloseChat();
        }

        private void InternalButtonOnSendhandler()
        {
            Channel channel = _selectChannel.GetSelectedChannel();
            string characterName = _networkProcessor.GetParentObject().CharacterContract.CharacterName;
            string message = _inputField.text;

            _networkProcessor.GetParentObject().SendPacketAsync(LoadMessages.ToPacket(channel, characterName, message));

            _inputField.text = string.Empty;
        }

        private int InternalGetConversionRankId(ChatUserData chatUserData, int id)
        {
            int gmId = chatUserData.GameMaster;
            bool gmStatus = chatUserData.GameMasterStatus;

            if (gmStatus)
            {
                switch (gmId)
                {
                    case 1: return 155; // chat guard
                    case 2: return 165; // game master
                }
            }

            switch (id)
            {
                case 0: return 152; // Commoner
                case 1: return 153; // Knight
                case 2: return 154; // Baronet
                case 3: return 162; // Lord
                case 4: return 163; // Baron
                case 5: return 159; // Viscount
                case 6: return 161; // Comte
                case 7: return 160; // Marquis
                case 8: return 158; // Duke
                case 9: return 156; // Archduke
                default: return 152; // Commoner
            }
        }

        private void InternalChatManagerOnSmileClickhandler(string text)
        {
            int positionCaret = _inputField.caretPosition;
            _inputField.text = _inputField.text.Insert(positionCaret, text);
        }
    }
}