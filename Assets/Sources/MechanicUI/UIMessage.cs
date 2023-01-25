using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.MechanicUI
{
    public sealed class UIMessage : MonoBehaviour
    {
        [SerializeField] private Text _mainMessage;
        [SerializeField] private Text _mainCountScore;

        public static UIMessage Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void AddCount(int count)
        {
            _mainMessage.gameObject.SetActive(false);

            if (!_mainCountScore.gameObject.activeSelf)
                _mainCountScore.gameObject.SetActive(true);

            _mainCountScore.text = count.ToString();

            if (count <= 1)
                Destroy(_mainCountScore.gameObject, 1f);
        }
    }
}