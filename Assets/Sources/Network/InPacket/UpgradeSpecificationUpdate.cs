using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Tools;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpgradeSpecificationUpdate : NetworkBasePacket
    {
        public UpgradeSpecificationUpdate(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            long objId = networkPacket.ReadLong();
            ObjectData player = _client.GetPlayers.FirstOrDefault(player => player.ObjId == objId);
            _playerContract = player.ObjectContract;

            _specification = (Specification)networkPacket.ReadByte();
            player.ObjectContract.MinHealth = networkPacket.ReadInt();
            player.ObjectContract.Health = networkPacket.ReadInt();
            player.ObjectContract.MinMana = networkPacket.ReadInt();
            player.ObjectContract.Mana = networkPacket.ReadInt();
            player.ObjectContract.ScoreSpecification = networkPacket.ReadInt();
            player.ObjectContract.Strength = networkPacket.ReadInt();
            player.ObjectContract.Agility = networkPacket.ReadInt();
            player.ObjectContract.Intelligence = networkPacket.ReadInt();
            player.ObjectContract.Endurance = networkPacket.ReadInt();
            player.ObjectContract.MoveSpeed = networkPacket.ReadFloat();
            player.ObjectContract.AttackSpeed = networkPacket.ReadFloat();
            player.ObjectContract.PhysAttack = networkPacket.ReadInt();
            player.ObjectContract.MagicAttack = networkPacket.ReadInt();
            player.ObjectContract.PhysDefence = networkPacket.ReadInt();
            player.ObjectContract.MagicDefence = networkPacket.ReadInt();
            player.ObjectContract.CritChance = networkPacket.ReadFloat();
            player.ObjectContract.CritDamageMultiply = networkPacket.ReadFloat();
            player.ObjectContract.DodgeChance = networkPacket.ReadFloat();
            player.ObjectContract.HitChance = networkPacket.ReadFloat();

            if (networkPacket.InternalReadBool())
            {
                player.ObjectContract.AdditionalStrength = networkPacket.ReadInt();
                player.ObjectContract.AdditionalAgility = networkPacket.ReadInt();
                player.ObjectContract.AdditionalIntelligence = networkPacket.ReadInt();
                player.ObjectContract.AdditionalEndurance = networkPacket.ReadInt();
            }
        }

        private readonly ClientProcessor _client;
        private readonly Specification _specification;
        private readonly PlayerContract _playerContract;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpgradeSpecificationUpdate)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                SpecificationManager specification = SpecificationManager.Instance;

                specification.gameObject.SetActive(true);
                specification.InternalUpdateScoreSpecificationText(_playerContract.ScoreSpecification.ToString());

                switch (_specification)
                {
                    case Specification.Strength:
                    case Specification.Intelligence:
                        specification.InternalUpdateStrengthText($"{_playerContract.Strength+_playerContract.AdditionalStrength}{ColorCode.ColorGreen}+({_playerContract.AdditionalStrength})</color>");
                        specification.InternalUpdatePhysDefenceText(_playerContract.PhysDefence.ToString());
                        specification.InternalUpdatePhysAttackText($"{_playerContract.PhysAttack}~{InternalParseDamage(_playerContract.PhysAttack)}");
                        specification.InternalUpdateIntelligenceText($"{_playerContract.Intelligence + _playerContract.AdditionalIntelligence}{ColorCode.ColorGreen}+({_playerContract.AdditionalIntelligence})</color>");
                        specification.InternalUpdateMagicAttackText($"{_playerContract.MagicAttack}~{InternalParseDamage(_playerContract.MagicAttack)}");
                        specification.InternalUpdateMagicDefenceText(_playerContract.MagicDefence.ToString());
                        break;
                    case Specification.Agility:
                        specification.InternalUpdateAgilityText($"{_playerContract.Agility+_playerContract.AdditionalAgility}{ColorCode.ColorGreen}+({_playerContract.AdditionalAgility})</color>");
                        specification.InternalUpdateMoveSpeedText($"{(_playerContract.MoveSpeed * 10f).ToString("#.##")} %");
                        specification.InternalUpdateAttackSpeedText($"{(_playerContract.AttackSpeed * 10f).ToString("#.##")} %");
                        specification.InternalUpdateCriticalChangeText($"{_playerContract.CritChance.ToString("#.##")} %");
                        specification.InternalUpdateCriticalDamageMultiplyText($"{_playerContract.CritDamageMultiply.ToString("#.##")} %");
                        specification.InternalUpdateDodgeChanceText($"{_playerContract.DodgeChance.ToString("#.##")} %");
                        specification.InternalUpdateHitChangeText($"{_playerContract.HitChance.ToString("#.##")} %");
                        break;
                    case Specification.Endurance:
                        specification.InternalUpdateEnduranceText($"{_playerContract.Endurance+_playerContract.AdditionalEndurance}{ColorCode.ColorGreen}+({_playerContract.AdditionalEndurance})</color>");
                        break;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpgradeSpecificationUpdate);
            }

            return codeError;
        }

        private int InternalParseDamage(int damage)
        {
            if (damage < 1000)
                return unchecked(damage + ((damage / 100) * 10));
            else if (damage < 10000)
                return unchecked(damage + ((damage / 100) * 5));

            return unchecked(damage + ((damage / 100) * 2));
        }
    }
}