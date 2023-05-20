using System;
using System.Linq;
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
            _objId = _playerContract.ObjId;
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
            _playerContract.NextExperience = networkPacket.ReadLong();
            _playerContract.CharacterBaseClass = (BaseClass)networkPacket.ReadInt();
            _playerContract.AttackDistance = networkPacket.ReadInt();
            _playerContract.MoveSpeed = networkPacket.ReadFloat();
            _playerContract.AttackSpeed = networkPacket.ReadFloat();
        }

        private readonly PlayerContract _playerContract;
        private readonly ClientProcessor _client;
        private readonly long _objId;

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

                ObjectData player = _client.GetPlayers.FirstOrDefault(player => player.ObjId == _objId);
                _client.GetPlayers.RemoveAll(player => player.ObjId != _objId);

                if (player == null)
                {
                    ObjectData playerData = new ObjectData();
                    playerData.ObjId = _objId;
                    playerData.IsBot = false;
                    playerData.ObjectContract = _playerContract;
                    _client.CharacterContract = _playerContract;

                    _client.GetPlayers.Add(playerData);
                }
                else
                {
                    player.ObjectContract = _playerContract;
                    player.ObjectIsLoadData = false;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(SelectableCharacter);
            }

            return codeError;
        }
    }
}