using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Network.InPacket
{
    public sealed class ResponsePlayerRankTable : NetworkBasePacket
    {
        public ResponsePlayerRankTable(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int count = networkPacket.ReadByte();
            _playerRankDatas = new PlayerRankData[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                _playerRankDatas[iterator] = new PlayerRankData();
                _playerRankDatas[iterator].CharacterName = networkPacket.ReadString();
                _playerRankDatas[iterator].Level = networkPacket.ReadInt();
                _playerRankDatas[iterator].PlayerRank = networkPacket.ReadInt();
                _playerRankDatas[iterator].NumberWinners = networkPacket.ReadInt();
                _playerRankDatas[iterator].NumberLosses = networkPacket.ReadInt();
            }
        }

        private readonly ClientProcessor _client;
        private readonly PlayerRankData[] _playerRankDatas;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(ResponsePlayerRankTable)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetTemporaryContainerForPlayerRanks.AddRange(_playerRankDatas);
                _client.SetRankTableLoaded();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(ResponsePlayerRankTable);
            }

            return codeError;
        }
    }
}