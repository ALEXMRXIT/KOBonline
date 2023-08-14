using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
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
                MainUI.Instance.UpdateUI(_playerContract);
                SpecificationManager.Instance.InternalUpdateScoreSpecificationText(_playerContract.ScoreSpecification.ToString());

                switch (_specification)
                {
                    case Specification.Strength:
                        SpecificationManager.Instance.InternalUpdateStrengthText(_playerContract.Strength.ToString());
                        SpecificationManager.Instance.InternalUpdatePhysDefenceText(_playerContract.PhysDefence.ToString());
                        SpecificationManager.Instance.InternalUpdatePhysAttackText($"{_playerContract.PhysAttack}~{InternalParseDamage(_playerContract.PhysAttack)}");
                        break;
                    case Specification.Agility:
                        SpecificationManager.Instance.InternalUpdateAgilityText(_playerContract.Agility.ToString());
                        SpecificationManager.Instance.InternalUpdateMoveSpeedText($"{(_playerContract.MoveSpeed * 10f).ToString("#.##")} %");
                        SpecificationManager.Instance.InternalUpdateAttackSpeedText($"{(_playerContract.AttackSpeed * 10f).ToString("#.##")} %");
                        SpecificationManager.Instance.InternalUpdateCriticalChangeText($"{_playerContract.CritChance.ToString("#.##")} %");
                        SpecificationManager.Instance.InternalUpdateCriticalDamageMultiplyText($"{_playerContract.CritDamageMultiply.ToString("#.##")} %");
                        SpecificationManager.Instance.InternalUpdateDodgeChanceText($"{_playerContract.DodgeChance.ToString("#.##")} %");
                        SpecificationManager.Instance.InternalUpdateHitChangeText($"{_playerContract.HitChance.ToString("#.##")} %");
                        break;
                    case Specification.Intelligence:
                        SpecificationManager.Instance.InternalUpdateIntelligenceText(_playerContract.Intelligence.ToString());
                        SpecificationManager.Instance.InternalUpdateMagicAttackText($"{_playerContract.MagicAttack}~{InternalParseDamage(_playerContract.MagicAttack)}");
                        SpecificationManager.Instance.InternalUpdateMagicDefenceText(_playerContract.MagicDefence.ToString());
                        break;
                    case Specification.Endurance:
                        SpecificationManager.Instance.InternalUpdateEnduranceText(_playerContract.Endurance.ToString());
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