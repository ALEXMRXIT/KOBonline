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
    public sealed class PlayerDeath : NetworkBasePacket
    {
        public PlayerDeath(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(PlayerDeath)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);
                player.IsDeath = true;
                player.SoundCharacterLink.CallDeathSoundEffect();

                if (player.IsBot)
                    player.ClientHud.UpdateEnemyHealthBar(0, player.ObjectContract.Health);
                else
                    player.ClientHud.UpdateHealthBar(0, player.ObjectContract.Health);

                player.ObjectTarget.ClearTarget();
                player.ClientAnimationState.SetCharacterState(player._stateAnimationDeath);

                _client.GetPlayers.Where(x => x.ObjId != _objId).ToList()
                    .ForEach(client => client.ClientAnimationState.SetCharacterState(client._stateAnimationIdle));
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(PlayerDeath);
            }

            return codeError;
        }
    }
}