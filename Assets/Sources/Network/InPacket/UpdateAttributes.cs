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
    public sealed class UpdateAttributes : NetworkBasePacket
    {
        public UpdateAttributes(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
            _objId = networkPacket.ReadLong();
            int count = networkPacket.ReadInt();

            _objectData = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

            for (int iterator = 0; iterator < count; iterator++)
            {
                byte code = networkPacket.ReadByte();
                bool int32OrFloat = networkPacket.InternalReadBool();

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
                    case StatsCode.Experience: _objectData.ObjectContract.Experience = networkPacket.ReadLong(); break;
                    case StatsCode.MoveSpeed: _objectData.ObjectContract.MoveSpeed = networkPacket.ReadInt(); break;
                    case StatsCode.AttackSpeed: _objectData.ObjectContract.AttackSpeed = networkPacket.ReadFloat(); break;
                }
            }
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly ObjectData _objectData;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateAttributes)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_objectData.IsBot)
                {
                    if (ClientProcessor.ClientSession.ClientSessionStatus == SessionStatus.SessionGameFighting)
                    {
                        HudCharacter hudCharacter = HudCharacter.Instance;
                        hudCharacter.UpdateEnemyHealthBar(_objectData.ObjectContract.MinHealth, _objectData.ObjectContract.Health);
                        hudCharacter.UpdateEnemyManaBar(_objectData.ObjectContract.MinMana, _objectData.ObjectContract.Mana);
                    }
                }
                else
                {
                    if (ClientProcessor.ClientSession.ClientSessionStatus == SessionStatus.SessionGameFighting)
                    {
                        HudCharacter hudCharacter = HudCharacter.Instance;
                        hudCharacter.UpdateHealthBar(_objectData.ObjectContract.MinHealth, _objectData.ObjectContract.Health);
                        hudCharacter.UpdateManaBar(_objectData.ObjectContract.MinMana, _objectData.ObjectContract.Mana);
                    }
                    else if (ClientProcessor.ClientSession.ClientSessionStatus == SessionStatus.SessionGameMenu)
                    {
                        MainUI.Instance.UpdateExperience(_objectData.ObjectContract);
                    }
                }

                Debug.Log($"Attribute client: {_objectData.ObjectContract.CharacterName} was update.");
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateAttributes);
            }

            return codeError;
        }
    }
}