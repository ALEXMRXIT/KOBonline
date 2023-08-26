using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Assets.Sources.Network;
using UnityEngine.EventSystems;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.UI.Utilites;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    [RequireComponent(typeof(Animator))]
    public sealed class Slot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool _main = true;
        [SerializeField] private Button _button;
        [SerializeField] private int _slotId;

        private Animator _animator;
        private GameObject _item;
        private bool _showButton;
        private INetworkProcessor _networkProcessor;
        private Skill _skill;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_main)
                _button.onClick.AddListener(InternalButtonClickHandler);
            _networkProcessor = ClientProcessor.Instance;
        }

        public void EnterToSlotObject(GameObject item, Skill skill)
        {
            _item = item;
            _skill = skill;

            item.transform.SetParent(gameObject.transform);
            RectTransform rectTransform = item.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0f, 0f, rectTransform.localPosition.z);

            Image image = item.GetComponent<Image>();
            image.raycastTarget = false;
        }

        public bool IsSlotEmpty() => _item == null;
        public void DestroyItem()
        {
            if (_item == null)
                return;

            Destroy(_item);
            _item = null;
        }

        public int GetSlotId()
        {
            return _slotId;
        }

        public void CloseButton()
        {
            _showButton = false;
            _animator.SetBool("manager", _showButton);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_main)
            {
                if (IsSlotEmpty())
                    return;

                CustomSlotInstance.Instance.CloseSelecatbleSlot(this);
                transform.SetAsLastSibling();
                _animator.SetBool("manager", _showButton = !_showButton);
            }
        }

        private void InternalButtonClickHandler()
        {
            _networkProcessor.GetParentObject().GetSkillDatas.Where(
                skill => skill.SkillId == _skill.Id).FirstOrDefault().SlotId = -1;
            DestroyItem();
            CustomSlotInstance.Instance.UpdateLastSelectableSlot();
            _networkProcessor.SendPacketAsync(SendUpgradeSkill.ToPacket(_skill.Id, true, -1));
            _showButton = false;
            _animator.SetBool("manager", _showButton);
        }
    }
}