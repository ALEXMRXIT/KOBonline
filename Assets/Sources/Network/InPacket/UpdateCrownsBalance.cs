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
    public sealed class UpdateCrownsBalance : NetworkBasePacket
    {
        public UpdateCrownsBalance(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _crowns = networkPacket.ReadInt();
            _soulCrowns = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly int _crowns;
        private readonly int _soulCrowns;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateCrownsBalance)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                {
                    ObjectData player = _client.GetPlayers.FirstOrDefault(player => player.ObjId == _client.GetCharacterId);
                    player.ObjectContract.Crowns = _crowns;
                    player.ObjectContract.SoulCrowns = _soulCrowns;

                    MainUI.Instance.UpdateMoney(_crowns, _soulCrowns);
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateCrownsBalance);
            }

            return codeError;
        }
    }
}