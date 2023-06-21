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
    public sealed class UpdateAbility : NetworkBasePacket
    {
        public UpdateAbility(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _objId = networkPacket.ReadLong();
            _skillId = networkPacket.ReadLong();

            byte count = networkPacket.ReadByte();
            for (byte iterator = 0; iterator < count; iterator++)
            {
                byte code = networkPacket.ReadByte();

                switch (code)
                {
                    case 0: _health = networkPacket.ReadInt(); break;
                }
            }
        }

        private readonly ClientProcessor _client;
        private readonly long _objId;
        private readonly long _skillId;
        private readonly int _health;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateAbility)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => x.ObjId == _objId);

                switch (_skillId)
                {
                    case 5: player.ClientTextView.HealTarget(player, _health); break;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateAbility);
            }

            return codeError;
        }
    }
}