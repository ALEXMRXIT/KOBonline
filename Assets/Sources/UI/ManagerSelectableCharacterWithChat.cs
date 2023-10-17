using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public sealed class ManagerSelectableCharacterWithChat : MonoBehaviour
    {
        [SerializeField] private Button _privateMessage;
        [SerializeField] private Button _challangeDuel;

        private string[] _args;
        private RectTransform _rectTransform;
        private ChatManager _chat;

        public void Init(RectTransform panel, Transform parent, ChatManager chat)
        {
            _privateMessage.onClick.AddListener(InternalOnClickPrivteMessageHandler);
            _challangeDuel.onClick.AddListener(InternalOnClickChallengeDuelHandler);

            _rectTransform = panel;
            _rectTransform.SetParent(parent, worldPositionStays: false);
            _rectTransform.transform.SetAsLastSibling();
            SetStatusGameObject(status: false);
            _chat = chat;
        }

        public void SetPosition(Vector3 worldPointInRectangle)
        {
            _rectTransform.position = worldPointInRectangle;
        }

        public void SetStatusGameObject(bool status)
        {
            gameObject.SetActive(status);
        }

        public void SetArgs(string[] args)
        {
            _args = args;
        }

        private void InternalOnClickPrivteMessageHandler()
        {
            _chat.GetRefSelectableChannelChat().InternalOnButtonClickPrivateMessageHandler(isOpenWindow: false);
            _chat.SetNameForArgs(_args[0]);
            SetStatusGameObject(status: false);
        }

        private void InternalOnClickChallengeDuelHandler()
        {

            SetStatusGameObject(status: false);
        }
    }
}