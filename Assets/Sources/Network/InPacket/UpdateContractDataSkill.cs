using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
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
    public sealed class UpdateContractDataSkill : NetworkBasePacket
    {
        public UpdateContractDataSkill(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            _skillData = new SkillData();
            _skillData.SkillId = networkPacket.ReadLong();
            _skillData.Experience = networkPacket.ReadInt();
            _skillData.Level = networkPacket.ReadInt();
            _skillData.SlotId = networkPacket.ReadInt();
            _skillData.WorksInNonCombat = networkPacket.InternalReadBool();
        }

        private readonly ClientProcessor _client;
        private readonly SkillData _skillData;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UpdateContractDataSkill)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                int index = _client.GetSkillDatas.IndexOf(_client.GetSkillDatas.Where
                    (s => s.SkillId == _skillData.SkillId).FirstOrDefault());
                _client.GetSkillDatas[index] = _skillData;
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UpdateContractDataSkill);
            }

            return codeError;
        }
    }
}