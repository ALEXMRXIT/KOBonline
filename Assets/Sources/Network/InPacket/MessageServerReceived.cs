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
    public sealed class MessageServerReceived : NetworkBasePacket
    {
        public MessageServerReceived(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _messageType = networkPacket.ReadByte();

            if (_messageType == 0x00)
                _message = networkPacket.ReadString();

            _authLogic = AuthLogic.Instance;
        }

        private readonly byte _messageType;
        private readonly string _message;
        private readonly ClientProcessor _client;
        private readonly AuthLogic _authLogic;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(MessageServerReceived)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                switch ((MessageId)_messageType)
                {
                    case MessageId.MessageSimpleMessage:
                        break;
                    case MessageId.MessageLoginIsEmpty:
                        _authLogic.ShowErrorMessage($"Login cannot be less than 6 characters long.");
                        break;
                    case MessageId.MessagePasswordIsEmpty:
                        _authLogic.ShowErrorMessage($"Password cannot be less than 4 characters long.");
                        break;
                    case MessageId.MessageOperationFail:
                        _authLogic.ShowErrorMessage($"Operation failed, please try again later.");
                        break;
                    case MessageId.ReasonUserOrPassWrong:
                        _authLogic.ShowErrorMessage($"Invalid username or password, please try again later.");
                        break;
                    case MessageId.MessageCharacterCreate:
                        CharacterLoadedWithServer.Instance.EnableUICreateCharacter();
                        break;
                }
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