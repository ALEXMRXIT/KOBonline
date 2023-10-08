using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.UI.Utilites
{
    public sealed class ChatMessageSelected : MonoBehaviour, IPointerDownHandler
    {
        private ChatManager _chat;
        private TextMeshProUGUI _textMeshProUGUI;

        public void Init(ChatManager chatManager, TextMeshProUGUI textMeshProUGUI)
        {
            _chat = chatManager;
            _textMeshProUGUI = textMeshProUGUI;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshProUGUI, eventData.pressPosition, null);

            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _textMeshProUGUI.textInfo.linkInfo[linkIndex];

                switch (linkInfo.GetLinkID())
                {
                    case "worldselectchannel":
                        _chat.GetRefSelectableChannelChat().InternalOnButtonClickWorldHandler(isOpenWindow: false);
                        _chat.ClearInputField();
                        break;
                    case "storyselectchannel":
                        _chat.GetRefSelectableChannelChat().InternalOnButtonClickStoryHandler(isOpenWindow: false);
                        _chat.ClearInputField();
                        break;
                    case "classselectchannel":
                        _chat.GetRefSelectableChannelChat().InternalOnButtonClickClassHandler(isOpenWindow: false);
                        _chat.ClearInputField();
                        break;
                    case "prmesselectchannel":
                        _chat.GetRefSelectableChannelChat().InternalOnButtonClickPrivateMessageHandler(isOpenWindow: false);
                        _chat.ClearInputField();
                        break;
                    case "refName":
                        _chat.GetRefSelectableChannelChat().InternalOnButtonClickPrivateMessageHandler(isOpenWindow: false);
                        _chat.SetNameForArgs(linkInfo.GetLinkText());
                        break;
                }
            }
        }
    }
}