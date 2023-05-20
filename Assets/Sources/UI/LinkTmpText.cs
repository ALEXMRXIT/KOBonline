using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public sealed class LinkTmpText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;

        public void SetText(string text)
        {
            _tmp.text = text;
        }
    }
}