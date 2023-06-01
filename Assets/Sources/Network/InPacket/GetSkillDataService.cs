using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class GetSkillDataService : NetworkBasePacket
    {
        public GetSkillDataService(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int count = networkPacket.ReadInt();
            _skillData = new SkillData[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                _skillData[iterator] = new SkillData();
                _skillData[iterator].SkillId = networkPacket.ReadLong();
                _skillData[iterator].Experience = networkPacket.ReadInt();
                _skillData[iterator].Level = networkPacket.ReadInt();
                _skillData[iterator].SlotId = networkPacket.ReadInt();
            }
        }

        private readonly ClientProcessor _client;
        private readonly SkillData[] _skillData;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(GetSkillDataService)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetSkillDatas = new List<SkillData>(_skillData);
                _client.SetLoadedSkillData();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(GetSkillDataService);
            }

            return codeError;
        }
    }
}