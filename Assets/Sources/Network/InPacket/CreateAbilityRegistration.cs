using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class CreateAbilityRegistration : NetworkBasePacket
    {
        public CreateAbilityRegistration(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _charId = networkPacket.ReadLong();
            _skillId = networkPacket.ReadLong();
            _level = networkPacket.ReadInt();
            _dateTime = networkPacket.InternalReadDateTime();
        }

        private readonly ClientProcessor _client;
        private readonly long _charId;
        private readonly long _skillId;
        private readonly int _level;
        private readonly DateTime _dateTime;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(CreateAbilityRegistration)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _charId);
                player.ClientVisualModelOfAbilityExecution.AddVisualAbility(_skillId, _level, _dateTime);

                switch (_skillId)
                {
                    case 1:
                        player.ClientAbilityEffectLink.MagicShieldEffectPlay();
                        player.SoundCharacterLink.CallMageShieldSoundEffect();
                        break; // mage shield
                    case 4:
                        player.ClientAbilityEffectLink.StrongBodyEffectPlay();
                        player.SoundCharacterLink.CallStrongBodySoundEffect();
                        break; // strong body
                    case 5:
                        player.ClientAbilityEffectLink.HeroesPowerEffectPlay();
                        player.SoundCharacterLink.CallHeroesPowerSoundEffect();
                        break; // heroes power
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(CreateAbilityRegistration);
            }

            return codeError;
        }
    }
}