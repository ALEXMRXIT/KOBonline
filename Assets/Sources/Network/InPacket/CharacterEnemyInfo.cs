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
    public sealed class CharacterEnemyInfo : NetworkBasePacket
    {
        public CharacterEnemyInfo(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _playerContract = new PlayerContract();
            _client = clientProcessor;

            _playerContract.ObjId = networkPacket.ReadLong();
            _playerContract.CharacterName = networkPacket.ReadString();
            _playerContract.Health = networkPacket.ReadInt();
            _playerContract.Mana = networkPacket.ReadInt();
            _playerContract.Sex = (PlayerSex)networkPacket.ReadInt();
            _playerContract.PlayerRank = networkPacket.ReadInt();
            _playerContract.CharacterBaseClass = (BaseClass)networkPacket.ReadInt();

            _playerContract.PositionX = networkPacket.ReadFloat();
            _playerContract.PositionY = networkPacket.ReadFloat();
            _playerContract.PositionZ = networkPacket.ReadFloat();

            _playerContract.RotationX = networkPacket.ReadFloat();
            _playerContract.RotationY = networkPacket.ReadFloat();
            _playerContract.RotationZ = networkPacket.ReadFloat();
            _playerContract.AttackDistance = networkPacket.ReadInt();
            _playerContract.MoveSpeed = networkPacket.ReadInt();
        }

        private readonly PlayerContract _playerContract;
        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(CharacterEnemyInfo)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetEnemyData.ObjectContract = _playerContract;

                _client.GetEnemyData.UpdatePositionInServer = new Vector3(
                    _playerContract.PositionX, _playerContract.PositionY, _playerContract.PositionZ);

                _client.GetEnemyData.ObjectIsLoadData = true;
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