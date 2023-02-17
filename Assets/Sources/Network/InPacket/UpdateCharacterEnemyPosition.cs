using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpdateCharacterEnemyPosition : NetworkBasePacket
    {
        public UpdateCharacterEnemyPosition(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
            _positionX = networkPacket.ReadFloat();
            _positionY = networkPacket.ReadFloat();
            _positionZ = networkPacket.ReadFloat();
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly float _positionX;
        private readonly float _positionY;
        private readonly float _positionZ;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateCharacterEnemyPosition)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId).UpdatePositionInServer =
                    new Vector3(_positionX, _positionY, _positionZ);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateCharacterEnemyPosition);
            }

            return codeError;
        }
    }
}