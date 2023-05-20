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
    public sealed class UpdateContractPositionInScene : NetworkBasePacket
    {
        public UpdateContractPositionInScene(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();

            _positionX = networkPacket.ReadFloat();
            _positionY = networkPacket.ReadFloat();
            _positionZ = networkPacket.ReadFloat();

            _rotationX = networkPacket.ReadFloat();
            _rotationY = networkPacket.ReadFloat();
            _rotationZ = networkPacket.ReadFloat();
        }

        private readonly long _objId;
        private readonly float _positionX;
        private readonly float _positionY;
        private readonly float _positionZ;
        private readonly float _rotationX;
        private readonly float _rotationY;
        private readonly float _rotationZ;
        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateContractPositionInScene)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData objectData = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

                objectData.ObjectContract.PositionX = _positionX;
                objectData.ObjectContract.PositionY = _positionY;
                objectData.ObjectContract.PositionZ = _positionZ;

                objectData.ObjectContract.RotationX = _rotationX;
                objectData.ObjectContract.RotationY = _rotationY;
                objectData.ObjectContract.RotationZ = _rotationZ;

                objectData.UpdatePositionInServer = new Vector3(
                    _positionX, _positionY, _positionZ);

                objectData.ObjectIsLoadData = true;
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateContractPositionInScene);
            }

            return codeError;
        }
    }
}