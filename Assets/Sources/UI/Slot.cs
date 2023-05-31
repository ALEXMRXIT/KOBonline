using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Sources.UI.Utilites;

namespace Assets.Sources.UI
{
    [RequireComponent(typeof(Animator))]
    public sealed class Slot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Button _button;

        private Animator _animator;
        private GameObject _item;
        private bool _showButton = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _button.onClick.AddListener(InternalButtonClickHandler);
        }

        public void EnterToSlotObject(GameObject item)
        {
            _item = item;
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

        public void CloseButton()
        {
            _showButton = false;
            _animator.SetBool("manager", _showButton);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CustomSlotInstance.Instance.CloseSelecatbleSlot(this);
            transform.SetAsLastSibling();
            _animator.SetBool("manager", _showButton = !_showButton);
        }

        private void InternalButtonClickHandler()
        {
            DestroyItem();
            CustomSlotInstance.Instance.UpdateLastSelectableSlot();
            _showButton = false;
            _animator.SetBool("manager", _showButton);
        }
    }
}