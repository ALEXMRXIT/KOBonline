using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Base;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    [Serializable]
    public struct WinRateConfigeration
    {
        public Color _winRateColor;
        public float _whenLess;
    }

    public sealed class SpecificationManager : MonoBehaviour
    {
        [SerializeField] private string _formatDoubleValue = "#.##";
        [SerializeField] private Text _scoreSpecificationText;
        [SerializeField] private Text _strengthText;
        [SerializeField] private Text _agilityText;
        [SerializeField] private Text _intelligenceText;
        [SerializeField] private Text _enduranceText;
        [SerializeField] private Text _physAttackText;
        [SerializeField] private Text _magicAttackText;
        [SerializeField] private Text _physDefenceText;
        [SerializeField] private Text _magicDefenceText;
        [SerializeField] private Text _criticalChanceText;
        [SerializeField] private Text _criticalDamageMultiplyText;
        [SerializeField] private Text _dodgeChanceText;
        [SerializeField] private Text _hitChangeText;
        [SerializeField] private Text _attackSpeedText;
        [SerializeField] private Text _moveSpeedText;
        [SerializeField] private Text _rankText;
        [SerializeField] private Text _winRateText;
        [SerializeField] private Text _numberOfFightsText;
        [SerializeField] private Button _buttonUpgradeStrength;
        [SerializeField] private Button _buttonUpgradeAgility;
        [SerializeField] private Button _buttonUpgradeIntelligence;
        [SerializeField] private Button _buttonUpgradeEndurance;
        [SerializeField] private WinRateConfigeration[] _winRateConfigerations;

        private bool _statusWindow = true;
        private INetworkProcessor _networkProcessor;
        private PlayerContract _playerContract;

        public static SpecificationManager Instance;

        public IEnumerator LoadCharacterSpecification(INetworkProcessor networkProcessor)
        {
            _networkProcessor = networkProcessor;
            Instance = this;

            long objId = networkProcessor.GetParentObject().GetCharacterId;
            ObjectData data = networkProcessor.GetParentObject().GetPlayers.Where(x => x.ObjId == objId).FirstOrDefault();

            if (data == null)
                throw new MissingReferenceException(nameof(ObjectData));

            PlayerContract playerContract = data.ObjectContract;
            _playerContract = playerContract;

            InternalUpdateScoreSpecificationText(playerContract.ScoreSpecification.ToString());
            InternalUpdateStrengthText(playerContract.Strength.ToString());
            InternalUpdateAgilityText(playerContract.Agility.ToString());
            InternalUpdateIntelligenceText(playerContract.Intelligence.ToString());
            InternalUpdateEnduranceText(playerContract.Endurance.ToString());
            InternalUpdatePhysAttackText($"{playerContract.PhysAttack}~{InternalParseDamage(playerContract.PhysAttack)}");
            InternalUpdateMagicAttackText($"{playerContract.MagicAttack}~{InternalParseDamage(playerContract.MagicAttack)}");
            InternalUpdatePhysDefenceText(playerContract.PhysDefence.ToString());
            InternalUpdateMagicDefenceText(playerContract.MagicDefence.ToString());
            InternalUpdateCriticalChangeText($"{playerContract.CritChance.ToString(_formatDoubleValue)} %");
            InternalUpdateCriticalDamageMultiplyText($"{playerContract.CritDamageMultiply.ToString(_formatDoubleValue)} %");
            InternalUpdateDodgeChanceText($"{playerContract.DodgeChance.ToString(_formatDoubleValue)} %");
            InternalUpdateHitChangeText($"{playerContract.HitChance.ToString(_formatDoubleValue)} %");
            InternalUpdateAttackSpeedText($"{(playerContract.AttackSpeed * 10f).ToString(_formatDoubleValue)} %");
            InternalUpdateMoveSpeedText($"{(playerContract.MoveSpeed * 10f).ToString(_formatDoubleValue)} %");
            InternalUpdateRankText(playerContract.PlayerRank.ToString());
            InternalUpdateWinRateText($"{InternalParseSingleToStringIwthForma(InternalWinRateCalculate(playerContract))} %");
            InternalUpdateNumberOfFightsText((playerContract.NumberWinners + playerContract.NumberLosses).ToString());

            for (int iterator = 0; iterator < _winRateConfigerations.Length; iterator++)
            {
                float winRate = InternalWinRateCalculate(playerContract);

                if (winRate <= _winRateConfigerations[iterator]._whenLess)
                {
                    _winRateText.color = _winRateConfigerations[iterator]._winRateColor;
                    break;
                }
            }

            _buttonUpgradeStrength.onClick.AddListener(InternalOnButtonUpgradeStrengthHandler);
            _buttonUpgradeAgility.onClick.AddListener(InternalOnButtonUpgradeAgilityHandler);
            _buttonUpgradeIntelligence.onClick.AddListener(InternalOnButtonUpgradeIntelligenceHandler);
            _buttonUpgradeEndurance.onClick.AddListener(InternalOnButtonUpgradeEnduranceHandler);

            OpenOrClosePanel();
            yield break;
        }

        private string InternalParseSingleToStringIwthForma(float value)
        {
            if (value <= 0.0f)
                return value.ToString();

            return value.ToString(_formatDoubleValue);
        }

        private float InternalWinRateCalculate(PlayerContract playerContract)
        {
            return (playerContract.NumberWinners / InternalIgnoreDivideByZero(playerContract)) * 100.0f;
        }

        private float InternalIgnoreDivideByZero(PlayerContract playerContract)
        {
            int result = (playerContract.NumberWinners + playerContract.NumberLosses);
            return result <= 0 ? 1 : result;
        }

        private void InternalOnButtonUpgradeStrengthHandler()
        {
            if (_playerContract.ScoreSpecification <= 0)
                return;

            _networkProcessor.SendPacketAsync(SendUpgradeSpecification.ToPacket(Specification.Strength, 1));
        }

        private void InternalOnButtonUpgradeAgilityHandler()
        {
            if (_playerContract.ScoreSpecification <= 0)
                return;

            _networkProcessor.SendPacketAsync(SendUpgradeSpecification.ToPacket(Specification.Agility, 1));
        }

        private void InternalOnButtonUpgradeIntelligenceHandler()
        {
            if (_playerContract.ScoreSpecification <= 0)
                return;

            _networkProcessor.SendPacketAsync(SendUpgradeSpecification.ToPacket(Specification.Intelligence, 1));
        }

        private void InternalOnButtonUpgradeEnduranceHandler()
        {
            if (_playerContract.ScoreSpecification <= 0)
                return;

            _networkProcessor.SendPacketAsync(SendUpgradeSpecification.ToPacket(Specification.Endurance, 1));
        }

        public void InternalUpdateScoreSpecificationText(string message) => _scoreSpecificationText.text = message;

        public void InternalUpdateStrengthText(string message) => _strengthText.text = message;

        public void InternalUpdateAgilityText(string message) => _agilityText.text = message;

        public void InternalUpdateIntelligenceText(string message) => _intelligenceText.text = message;

        public void InternalUpdateEnduranceText(string message) => _enduranceText.text = message;

        public void InternalUpdatePhysAttackText(string message) => _physAttackText.text = message;

        public void InternalUpdateMagicAttackText(string message) => _magicAttackText.text = message;

        public void InternalUpdatePhysDefenceText(string message) => _physDefenceText.text = message;

        public void InternalUpdateMagicDefenceText(string message) => _magicDefenceText.text = message;

        public void InternalUpdateCriticalChangeText(string message) => _criticalChanceText.text = message;

        public void InternalUpdateCriticalDamageMultiplyText(string message) => _criticalDamageMultiplyText.text = message;

        public void InternalUpdateDodgeChanceText(string message) => _dodgeChanceText.text = message;

        public void InternalUpdateHitChangeText(string message) => _hitChangeText.text = message;

        public void InternalUpdateAttackSpeedText(string message) => _attackSpeedText.text = message;

        public void InternalUpdateMoveSpeedText(string message) => _moveSpeedText.text = message;

        public void InternalUpdateRankText(string message) => _rankText.text = message;

        public void InternalUpdateWinRateText(string message) => _winRateText.text = message;

        public void InternalUpdateNumberOfFightsText(string message) => _numberOfFightsText.text = message;

        private int InternalParseDamage(int damage)
        {
            if (damage < 1000)
                return unchecked(damage + ((damage / 100) * 10));
            else if (damage < 10000)
                return unchecked(damage + ((damage / 100) * 5));

            return unchecked(damage + ((damage / 100) * 2));
        }

        public void OpenOrClosePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);
        }
    }
}