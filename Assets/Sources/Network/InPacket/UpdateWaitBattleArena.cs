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

            _errorEnergy = networkPacket.InternalReadBool();

            if (!_errorEnergy)
            {
                _values = new int[networkPacket.ReadByte()];
                for (int iterator = 0; iterator < _values.Length; iterator++)
                    _values[iterator] = networkPacket.ReadInt();

                _games = networkPacket.ReadInt();
            }
        }

        private readonly ClientProcessor _client;
        private readonly ButtleUI _buttleUI;
        private readonly bool _errorEnergy;
        private readonly int[] _values;
        private readonly int _games;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateWaitBattleArena)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_client.CurrentSession == ClientCurrentMenu.Game)
                {
                    if (_buttleUI != null)
                    {
                        if (_errorEnergy)
                        {
                            _buttleUI.CloseWindow();
                            _buttleUI.ShowWindow();
                        }
                        else
                        {
                            _buttleUI.UpdateDescription(_values, _games);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateWaitBattleArena);
            }

            return codeError;
        }
    }
}