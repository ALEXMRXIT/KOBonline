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
                CharacterMovement mainMovement = _client.GetPlayerData.GameObjectModel.AddComponent<CharacterMovement>();
                CharacterMovement enemyMovement = _client.GetEnemyData.GameObjectModel.AddComponent<CharacterMovement>();

                mainMovement.Init(_client.GetPlayerData.ObjectTarget, _client.GetPlayerData, _client);
                enemyMovement.Init(_client.GetEnemyData.ObjectTarget, _client.GetEnemyData, null);
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