using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    public sealed class RankContent : MonoBehaviour
    {
        [SerializeField] private PlayerRankModel _player;
        [SerializeField] private Transform _spawnPlayerIwthContent;
        [SerializeField] private Sprite _buttonActive;
        [SerializeField] private Sprite _buttonDeactive;

        private INetworkProcessor _networkProcessor;
        private RankModel _rankModel;
        private ColorStatusButton _colorStatusButton;
        private MainUI _mainUI;

        public void SetFlagOnlyWithGameObject(bool flag)
        {
            gameObject.SetActive(flag);
        }

        public void InitForSettings(RankModel rankModel, ColorStatusButton colorStatusButton)
        {
            _rankModel = rankModel;
            _colorStatusButton = colorStatusButton;
            gameObject.SetActive(false);
        }

        public void SetFlagActiveContent(bool flag)
        {
            SetFlagOnlyWithGameObject(flag);

            if (!flag)
            {
                _rankModel._button.image.sprite = _buttonDeactive;
                _rankModel._buttonText.color = _colorStatusButton._deactiveButtonStatus;
            }
            else
            {
                _rankModel._button.image.sprite = _buttonActive;
                _rankModel._buttonText.color = _colorStatusButton._activeButtonStatus;
            }
        }

        public IEnumerator LoadConent(INetworkProcessor networkProcessor,
            RankModel rankModel, ColorStatusButton colorStatusButton, MainUI mainUI, ContentType contentType)
        {
            _networkProcessor = networkProcessor;
            _rankModel = rankModel;
            _colorStatusButton = colorStatusButton;
            _mainUI = mainUI;

            networkProcessor.SendPacketAsync(SendRequestRankConent.ToPacket(contentType));
            yield return new WaitUntil(() => networkProcessor.GetParentObject().IsRankTableLoaded);
            networkProcessor.GetParentObject().ResetRankTableLoaded();

            List<PlayerRankData> players = new List<PlayerRankData>();
            players.AddRange(_networkProcessor.GetParentObject().GetTemporaryContainerForPlayerRanks);

            _networkProcessor.GetParentObject().ResetTemporaryContainer();

            int count = players.Count;
            for (int iterator = 0; iterator < count; iterator++)
            {
                GameObject player = Instantiate(_player.gameObject, _spawnPlayerIwthContent);

                if (!player.TryGetComponent(out PlayerRankModel playerRankModel))
                    throw new MissingComponentException(nameof(PlayerRankModel));

                playerRankModel.SetSpriteRanks(_mainUI.GetAllSpriteRankWithMainMenu());
                playerRankModel.SetModel(players[iterator], networkProcessor, contentType, iterator + 1);
            }
        }
    }
}