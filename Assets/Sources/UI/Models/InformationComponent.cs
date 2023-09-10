using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Models
{
    public sealed class InformationComponent : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(InternalOnClickHandler);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        private void InternalOnClickHandler()
        {
            gameObject.SetActive(false);
        }
    }
}