using System;
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
            networkPacket.ReadLong();
            int count = networkPacket.ReadInt();

            for (int iterator = 0; iterator < count; iterator++)
            {
                byte code = networkPacket.ReadByte();

                switch (code)
                {
                    case StatsCode.Level: _client.GetEnemyData.ObjectContract.Level = networkPacket.ReadInt(); break;
                    case StatsCode.MinHealth: _client.GetEnemyData.ObjectContract.MinHealth = networkPacket.ReadInt(); break;
                    case StatsCode.MaxHealth: _client.GetEnemyData.ObjectContract.Health = networkPacket.ReadInt(); break;
                    case StatsCode.MinMana: _client.GetEnemyData.ObjectContract.MinMana = networkPacket.ReadInt(); break;
                    case StatsCode.MaxMana: _client.GetEnemyData.ObjectContract.Mana = networkPacket.ReadInt(); break;
                    case StatsCode.PlayerRank: _client.GetEnemyData.ObjectContract.PlayerRank = networkPacket.ReadInt(); break;
                    case StatsCode.Strength: _client.GetEnemyData.ObjectContract.Strength = networkPacket.ReadInt(); break;
                    case StatsCode.Agility: _client.GetEnemyData.ObjectContract.Agility = networkPacket.ReadInt(); break;
                    case StatsCode.Intelligence: _client.GetEnemyData.ObjectContract.Intelligence = networkPacket.ReadInt(); break;
                    case StatsCode.Endurance: _client.GetEnemyData.ObjectContract.Endurance = networkPacket.ReadInt(); break;
                    case StatsCode.Experience: _client.GetEnemyData.ObjectContract.Experience = networkPacket.ReadInt(); break;
                    case StatsCode.MoveSpeed: _client.GetEnemyData.ObjectContract.MoveSpeed = networkPacket.ReadInt(); break;
                }
            }
        }

        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(AddHUDInEnemyCharacter)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetEnemyData.ObjectHUD.SetHUD(_client.GetEnemyData.ObjectContract);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
            }

            return codeError;
        }
    }
}