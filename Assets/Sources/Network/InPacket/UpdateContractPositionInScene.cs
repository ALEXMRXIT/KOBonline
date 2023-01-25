using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpdateContractPositionInScene : NetworkBasePacket
    {
        public UpdateContractPositionInScene(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _positionX = networkPacket.ReadFloat();
            _positionY = networkPacket.ReadFloat();
            _positionZ = networkPacket.ReadFloat();

            _rotationX = networkPacket.ReadFloat();
            _rotationY = networkPacket.ReadFloat();
            _rotationZ = networkPacket.ReadFloat();
        }

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
                _client.GetPlayerContract.PositionX = _positionX;
                _client.GetPlayerContract.PositionY = _positionY;
                _client.GetPlayerContract.PositionZ = _positionZ;

                _client.GetPlayerContract.RotationX = _rotationX;
                _client.GetPlayerContract.RotationY = _rotationY;
                _client.GetPlayerContract.RotationZ = _rotationZ;
                _client.GetPlayerPacketLoaded = true;
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