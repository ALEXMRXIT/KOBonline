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
    public sealed class GettingPresents : NetworkBasePacket
    {
        public GettingPresents(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int count = networkPacket.ReadInt();
            _presentContracts = new PresentContract[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                _presentContracts[iterator] = new PresentContract();
                _presentContracts[iterator].PresentType = networkPacket.ReadInt();
                _presentContracts[iterator].ObjId = networkPacket.ReadLong();
                _presentContracts[iterator].Time = networkPacket.ReadLong();
                _presentContracts[iterator].Slot = networkPacket.ReadInt();
                _presentContracts[iterator].CostOfOneSecondGift = networkPacket.ReadInt();
            }

            _howMuchWillCostReRollGiftlvl1 = networkPacket.ReadInt();
            _howMuchWillCostReRollGiftlvl2 = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly PresentContract[] _presentContracts;
        private readonly int _howMuchWillCostReRollGiftlvl1;
        private readonly int _howMuchWillCostReRollGiftlvl2;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(GettingPresents)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.GetPresentManager != null && _presentContracts.Length > 0)
                    _client.GetPresentManager.SetPresentContract(_presentContracts);

                if (_client.GetPresentManager != null)
                    _client.GetPresentManager.SetPrice(_howMuchWillCostReRollGiftlvl1, _howMuchWillCostReRollGiftlvl2);

                _client.SetFlagIsLoadedPresents();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(GettingPresents);
            }

            return codeError;
        }
    }
}