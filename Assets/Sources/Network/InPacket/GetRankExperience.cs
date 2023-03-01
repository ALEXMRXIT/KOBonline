using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class GetRankExperience : NetworkBasePacket
    {
        public GetRankExperience(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int length = networkPacket.ReadInt();
            _buffer = new int[length];
            for (int iterator = 0; iterator < length; iterator++)
                _buffer[iterator] = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly int[] _buffer;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(GetRankExperience)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetRank.TableInit(_buffer);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(GetRankExperience);
            }

            return codeError;
        }
    }
}