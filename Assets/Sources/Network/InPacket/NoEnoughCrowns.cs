using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class NoEnoughCrowns : NetworkBasePacket
    {
        public NoEnoughCrowns(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
        }

        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(NoEnoughCrowns)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                    PresentManager.Instance.NoEnoughtCrownsForStartReRollMachine();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(NoEnoughCrowns);
            }

            return codeError;
        }
    }
}