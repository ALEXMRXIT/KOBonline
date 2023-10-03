using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;

namespace Assets.Sources.UI
{
    public sealed class SettingsHandler : MonoBehaviour
    {
        [SerializeField] private ColorStatusButton _colorStatusButton;
        [SerializeField] private RankModel[] _windowModels;

        private RankModel _lastActiveRank;
        private bool _statusWindow = true;

        public void InitializeSettingsWindow()
        {
            for (int iterator = 0; iterator < _windowModels.Length; iterator++)
            {
                int saveIndex = iterator;
                _windowModels[iterator]._rankContent.SetFlagOnlyWithGameObject(true);
                _windowModels[iterator]._button.onClick.AddListener(() => InternalButtonClickHandler(saveIndex));

                _windowModels[iterator]._rankContent.InitForSettings(_windowModels[iterator], _colorStatusButton);
            }

            _lastActiveRank = _windowModels[0];
            _lastActiveRank._rankContent.SetFlagActiveContent(true);

            if (gameObject.activeSelf)
                OpenOrClosePanel();
        }

        private void InternalButtonClickHandler(int index)
        {
            RankModel model = _windowModels[index];

            if (_lastActiveRank != null)
                _lastActiveRank._rankContent.SetFlagActiveContent(false);

            model._rankContent.SetFlagActiveContent(true);
            _lastActiveRank = model;
        }

        public void OpenOrClosePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);
        }
    }
}