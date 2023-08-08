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
    public sealed class BattleResult : NetworkBasePacket
    {
        public BattleResult(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _isRoundTimeOut = networkPacket.InternalReadBool();
            _isWin = networkPacket.InternalReadBool();
            _experience = networkPacket.ReadLong();
            _gold = networkPacket.ReadInt();
            _rank = networkPacket.ReadInt();
        }

        private readonly ClientProcessor _client;
        private readonly bool _isRoundTimeOut;
        private readonly bool _isWin;
        private readonly long _experience;
        private readonly int _gold;
        private readonly int _rank;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(BattleResult)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                ObjectData player = _client.GetPlayers.FirstOrDefault(x => !x.IsBot);
                player.SoundCharacterLink.StopBackgroundSound();
                player.SoundCharacterLink.PlaySoundIfWinOrLosse(_isWin);

                BattleResultSources battleResult = new
                    BattleResultSources(_isRoundTimeOut, _isWin, _experience, _gold, _rank);
                InformationEndedBattle.Instance.ShowResultBattle(battleResult);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(BattleResult);
            }

            return codeError;
        }
    }
}