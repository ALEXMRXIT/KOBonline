using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.MechanicUI.Models
{
    [RequireComponent(typeof(Image))]
    public sealed class Slot : MonoBehaviour
    {
        [SerializeField] private GameObject _effect;

        private Image _image;

        public void Init()
        {
            _image = GetComponent<Image>();
            SetEffectDisable();
        }

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public Sprite GetCurrentSlotSprite()
        {
            return _image.sprite;
        }

        public GameObject SetEffectEnable()
        {
            _effect.SetActive(true);
            return _effect;
        }

        public GameObject SetEffectDisable()
        {
            _effect.SetActive(false);
            return _effect;
        }
    }
}