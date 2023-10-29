using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
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
    public sealed class SinglePlayerDataFiller : NetworkBasePacket
    {
        public SinglePlayerDataFiller(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _typeResponse = networkPacket.ReadByte();

            if (_typeResponse == 0x00)
            {
                int length = networkPacket.ReadByte();
                _singlePlayerDataModels = new SinglePlayerDataModel[length];

                for (int iterator = 0; iterator < length; iterator++)
                {
                    _singlePlayerDataModels[iterator] = new SinglePlayerDataModel();
                    _singlePlayerDataModels[iterator].Name = networkPacket.ReadString();
                    _singlePlayerDataModels[iterator].Level = networkPacket.ReadInt();
                    _singlePlayerDataModels[iterator].Time = networkPacket.ReadLong();
                    _singlePlayerDataModels[iterator].Crowns = networkPacket.ReadInt();
                    _singlePlayerDataModels[iterator].Experience = networkPacket.ReadInt();
                }
            }
            else if (_typeResponse == 0x01) // message box
            {
                if (networkPacket.ReadInt() == 1)
                    _message = networkPacket.ReadString();

                if (networkPacket.ReadInt() == 1)
                    _error = networkPacket.ReadString();

                _price = networkPacket.ReadInt();
            }
            else if (_typeResponse == 0x02) // update time single model
            {
                _level = networkPacket.ReadInt();
                _time = networkPacket.ReadLong();
            }
        }

        private readonly ClientProcessor _client;
        private readonly SinglePlayerDataModel[] _singlePlayerDataModels;
        private readonly byte _typeResponse;
        private readonly string _message;
        private readonly string _error;
        private readonly int _price;
        private readonly int _level;
        private readonly long _time;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(SinglePlayerDataFiller)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                if (_typeResponse == 0x00)
                {
                    _client.SinglePlayerDataCollection = _singlePlayerDataModels;
                    _client.SetFlagIsLoadedSinglePlayer();
                }
                else if (_typeResponse == 0x01) // message box
                {
                    SingleMode.Instance.ShowMessageBox(_message, _error, _price);
                }
                else if (_typeResponse == 0x02) // update time single model
                {
                    SingleMode.Instance.UpdateTime(_level, _time);
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(SinglePlayerDataFiller);
            }

            return codeError;
        }
    }
}