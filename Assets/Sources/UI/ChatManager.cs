using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.UI.Models;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using Assets.Sources.UI.Utilites;
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
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _sendButton;
        [SerializeField] private ScrollRect _scrollRectChat;
        [SerializeField] private SmileAPI _smileAPI;
        [SerializeField] private Animator _panelAnimator;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private SelectableChannelChat _selectableChannelChat;
        [SerializeField] private ErrorMessageWindow _errorMessageWindow;
        [SerializeField] private Toggle _channelWorldToggle;
        [SerializeField] private Toggle _channelStoryToggle;
        [SerializeField] private Toggle _channelPrivateMessageToggle;
        [SerializeField] private Toggle _channelClassToggle;
        [SerializeField] private GameObject _buttonAnimationInChannelSelected;
        [SerializeField] private RectTransform _windowCharacterManager;
        [SerializeField] private ManagerSelectableCharacterWithChat _managerSelectableCharacterWithChat;
        [SerializeField] private Transform _spawnSelectablePanel;

        private Animator _chatAnimator;
        private Button _chatButton;
        private bool _stateChat = false;
        private Queue<RectTransform> _chatStack = new Queue<RectTransform>();
        private INetworkProcessor _networkProcessor;
        private RectTransform _rectContent;
        private StringBuilder _stringBuilder;
        private StringBuilder _stringBuilderPrivateMessage;
        private string _privateChannelName;

        private readonly string[] _colorChannel =
        {
            "#FFF583ff", // world channel
            "#F80000ff", // server channel
            "#613600ff", // story channel
            "#610059ff", // private message channel
            "#00a4baff"  // class channel
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

        public SelectableChannelChat GetRefSelectableChannelChat()
        {
            return _selectableChannelChat;
        }

        public void ClearInputField()
        {
            _inputField.text = string.Empty;
        }

        public void SetNameForArgs(string name)
        {
            if (!_inputField.text.Contains(name))
            {
                if (!string.IsNullOrEmpty(_inputField.text))
                    _inputField.text = $"{name}/ ";
                else
                    _inputField.text = _inputField.text.Insert(0, name + "/ ");
            }
            _privateChannelName = name;
        }

        public void ResetButtonAnimation()
        {
            if (!StaticFields._buttonAnimationInChannelSelection)
                return;

            StaticFields._buttonAnimationInChannelSelection = false;
            PlayerPrefs.SetInt(nameof(StaticFields._buttonAnimationInChannelSelection), Convert.ToInt32(StaticFields._buttonAnimationInChannelSelection));
            _buttonAnimationInChannelSelected.SetActive(false);
        }

        public void InviteOnDuel(string inviteCharacterName)
        {
            _networkProcessor.GetParentObject().SendPacketAsync(InviteOnDuelService.ToPacket(0x00, inviteCharacterName));
        }

        private void Start()
        {
            _chatButton.onClick.AddListener(InternalButtonOnHandler);
            _sendButton.onClick.AddListener(InternalButtonOnSendhandler);
        }

        public IEnumerator GetMessagesWithChat(INetworkProcessor networkProcessor)
        {
            ClientProcessor clientProcessor = networkProcessor.GetParentObject();
            _stringBuilder = new StringBuilder(capacity: 8 * 8 * 4);
            _stringBuilderPrivateMessage = new StringBuilder(8 * 8);

            if (clientProcessor == null)
                throw new NullReferenceException(nameof(ClientProcessor));

            if (PlayerPrefs.HasKey(nameof(StaticFields._incomingMessagesFromWorld)))
                StaticFields._incomingMessagesFromWorld = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(StaticFields._incomingMessagesFromWorld)));
            else
                StaticFields._incomingMessagesFromWorld = true;

            if (PlayerPrefs.HasKey(nameof(StaticFields._incomingMessageFromStory)))
                StaticFields._incomingMessageFromStory = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(StaticFields._incomingMessageFromStory)));
            else
                StaticFields._incomingMessageFromStory = true;

            if (PlayerPrefs.HasKey(nameof(StaticFields._incomingMessageFromPrivateMessage)))
                StaticFields._incomingMessageFromPrivateMessage = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(StaticFields._incomingMessageFromPrivateMessage)));
            else
                StaticFields._incomingMessageFromPrivateMessage = true;

            if (PlayerPrefs.HasKey(nameof(StaticFields._incomingMessageFromClass)))
                StaticFields._incomingMessageFromClass = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(StaticFields._incomingMessageFromClass)));
            else
                StaticFields._incomingMessageFromClass = true;

            if (PlayerPrefs.HasKey(nameof(StaticFields._buttonAnimationInChannelSelection)))
                StaticFields._buttonAnimationInChannelSelection = Convert.ToBoolean(PlayerPrefs.GetInt(nameof(StaticFields._buttonAnimationInChannelSelection)));
            else
                StaticFields._buttonAnimationInChannelSelection = true;

            _channelWorldToggle.isOn = StaticFields._incomingMessagesFromWorld;
            _channelStoryToggle.isOn = StaticFields._incomingMessageFromStory;
            _channelPrivateMessageToggle.isOn = StaticFields._incomingMessageFromPrivateMessage;
            _channelClassToggle.isOn = StaticFields._incomingMessageFromClass;

            if (!StaticFields._buttonAnimationInChannelSelection)
                _buttonAnimationInChannelSelected.SetActive(false);

            _selectableChannelChat.Init(networkProcessor, _colorChannel, this);
            _managerSelectableCharacterWithChat.Init(_windowCharacterManager, _spawnSelectablePanel, this);

            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.Story, StaticFields._incomingMessageFromStory));
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.World, StaticFields._incomingMessagesFromWorld));
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.PrivateMessage, StaticFields._incomingMessageFromPrivateMessage));
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.Class, StaticFields._incomingMessageFromClass));

            _selectableChannelChat.OpenOrClosePanel();
            CloseChat();

            yield break;
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

        public void AddMessageWithChat(ChatUserData chatUserData, string[] args)
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

            ClientProcessor clientProcessor = _networkProcessor.GetParentObject();
            string color = InternalGetColorWithChannel(chatUserData.MessageChannel);

            if (chatUserData.MessageChannel == Channel.World || chatUserData.MessageChannel == Channel.Class)
            {
                string insertTagLink = string.Empty;

                if (chatUserData.MessageChannel == Channel.World) insertTagLink = "worldselectchannel";
                else if (chatUserData.MessageChannel == Channel.Class) insertTagLink = "classselectchannel";

                _stringBuilder.Append($"<link=\"{insertTagLink}\"><color={color}><u><mark>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}]</mark></link>" +
                    $"<sprite={InternalGetConversionRankId(chatUserData, clientProcessor.GetRank.GetIndexByRankTable(chatUserData.MessageRankId))}" +
                    $"><link=\"refName\">{chatUserData.MessageCharacterName}</link></u>: {chatUserData.Message}</color>");
            }
            else if (chatUserData.MessageChannel == Channel.Server || chatUserData.MessageChannel == Channel.Story)
            {
                if (chatUserData.MessageChannel == Channel.Server)
                {
                    _stringBuilder.Append($"<color={color}><u><mark>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}]</mark></u> " +
                        $"{chatUserData.Message}</color>");
                }
                else if (chatUserData.MessageChannel == Channel.Story)
                {
                    _stringBuilder.Append($"<link=\"storyselectchannel\"><color={color}><u><mark>[{Enum.GetName(typeof(Channel), chatUserData.MessageChannel)}]</mark></u></link> " +
                        $"{chatUserData.Message}</color>");
                }
            }
            else if (chatUserData.MessageChannel == Channel.PrivateMessage)
            {
                try
                {
                    string name = _networkProcessor.GetParentObject().GetPlayers.Where(x => x.ObjectContract.ObjId ==
                        _networkProcessor.GetParentObject().GetCharacterId).FirstOrDefault().ObjectContract.CharacterName;

                    if (string.IsNullOrEmpty(name))
                        throw new Exception();

                    if (args == null || args.Length == 0)
                        throw new Exception();

                    if (name.Equals(args[0], StringComparison.OrdinalIgnoreCase))
                    {
                        _stringBuilder.Append($"<link=\"prmesselectchannel\"><color={color}><u><mark>[Private Message]" +
                            $"</mark></link><sprite={InternalGetConversionRankId(chatUserData, clientProcessor.GetRank.GetIndexByRankTable(chatUserData.MessageRankId))}></u>" +
                            $"<u><link=\"refName\">{chatUserData.MessageCharacterName}</link></u> Tell you: {chatUserData.Message}</color>");
                    }
                    else
                    {
                        _stringBuilder.Append($"<link=\"prmesselectchannel\"><color={color}><u><mark>[Private Message]" +
                            $"</mark></u></link>You tell <u><link=\"refName\">{_privateChannelName}</link></u>: {chatUserData.Message}</color>");
                    }
                }
                catch
                {
                    _stringBuilder.Append($"<mark><color={color}>[Error]</mark>Unknown error with the private messages channel. Please restart the game. If the error persists, please report it to the game master, thank you.</color>");
                }
            }

            GameObject messagePanel = Instantiate(_prefabBlockMessage, _spawnContent);

            if (!messagePanel.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
                throw new MissingComponentException(nameof(TextMeshProUGUI));

            if (!messagePanel.TryGetComponent(out RectTransform rect))
                throw new MissingComponentException(nameof(RectTransform));

            if (!messagePanel.TryGetComponent(out ChatMessageSelected chatMessageSelected))
                throw new MissingComponentException(nameof(ChatMessageSelected));

            chatMessageSelected.Init(this, textMeshProUGUI, _managerSelectableCharacterWithChat);
            _chatStack.Enqueue(rect);

            float sizeDeltaAll = 0;
            foreach (RectTransform rectTransform in _chatStack)
                sizeDeltaAll += rectTransform.sizeDelta.y;

            float rectXAxis = rect.gameObject.transform.localPosition.x;
            float rectYAxis = rect.gameObject.transform.localPosition.y;

            rect.gameObject.transform.localPosition = new Vector2(rectXAxis, rectYAxis - sizeDeltaAll);
            textMeshProUGUI.text = _stringBuilder.ToString();
            _stringBuilder.Clear();
            Canvas.ForceUpdateCanvases();

            sizeDeltaAll = 0;
            foreach (RectTransform rectTransform in _chatStack)
                sizeDeltaAll += rectTransform.sizeDelta.y;

            if (sizeDeltaAll >= -360)
                _rectContent.sizeDelta = new Vector2(_rectContent.sizeDelta.x, sizeDeltaAll);

            if (_scrollbar.value <= 0.1f)
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
            Channel channel = _selectableChannelChat.GetSelectedChannel();

            if (channel == Channel.Story)
            {
                _errorMessageWindow.ShowWindow("It is impossible to write messages to this channel!", isRequest: false);
                return;
            }

            ObjectData player = _networkProcessor.GetParentObject().GetPlayers.FirstOrDefault(x => !x.IsBot);
            string characterName = player.ObjectContract.CharacterName;
            string message = _inputField.text;
            string[] args = null;

            if (channel == Channel.PrivateMessage)
            {
                string[] parseString = message.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (parseString.Length == 0 || parseString.Length == 1)
                {
                    _errorMessageWindow.ShowWindow("Cannot send message. There was an error formatting the recipient name arguments.", isRequest: false);
                    return;
                }

                string name = _networkProcessor.GetParentObject().GetPlayers.Where(x => x.ObjectContract.ObjId ==
                    _networkProcessor.GetParentObject().GetCharacterId).FirstOrDefault().ObjectContract.CharacterName;

                if (name.Equals(parseString[0], StringComparison.OrdinalIgnoreCase))
                {
                    _errorMessageWindow.ShowWindow("You can't text yourself!", isRequest: false);
                    return;
                }

                args = new string[1];
                args[0] = parseString[0];
                _privateChannelName = parseString[0];

                int index = 0;
                foreach (string text in parseString)
                {
                    if (index++ == 0)
                        continue;

                    _stringBuilderPrivateMessage.Append(text);
                }

                message = _stringBuilderPrivateMessage.ToString();
                _stringBuilderPrivateMessage.Clear();
            }

            _networkProcessor.GetParentObject().SendPacketAsync(LoadMessages.ToPacket(channel, characterName, message, args));

            if (channel != Channel.PrivateMessage)
                ClearInputField();
            else
                _inputField.text = $"{_privateChannelName}\\ ";
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