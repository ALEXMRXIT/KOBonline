using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;

namespace Assets.Sources.UI
{
    [Serializable]
    public sealed class RankModel
    {
        public Button _button;
        public Text _buttonText;
        public RankContent _rankContent;
    }

    [Serializable]
    public struct ColorStatusButton
    {
        public Color _activeButtonStatus;
        public Color _deactiveButtonStatus;
    }

    public sealed class RankTableHandler : MonoBehaviour
    {
        [SerializeField] private ColorStatusButton _colorStatusButton;
        [SerializeField] private MainUI _mainMenu;
        [SerializeField] private AudioSource _open;
        [SerializeField] private AudioSource _close;
        [SerializeField] private RankModel[] _rankModels;

        private INetworkProcessor _networkProcessor;
        private RankModel _lastActiveRank;
        private bool _statusWindow = true;

        public IEnumerator LoadRankTable(INetworkProcessor networkProcessor)
        {
            _networkProcessor = networkProcessor;

            for (int iterator = 0; iterator < _rankModels.Length; iterator++)
            {
                int saveIndex = iterator;
                _rankModels[iterator]._rankContent.SetFlagOnlyWithGameObject(true);
                _rankModels[iterator]._button.onClick.AddListener(() => InternalButtonClickHandler(saveIndex));
                yield return _rankModels[iterator]._rankContent.LoadConent(
                    networkProcessor, _rankModels[iterator], _colorStatusButton, _mainMenu, (ContentType)(byte)saveIndex);

                _rankModels[iterator]._rankContent.SetFlagOnlyWithGameObject(false);
            }

            _lastActiveRank = _rankModels[0];
            _lastActiveRank._rankContent.SetFlagActiveContent(true);

            OpenOrClosePanel();
        }

        private void InternalButtonClickHandler(int index)
        {
            RankModel model = _rankModels[index];

            if (_lastActiveRank != null)
                _lastActiveRank._rankContent.SetFlagActiveContent(false);

            _open.Play();
            model._rankContent.SetFlagActiveContent(true);
            _lastActiveRank = model;
        }

        public void OpenOrClosePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);

            if (!_statusWindow)
                _close.Play();
            else
                _open.Play();
        }
    }
}