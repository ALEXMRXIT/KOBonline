using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;

namespace Assets.Sources.UI.Models
{
    public sealed class PlayerRankModel : MonoBehaviour
    {
        [SerializeField] private Text _number;
        [SerializeField] private Text _playerName;
        [SerializeField] private Image _rankImage;
        [SerializeField] private Text _dataTable;
        [SerializeField] private WinRateConfigeration[] _winRateConfigurations;

        private Sprite[] _ranks;

        public void SetSpriteRanks(Sprite[] sprites)
        {
            _ranks = sprites;
        }

        public void SetModel(PlayerRankData playerRankData, INetworkProcessor networkProcessor, ContentType contentType, int index)
        {
            _number.text = index.ToString();
            _playerName.text = $"{playerRankData.CharacterName}\n<size=20>{networkProcessor.GetParentObject().GetRank.GetNameRankByRankTable(playerRankData.PlayerRank)}</size>";
            _rankImage.sprite = _ranks[networkProcessor.GetParentObject().GetRank.GetIndexByRankTable(playerRankData.PlayerRank)];

            switch (contentType)
            {
                case ContentType.WinRate:
                    for (int iterator = 0; iterator < _winRateConfigurations.Length; iterator++)
                    {
                        float winRate = InternalWinRateCalculate(playerRankData);

                        if (winRate <= _winRateConfigurations[iterator]._whenLess)
                        {
                            _dataTable.color = _winRateConfigurations[iterator]._winRateColor;
                            break;
                        }
                    }
                    _dataTable.text = $"{InternalParseSingleToStringIwthForma(InternalWinRateCalculate(playerRankData))} %";
                    break;
                case ContentType.ByLevel:
                    _dataTable.text = $"{playerRankData.Level} lvl.";
                    break;
                case ContentType.NumOfFight:
                    _dataTable.text = $"{unchecked(playerRankData.NumberWinners + playerRankData.NumberLosses)} figh.";
                    break;
                case ContentType.Rate:
                    _dataTable.text = playerRankData.PlayerRank.ToString();
                    break;
            }
        }

        private string InternalParseSingleToStringIwthForma(float value)
        {
            if (value <= 0.0f)
                return value.ToString();

            return value.ToString("#.##");
        }

        private float InternalWinRateCalculate(PlayerRankData playerRank)
        {
            return (playerRank.NumberWinners / InternalIgnoreDivideByZero(playerRank)) * 100.0f;
        }

        private float InternalIgnoreDivideByZero(PlayerRankData playerRank)
        {
            int result = (playerRank.NumberWinners + playerRank.NumberLosses);
            return result <= 0 ? 1 : result;
        }
    }
}