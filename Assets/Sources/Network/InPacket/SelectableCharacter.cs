using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class SelectableCharacter : NetworkBasePacket
    {
        public SelectableCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _playerContract = new PlayerContract();
            _client = clientProcessor;

            _playerContract.AccountName = networkPacket.ReadString();
            _playerContract.ObjId = networkPacket.ReadLong();
            _playerContract.CharacterName = networkPacket.ReadString();
            _playerContract.Level = networkPacket.ReadInt();
            _playerContract.Health = networkPacket.ReadInt();
            _playerContract.Mana = networkPacket.ReadInt();
            _playerContract.Sex = (PlayerSex)networkPacket.ReadInt();
            _playerContract.PlayerRank = networkPacket.ReadInt();
            _playerContract.Strength = networkPacket.ReadInt();
            _playerContract.Agility = networkPacket.ReadInt();
            _playerContract.Intelligence = networkPacket.ReadInt();
            _playerContract.Endurance = networkPacket.ReadInt();
            _playerContract.Experience = networkPacket.ReadLong();
            _playerContract.CharacterBaseClass = (BaseClass)networkPacket.ReadInt();
            _playerContract.AttackDistance = networkPacket.ReadInt();
            _playerContract.MoveSpeed = networkPacket.ReadInt();
        }

        private readonly PlayerContract _playerContract;
        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(SelectableCharacter)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                CustomerModelView.Instance.ModelFinalBuild(_playerContract, showModel: true);
                MainUI.Instance.UpdateUI(_playerContract);

                _client.GetPlayerData = new ObjectData();
                _client.GetEnemyData = new ObjectData();
                _client.GetPlayerData.ObjectContract = _playerContract;
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