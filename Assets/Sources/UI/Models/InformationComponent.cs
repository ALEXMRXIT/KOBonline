using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Models
{
    public sealed class InformationComponent : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}