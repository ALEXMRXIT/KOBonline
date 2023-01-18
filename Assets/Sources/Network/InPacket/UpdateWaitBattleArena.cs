using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UpdateWaitBattleArena : NetworkBasePacket
    {
        public UpdateWaitBattleArena(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
            _buttleUI = ButtleUI.Instance;

            _values = new int[networkPacket.ReadByte()];
            for (int iterator = 0; iterator < _values.Length; iterator++)
                _values[iterator] = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly ButtleUI _buttleUI;
        private readonly int[] _values;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateWaitBattleArena)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _buttleUI.UpdateDescription(_values);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
            }

            return codeError;
        }
    }
}