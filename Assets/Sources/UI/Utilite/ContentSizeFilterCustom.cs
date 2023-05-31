using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Utilites
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class ContentSizeFilterCustom : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _offstetHeight;

        private RectTransform _mainRectTransform;
        private Vector2 _offset1 = Vector2.zero;
        private Vector2 _offset2 = Vector2.zero;

        public void Initialized()
        {
            _mainRectTransform = GetComponent<RectTransform>();
        }

        public void ContentUpdate()
        {
            _offset1 = _rectTransform.sizeDelta;
            _offset2 = _mainRectTransform.sizeDelta;

            _mainRectTransform.sizeDelta = new Vector2(_offset2.x, _offset1.y + _offstetHeight);
        }
    }
}