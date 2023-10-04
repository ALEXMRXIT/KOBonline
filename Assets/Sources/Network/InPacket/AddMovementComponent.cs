using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class AddMovementComponent : NetworkBasePacket
    {
        public AddMovementComponent(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
        }

        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(AddMovementComponent)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                foreach (ObjectData objectData in _client.GetPlayers)
                {
                    if (!objectData.GameObjectModel.TryGetComponent<CharacterMovement>(out _))
                        objectData.GameObjectModel.AddComponent<CharacterMovement>().Init(objectData.ObjectTarget, objectData, _client);
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(AddMovementComponent);
            }

            return codeError;
        }
    }
}