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
    public sealed class SessionChanged : NetworkBasePacket
    {
        public SessionChanged(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _newSession = networkPacket.ReadByte();
        }

        private readonly byte _newSession;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(SessionChanged)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ClientProcessor.ClientSession.ClientSessionStatus = (SessionStatus)_newSession;
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(SessionChanged);
            }

            return codeError;
        }
    }
}