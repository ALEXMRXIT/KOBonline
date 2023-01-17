using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.MechanicUI
{
    public sealed class ModeFightUI : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _chooseModeButton;
        [SerializeField] private Button _pvpOnlyButton;
        [SerializeField] private Button _singleOnlyButton;

        [Space]
        [SerializeField] private GameObject _panelChooseMode;

        public static ModeFightUI Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _fightButton.onClick.AddListener(FightButtonHandler);
            _chooseModeButton.onClick.AddListener(ChooseModeButtonHandler);
        }

        private void FightButtonHandler()
        {
        }

        private void ChooseModeButtonHandler()
        {
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }

        private void PVPOnlyButtonHandler()
        {
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }

        private void SingleOnlyButtonHandler()
        {
            _panelChooseMode.SetActive(!_panelChooseMode.activeSelf);
        }
    }
}