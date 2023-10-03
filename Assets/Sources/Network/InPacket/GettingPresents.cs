using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class GettingGiftPresentInPlayer : NetworkBasePacket
    {
        public GettingGiftPresentInPlayer(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _presentContract = new PresentContract();

            _presentContract.PresentType = networkPacket.ReadInt();
            _presentContract.ObjId = networkPacket.ReadLong();
            _presentContract.Time = networkPacket.ReadLong();
            _presentContract.Slot = networkPacket.ReadInt();
            _presentContract.CostOfOneSecondGift = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly PresentContract _presentContract;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(GettingGiftPresentInPlayer)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                    _client.GetPresentManager.SetPresentContractWithOnlyAlone(_presentContract);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(GettingGiftPresentInPlayer);
            }

            return codeError;
        }
    }
}