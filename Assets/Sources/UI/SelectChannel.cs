using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;

namespace Assets.Sources.UI
{
    public sealed class SelectChannel : MonoBehaviour
    {
        [SerializeField] private Text _textChannel;

        private Channel _channel;
        private readonly Color[] _colorChannel =
        {
            new Color(255, 245, 131, 255)
        };

        private void Awake()
        {
            _channel = Channel.World;
            InternalParseButtonText(_channel);
        }

        public Channel GetSelectedChannel()
        {
            return _channel;
        }

        private void InternalParseButtonText(Channel channel)
        {
            switch (channel)
            {
                case Channel.World:
                    _textChannel.color = _colorChannel[(int)channel];
                    _textChannel.text = "World";
                    break;
            }
        }
    }
}