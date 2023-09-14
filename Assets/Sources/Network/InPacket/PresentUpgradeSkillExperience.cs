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
    public sealed class PresentUpgradeSkillExperience : NetworkBasePacket
    {
        public PresentUpgradeSkillExperience(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            long id = networkPacket.ReadLong();
            SkillData skillData = _client.GetSkillDatas.Where(s => s.SkillId == id).FirstOrDefault();

            if (skillData == null)
                return;

            skillData.Experience = networkPacket.ReadInt();
            _skillData = skillData;
        }

        private readonly ClientProcessor _client;
        private readonly SkillData _skillData;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(PresentUpgradeSkillExperience)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                SkillManager.Instance.ForceUpdateSkillInformation(_skillData);
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(PresentUpgradeSkillExperience);
            }

            return codeError;
        }
    }
}