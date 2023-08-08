using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Tools;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class AddHUDInEnemyCharacter : NetworkBasePacket
    {
        public AddHUDInEnemyCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
            _objId = networkPacket.ReadLong();
            int count = networkPacket.ReadInt();

            _objectData = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

            for (int iterator = 0; iterator < count; iterator++)
            {
                byte code = networkPacket.ReadByte();
                bool int32OrFloat = networkPacket.InternalReadBool();

                if (int32OrFloat)
                {
                    switch (code)
                    {
                        case StatsCode.Level: _objectData.ObjectContract.Level = networkPacket.ReadInt(); break;
                        case StatsCode.MinHealth: _objectData.ObjectContract.MinHealth = networkPacket.ReadInt(); break;
                        case StatsCode.MaxHealth: _objectData.ObjectContract.Health = networkPacket.ReadInt(); break;
                        case StatsCode.MinMana: _objectData.ObjectContract.MinMana = networkPacket.ReadInt(); break;
                        case StatsCode.MaxMana: _objectData.ObjectContract.Mana = networkPacket.ReadInt(); break;
                        case StatsCode.PlayerRank: _objectData.ObjectContract.PlayerRank = networkPacket.ReadInt(); break;
                        case StatsCode.Strength: _objectData.ObjectContract.Strength = networkPacket.ReadInt(); break;
                        case StatsCode.Agility: _objectData.ObjectContract.Agility = networkPacket.ReadInt(); break;
                        case StatsCode.Intelligence: _objectData.ObjectContract.Intelligence = networkPacket.ReadInt(); break;
                        case StatsCode.Endurance: _objectData.ObjectContract.Endurance = networkPacket.ReadInt(); break;
                        case StatsCode.Experience: _objectData.ObjectContract.Experience = networkPacket.ReadInt(); break;
                    }
                }
                else
                {
                    switch (code)
                    {
                        case StatsCode.MoveSpeed: _objectData.ObjectContract.MoveSpeed = networkPacket.ReadFloat(); break;
                        case StatsCode.AttackSpeed: _objectData.ObjectContract.AttackSpeed = networkPacket.ReadFloat(); break;
                    }
                }
            }
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly ObjectData _objectData;
        
        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(AddHUDInEnemyCharacter)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                HudCharacter.Instance.ActivateHudEnemy(_objectData.ObjectContract);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(AddHUDInEnemyCharacter);
            }

            return codeError;
        }
    }
}