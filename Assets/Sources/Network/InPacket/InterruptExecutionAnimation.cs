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
    public sealed class InterruptExecutionAnimation : NetworkBasePacket
    {
        public InterruptExecutionAnimation(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
            _index = networkPacket.ReadByte();
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly byte _index;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(InterruptExecutionAnimation)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

                switch (_index)
                {
                    case 0: player.ClientAnimationState.SetCharacterState(player._stateAnimationAttack, player.ObjectContract.AttackSpeed); break;
                    case 1: player.ClientAnimationState.SetCharacterState(player._stateAnimationAttackMagic); break;
                    case 2: player.ClientAnimationState.SetCharacterState(player._stateAnimationCastSpell1); break;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(InterruptExecutionAnimation);
            }

            return codeError;
        }
    }
}