using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.UI;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Network.InPacket
{
    public sealed class UseAbilityForReload : NetworkBasePacket
    {
        public UseAbilityForReload(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;
            _skillId = networkPacket.ReadLong();
        }

        private readonly ClientProcessor _client;
        private readonly long _skillId;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(UseAbilityForReload)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                foreach (SlotBattle slotBattle in _client.GetBattleSlots)
                {
                    if (slotBattle.GetSkillSlot() == null)
                        continue;

                    if (slotBattle.GetSkillSlot().GetSkillId() == _skillId)
                    {
                        slotBattle.GetSkillSlot().PlayerCooldown();
                        continue;
                    }

                    if (!slotBattle.GetSkillSlot().IsSkillReloaded())
                        slotBattle.GetSkillSlot().CooldownWithFixedTime(1f);
                }
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(UseAbilityForReload);
            }

            return codeError;
        }
    }
}