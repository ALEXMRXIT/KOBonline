using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpdateCharacterMainPosition : NetworkBasePacket
    {
        public UpdateCharacterMainPosition(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            PositionX = networkPacket.ReadFloat();
            PositionY = networkPacket.ReadFloat();
            PositionZ = networkPacket.ReadFloat();
        }

        private readonly ClientProcessor _client;
        private readonly float PositionX;
        private readonly float PositionY;
        private readonly float PositionZ;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateCharacterMainPosition)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetPlayerData.UpdatePositionInServer = new Vector3(PositionX, PositionY, PositionZ);
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