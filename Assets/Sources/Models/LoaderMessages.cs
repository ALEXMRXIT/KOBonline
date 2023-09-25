using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Models
{
    public sealed class LoaderMessages : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private string[] _messages;
        [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            _text.text = _messages[Random.Range(0, _messages.Length - 1)];
            _image.sprite = _sprites[Random.Range(0, _sprites.Length - 1)];
        }
    }
}