using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class LoaderChatMessages : NetworkBasePacket
    {
        public LoaderChatMessages(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int count = networkPacket.ReadInt();
            _datas = new ChatUserData[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                _datas[iterator] = new ChatUserData((Channel)
                    networkPacket.ReadByte(), networkPacket.ReadInt(),
                        networkPacket.ReadString(), networkPacket.ReadString());
            }
        }

        private readonly ClientProcessor _client;
        private readonly ChatUserData[] _datas;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(LoaderChatMessages)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                foreach (ChatUserData chatUserData in _datas)
                    ChatManager.Instance.AddMessageWithChat(chatUserData);

                _client.SetLoadedChatMessages();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(LoaderChatMessages);
            }

            return codeError;
        }
    }
}