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
    public sealed class LoaderSkillsCharacter : NetworkBasePacket
    {
        public LoaderSkillsCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            Debug.LogWarning($"Loaded {networkPacket.ReadInt()} skills.");
        }

        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(LoaderSkillsCharacter)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {

            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(LoaderSkillsCharacter);
            }

            return codeError;
        }
    }
}