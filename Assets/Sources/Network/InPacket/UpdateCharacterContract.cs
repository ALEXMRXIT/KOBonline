using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Base;
using System.Linq;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpdateCharacterContract : NetworkBasePacket
    {
        public UpdateCharacterContract(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _playerContract = new PlayerContract();
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();

            _playerContract.AccountName = networkPacket.ReadString();
            _playerContract.ObjId = networkPacket.ReadLong();
            _playerContract.CharacterName = networkPacket.ReadString();
            _playerContract.Level = networkPacket.ReadInt();
            _playerContract.Health = networkPacket.ReadInt();
            _playerContract.MinHealth = _playerContract.Health;
            _playerContract.Mana = networkPacket.ReadInt();
            _playerContract.MinMana = _playerContract.Mana;
            _playerContract.Sex = (PlayerSex)networkPacket.ReadInt();
            _playerContract.PlayerRank = networkPacket.ReadInt();
            _playerContract.Strength = networkPacket.ReadInt();
            _playerContract.Agility = networkPacket.ReadInt();
            _playerContract.Intelligence = networkPacket.ReadInt();
            _playerContract.Endurance = networkPacket.ReadInt();
            _playerContract.Experience = networkPacket.ReadLong();
            _playerContract.NextExperience = networkPacket.ReadLong();
            _playerContract.CharacterBaseClass = (BaseClass)networkPacket.ReadInt();
            _playerContract.AttackDistance = networkPacket.ReadInt();
            _playerContract.MoveSpeed = networkPacket.ReadFloat();
            _playerContract.AttackSpeed = networkPacket.ReadFloat();
            _playerContract.GameMaster = networkPacket.ReadInt();
            _playerContract.GameMasterStatus = networkPacket.InternalReadBool();
        }

        private readonly PlayerContract _playerContract;
        private readonly long _objId;
        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateCharacterContract)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);
                player.ObjectContract = _playerContract;
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateCharacterContract);
            }

            return codeError;
        }
    }
}