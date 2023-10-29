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
    public sealed class InviteOnDuelService : NetworkBasePacket
    {
        public InviteOnDuelService(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _code = networkPacket.ReadByte();

            if (_code == 0x00 || _code == 0x01)
            {
                _characterName = networkPacket.ReadString();
                _energy = networkPacket.ReadInt();
            }
        }

        private readonly ClientProcessor _client;
        private readonly byte _code;
        private readonly string _characterName;
        private readonly int _energy;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(InviteOnDuelService)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                {
                    if (_code == 0x00 || _code == 0x01)
                    {
                        MainUI.Instance.ShowInviteMessageCost(_code, _energy, _characterName);
                    }
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(InviteOnDuelService);
            }

            return codeError;
        }
    }
}