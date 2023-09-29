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
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class CharacterEnemyInfo : NetworkBasePacket
    {
        public CharacterEnemyInfo(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _playerContract = new PlayerContract();
            _client = clientProcessor;

            _playerContract.ObjId = networkPacket.ReadLong();
            _objId = _playerContract.ObjId;
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
            _playerContract.MoveSpeed = networkPacket.ReadFloat();
            _playerContract.AttackSpeed = networkPacket.ReadFloat();
        }

        private readonly PlayerContract _playerContract;
        private readonly ClientProcessor _client;
        private readonly long _objId;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(CharacterEnemyInfo)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData enemyData = new ObjectData();
                enemyData.ObjectContract = _playerContract;
                enemyData.ObjId = _objId;
                enemyData.IsBot = true;

                enemyData._stateAnimationAttackMagic = new StateAnimationAttackMagic();
                enemyData._stateAnimationAttackMagic2 = new StateAnimationAttackMagic2();
                enemyData._stateAnimationAttack = new StateAnimationAttack();
                enemyData._stateAnimationCastSpell1 = new StateAnimationCastSpell1();
                enemyData._stateAnimationCastSpell2 = new StateAnimationCastSpell2();
                enemyData._stateAnimationDeath = new StateAnimationDeath();
                enemyData._stateAnimationIdle = new StateAnimationIdle();
                enemyData._stateAnimationRun = new StateAnimationRun();

                enemyData.UpdatePositionInServer = new Vector3(
                    _playerContract.PositionX, _playerContract.PositionY, _playerContract.PositionZ);

                enemyData.ObjectIsLoadData = true;
                _client.GetPlayers.Add(enemyData);
                _client.GetNetworkDataLoader.Increment();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(CharacterEnemyInfo);
            }

            return codeError;
        }
    }
}