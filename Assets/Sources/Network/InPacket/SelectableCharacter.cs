using System;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Network.InPacket
{
    public sealed class SelectableCharacter : NetworkBasePacket
    {
        public SelectableCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            playerContract = new PlayerContract();
            _client = clientProcessor;

            playerContract.AccountName = networkPacket.ReadString();
            playerContract.ObjId = networkPacket.ReadLong();
            playerContract.CharacterName = networkPacket.ReadString();
            playerContract.Level = networkPacket.ReadInt();
            playerContract.Health = networkPacket.ReadInt();
            playerContract.Mana = networkPacket.ReadInt();
            playerContract.Sex = (PlayerSex)networkPacket.ReadInt();
            playerContract.Strength = networkPacket.ReadInt();
            playerContract.Agility = networkPacket.ReadInt();
            playerContract.Intelligence = networkPacket.ReadInt();
            playerContract.Endurance = networkPacket.ReadInt();
            playerContract.Experience = networkPacket.ReadLong();
            playerContract.CharacterBaseClass = (BaseClass)networkPacket.ReadInt();
        }

        private readonly PlayerContract playerContract;
        private readonly ClientProcessor _client;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(SelectableCharacter)}.");
#endif

            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                CustomerModelView.Instance.ModelFinalBuild(playerContract, showModel: true);
                MainUI.Instance.UpdateUI(playerContract);
                _client.GetPlayerContract = playerContract;
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