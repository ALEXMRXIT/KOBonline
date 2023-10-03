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
            _client = clientProcessor;

            if (_messageType == 0x00)
                _message = networkPacket.ReadString();

            if (clientProcessor.CurrentSession == ClientCurrentMenu.Login)
                _authLogic = AuthLogic.Instance;
            else if (clientProcessor.CurrentSession == ClientCurrentMenu.Create)
                _customerCreateLogic = CustomerCreateLogic.Instance;
        }

        private readonly byte _messageType;
        private readonly string _message;
        private readonly ClientProcessor _client;
        private readonly AuthLogic _authLogic;
        private readonly CustomerCreateLogic _customerCreateLogic;
        private delegate void CallBalckMessageError(string message);

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(MessageServerReceived)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();
            CallBalckMessageError messageError = null;

            if (_client.CurrentSession == ClientCurrentMenu.Login)
                messageError = _authLogic.ShowErrorMessage;
            else if (_client.CurrentSession == ClientCurrentMenu.Create)
                messageError = _customerCreateLogic.ShowErrorMessage;

            try
            {
                switch ((MessageId)_messageType)
                {
                    case MessageId.MessageSimpleMessage:
                        break;
                    case MessageId.MessageLoginIsEmpty:
                        messageError($"Login cannot be less than 6 characters long.");
                        break;
                    case MessageId.MessagePasswordIsEmpty:
                        messageError($"Password cannot be less than 4 characters long.");
                        break;
                    case MessageId.MessageOperationFail:
                        messageError($"Operation failed, please try again later.");
                        break;
                    case MessageId.ReasonUserOrPassWrong:
                        messageError($"Invalid username or password, please try again later.");
                        break;
                    case MessageId.MessageCharacterCreate:
                        _client.CurrentSession = ClientCurrentMenu.Create;
                        CharacterLoadedWithServer.Instance.EnableUICreateCharacter();
                        break;
                    case MessageId.MessageCharacterNameIsEmpty:
                        messageError($"Name cannot be empty.");
                        break;
                    case MessageId.MessageNameExists:
                        messageError("This name already exists, please use another one.");
                        break;
                    case MessageId.MessageInvalidNamePattern:
                        messageError("Invalid characters were found in your name. You can only use letters and numbers.");
                        break;
                    case MessageId.MessageBlockCreateCharacter:
                        messageError("At this point in time, it is not allowed to create characters. Contact technical support.");
                        break;
                    case MessageId.MessageMaxCharacter:
                        messageError("You have reached the maximum character limit.");
                        break;
                    case MessageId.MessageIncorrectName:
                        messageError("Your name is not available.");
                        break;
                    case MessageId.MessageGameRun:
                        _client.CurrentSession = ClientCurrentMenu.Game;
                        CharacterLoadedWithServer.Instance.EnableUIGameRun();
                        break;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(MessageServerReceived);
            }

            return codeError;
        }
    }
}