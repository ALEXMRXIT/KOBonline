using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class PlayerSetAttackCombat : NetworkBasePacket
    {
        public PlayerSetAttackCombat(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(PlayerSetAttackCombat)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

                if (player.GameObjectModel.TryGetComponent(out CharacterMovement characterMovement))
                {
                    characterMovement.InternalStopCoroutine();
                    GameObject.Destroy(characterMovement);
                }

                player.ClientAnimationState.SetCharacterState(player._stateAnimationIdle);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(PlayerSetAttackCombat);
            }

            return codeError;
        }
    }
}