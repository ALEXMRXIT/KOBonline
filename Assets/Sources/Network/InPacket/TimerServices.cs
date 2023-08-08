using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class TimerServices : NetworkBasePacket
    {
        public TimerServices(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _codeTypeImplement = networkPacket.ReadByte();

            switch (_codeTypeImplement)
            {
                case 0: // set time
                    _seconds = networkPacket.ReadShort();
                    break;
                case 1: /* start time */ break;
                case 2: /* stop time */ break;
            }
        }

        private readonly ClientProcessor _client;
        private readonly byte _codeTypeImplement;
        private readonly short _seconds;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(TimerServices)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                switch (_codeTypeImplement)
                {
                    case 0: // set time
                        _client.GetTimeRound.Init();

                        int minutes = 0;
                        int seconds = _seconds;
                        const int secondInOneTime = 60;

                        while (seconds >= secondInOneTime)
                        {
                            minutes++;
                            seconds -= secondInOneTime;
                        }

                        _client.GetTimeRound.AddMinute(minutes);
                        _client.GetTimeRound.AddSeconds(seconds);
                        break;
                    case 1: // start time
                        _client.GetTimeRound.StartTime();
                        break;
                    case 2: // stop time
                        _client.GetTimeRound.StopTime();
                        break;
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(TimerServices);
            }

            return codeError;
        }
    }
}