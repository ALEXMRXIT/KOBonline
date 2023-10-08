using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Tools;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    public sealed class SelectableChannelChat : MonoBehaviour
    {
        [SerializeField] private Toggle _channelWorldToggle;
        [SerializeField] private Button _channelWorldButton;
        [SerializeField] private Toggle _channelStoryToggle;
        [SerializeField] private Button _channelStoryButton;
        [SerializeField] private Toggle _channelPrivateMessageToggle;
        [SerializeField] private Button _channelPrivateMessageButton;
        [SerializeField] private Toggle _channelClassToggle;
        [SerializeField] private Button _channelClassButton;
        [SerializeField] private Text _textViewCurrentSelectedChannel;

        private bool _statusWindow = true;
        private Channel _currentSelectedChannel;
        private INetworkProcessor _networkProcessor;
        private IReadOnlyList<string> _colors;
        private ChatManager _chat;

        public void Init(INetworkProcessor networkProcessor, IReadOnlyCollection<string> colors, ChatManager chat)
        {
            _networkProcessor = networkProcessor;
            _currentSelectedChannel = Channel.World;
            _colors = colors as IReadOnlyList<string>;
            _chat = chat;

            _channelWorldToggle.onValueChanged.AddListener(InternalOnToggleWorldChangeHandler);
            _channelWorldButton.onClick.AddListener(() => InternalOnButtonClickWorldHandler(isOpenWindow: true));
            _channelStoryToggle.onValueChanged.AddListener(InternalOnToggleStoryChangeHandler);
            _channelStoryButton.onClick.AddListener(() => InternalOnButtonClickStoryHandler(isOpenWindow: true));
            _channelPrivateMessageToggle.onValueChanged.AddListener(InternalOnTogglePrivateMessageChangeHandler);
            _channelPrivateMessageButton.onClick.AddListener(() => InternalOnButtonClickPrivateMessageHandler(isOpenWindow: true));
            _channelClassToggle.onValueChanged.AddListener(InternalOnToggleClassChangeHandler);
            _channelClassButton.onClick.AddListener(() => InternalOnButtonClickClassHandler(isOpenWindow: true));
        }

        public void OpenOrClosePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);
        }

        public void CloseAnimation()
        {
            _chat.ResetButtonAnimation();
        }

        public Channel GetSelectedChannel()
        {
            return _currentSelectedChannel;
        }

        private void InternalOnToggleWorldChangeHandler(bool isOn)
        {
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.World, isOn));
            StaticFields._incomingMessagesFromWorld = isOn;
            PlayerPrefs.SetInt(nameof(StaticFields._incomingMessagesFromWorld), Convert.ToInt32(StaticFields._incomingMessagesFromWorld));
        }

        public void InternalOnButtonClickWorldHandler(bool isOpenWindow = true)
        {
            _currentSelectedChannel = Channel.World;
            _textViewCurrentSelectedChannel.text = $"<color={_colors[(int)_currentSelectedChannel]}>{Enum.GetName(typeof(Channel), _currentSelectedChannel)}</color>";

            if (isOpenWindow)
                OpenOrClosePanel();
        }

        private void InternalOnToggleStoryChangeHandler(bool isOn)
        {
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.Story, isOn));
            StaticFields._incomingMessageFromStory = isOn;
            PlayerPrefs.SetInt(nameof(StaticFields._incomingMessageFromStory), Convert.ToInt32(StaticFields._incomingMessageFromStory));
        }

        public void InternalOnButtonClickStoryHandler(bool isOpenWindow = true)
        {
            _currentSelectedChannel = Channel.Story;
            _textViewCurrentSelectedChannel.text = $"<color={_colors[(int)_currentSelectedChannel]}>{Enum.GetName(typeof(Channel), _currentSelectedChannel)}</color>";

            if (isOpenWindow)
                OpenOrClosePanel();
        }

        private void InternalOnTogglePrivateMessageChangeHandler(bool isOn)
        {
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.PrivateMessage, isOn));
            StaticFields._incomingMessageFromPrivateMessage = isOn;
            PlayerPrefs.SetInt(nameof(StaticFields._incomingMessageFromPrivateMessage), Convert.ToInt32(StaticFields._incomingMessageFromPrivateMessage));
        }

        public void InternalOnButtonClickPrivateMessageHandler(bool isOpenWindow = true)
        {
            _currentSelectedChannel = Channel.PrivateMessage;
            _textViewCurrentSelectedChannel.text = $"<color={_colors[(int)_currentSelectedChannel]}>Private</color>";

            if (isOpenWindow)
                OpenOrClosePanel();
        }

        private void InternalOnToggleClassChangeHandler(bool isOn)
        {
            _networkProcessor.SendPacketAsync(OnValueChangeToggleChannel.ToPacket(Channel.Class, isOn));
            StaticFields._incomingMessageFromClass = isOn;
            PlayerPrefs.SetInt(nameof(StaticFields._incomingMessageFromClass), Convert.ToInt32(StaticFields._incomingMessageFromClass));
        }

        public void InternalOnButtonClickClassHandler(bool isOpenWindow = true)
        {
            _currentSelectedChannel = Channel.Class;
            _textViewCurrentSelectedChannel.text = $"<color={_colors[(int)_currentSelectedChannel]}>{Enum.GetName(typeof(Channel), _currentSelectedChannel)}</color>";

            if (isOpenWindow)
                OpenOrClosePanel();
        }
    }
}