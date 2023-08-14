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

namespace Assets.Sources.Network.InPacket
{
    public sealed class CharacterAttackCombat : NetworkBasePacket
    {
        public CharacterAttackCombat(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
            _damage = networkPacket.ReadInt();
            _willSkillUse = networkPacket.InternalReadBool();
            _isCriticalDamage = networkPacket.InternalReadBool();
            _damageMiss = networkPacket.InternalReadBool();
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly int _damage;
        private readonly bool _willSkillUse;
        private readonly bool _isCriticalDamage;
        private readonly bool _damageMiss;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(CharacterAttackCombat)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

                if (!_willSkillUse && player.ClientAnimationState.GetCurrentPlayingAnimationState() != player._stateAnimationAttackMagic)
                    player.ClientAnimationState.SetCharacterState(player._stateAnimationAttack, player.ObjectContract.AttackSpeed);

                Damage damage = new Damage(player.IsBot, _damage, _isCriticalDamage, _damageMiss);

                player.ClientTextView.AddDamage(damage);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(CharacterAttackCombat);
            }

            return codeError;
        }
    }
}