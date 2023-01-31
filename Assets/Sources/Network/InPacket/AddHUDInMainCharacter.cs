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
    public sealed class AddHUDInMainCharacter : NetworkBasePacket
    {
        public AddHUDInMainCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            networkPacket.ReadLong();
            int count = networkPacket.ReadInt();

            for (int iterator = 0; iterator < count; iterator++)
            {
                byte code = networkPacket.ReadByte();

                switch (code)
                {
                    case StatsCode.Level: _client.GetPlayerData.ObjectContract.Level = networkPacket.ReadInt(); break;
                    case StatsCode.MinHealth: _client.GetPlayerData.ObjectContract.MinHealth = networkPacket.ReadInt(); break;
                    case StatsCode.MaxHealth: _client.GetPlayerData.ObjectContract.Health = networkPacket.ReadInt(); break;
                    case StatsCode.MinMana: _client.GetPlayerData.ObjectContract.MinMana = networkPacket.ReadInt(); break;
                    case StatsCode.MaxMana: _client.GetPlayerData.ObjectContract.Mana = networkPacket.ReadInt(); break;
                    case StatsCode.PlayerRank: _client.GetPlayerData.ObjectContract.PlayerRank = networkPacket.ReadInt(); break;
                    case StatsCode.Strength: _client.GetPlayerData.ObjectContract.Strength = networkPacket.ReadInt(); break;
                    case StatsCode.Agility: _client.GetPlayerData.ObjectContract.Agility = networkPacket.ReadInt(); break;
                    case StatsCode.Intelligence: _client.GetPlayerData.ObjectContract.Intelligence = networkPacket.ReadInt(); break;
                    case StatsCode.Endurance: _client.GetPlayerData.ObjectContract.Endurance = networkPacket.ReadInt(); break;
                    case StatsCode.Experience: _client.GetPlayerData.ObjectContract.Experience = networkPacket.ReadInt(); break;
                    case StatsCode.MoveSpeed: _client.GetPlayerData.ObjectContract.MoveSpeed = networkPacket.ReadInt(); break;
                }
            }
        }

        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(AddHUDInMainCharacter)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetPlayerData.ObjectHUD.SetHUD(_client.GetPlayerData.ObjectContract);
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