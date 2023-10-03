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
    public sealed class SendingItemForPresent : NetworkBasePacket
    {
        public SendingItemForPresent(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int length = networkPacket.ReadInt();
            _itemContracts = new ItemContract[length];

            for (int iterator = 0; iterator < length; iterator++)
            {
                _itemContracts[iterator] = new ItemContract();
                _itemContracts[iterator].ItemId = networkPacket.ReadLong();
            }
        }

        private readonly ClientProcessor _client;
        private readonly ItemContract[] _itemContracts;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(SendingItemForPresent)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                    PresentManager.Instance.SetItemForPresentMachine(_itemContracts);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(SendingItemForPresent);
            }

            return codeError;
        }
    }
}