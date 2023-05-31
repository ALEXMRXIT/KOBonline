using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using System.Collections.Generic;

namespace Assets.Sources.Network.InPacket
{
    public sealed class LoaderSkillsCharacter : NetworkBasePacket
    {
        public LoaderSkillsCharacter(NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            _client = clientProcessor;

            int count = networkPacket.ReadInt();
            _skillContract = new SkillContract[count];

            for (int iterator = 0; iterator < count; iterator++)
            {
                _skillContract[iterator] = new SkillContract();
                _skillContract[iterator].Id = networkPacket.ReadInt();
                _skillContract[iterator].Class = (BaseClass)networkPacket.ReadInt();
                _skillContract[iterator].Name = networkPacket.ReadString();
                _skillContract[iterator].Description = networkPacket.ReadString();
                _skillContract[iterator].TypeSkill = (SkillType)networkPacket.ReadByte();
                _skillContract[iterator].UseType = (SkillUseType)networkPacket.ReadByte();
                _skillContract[iterator].Invoke = networkPacket.InternalReadBool();
                _skillContract[iterator].IDInvoke = networkPacket.ReadLong();
                _skillContract[iterator].DamageType = (SkillDamageType)networkPacket.ReadByte();
                _skillContract[iterator].Distance = networkPacket.ReadInt();
                _skillContract[iterator].BaseDamage = networkPacket.ReadInt();
                _skillContract[iterator].MultiplyDamage = networkPacket.ReadFloat();
                _skillContract[iterator].Buff = networkPacket.InternalReadBool();
                _skillContract[iterator].DeBuff = networkPacket.InternalReadBool();
                _skillContract[iterator].PrecentPhysAtk = networkPacket.ReadFloat();
                _skillContract[iterator].PrecentMagAtk = networkPacket.ReadFloat();
                _skillContract[iterator].AddPhysAtk = networkPacket.ReadInt();
                _skillContract[iterator].AddMagAtk = networkPacket.ReadInt();
                _skillContract[iterator].PrecentPhysDef = networkPacket.ReadFloat();
                _skillContract[iterator].PrecentMagDef = networkPacket.ReadFloat();
                _skillContract[iterator].AddPhysDef = networkPacket.ReadInt();
                _skillContract[iterator].AddMagDef = networkPacket.ReadInt();
                _skillContract[iterator].AddStrength = networkPacket.ReadInt();
                _skillContract[iterator].AddAgility = networkPacket.ReadInt();
                _skillContract[iterator].AddIntelligence = networkPacket.ReadInt();
                _skillContract[iterator].AddEndurance = networkPacket.ReadInt();
                _skillContract[iterator].AddLuck = networkPacket.ReadInt();
                _skillContract[iterator].AttackSpeed = networkPacket.ReadFloat();
                _skillContract[iterator].AddCriteRate = networkPacket.ReadFloat();
                _skillContract[iterator].AddCritDamageMultiply = networkPacket.ReadFloat();
                _skillContract[iterator].AddMissRate = networkPacket.ReadFloat();
                _skillContract[iterator].AddHitRate = networkPacket.ReadFloat();
                _skillContract[iterator].HealthRegeneration = networkPacket.ReadInt();
                _skillContract[iterator].ManaRegeneration = networkPacket.ReadInt();
                _skillContract[iterator].AddHealth = networkPacket.ReadInt();
                _skillContract[iterator].AddMana = networkPacket.ReadInt();
                _skillContract[iterator].TimeUse = networkPacket.ReadInt();
                _skillContract[iterator].Recharge = networkPacket.ReadInt();
                _skillContract[iterator].TimeDelay = networkPacket.ReadInt();
                int countExperience = networkPacket.ReadInt();
                _skillContract[iterator].Experience = new int[countExperience];
                for (int iteratorExp = 0; iteratorExp < countExperience; iteratorExp++)
                    _skillContract[iterator].Experience[iteratorExp] = networkPacket.ReadInt();
                int countMana = networkPacket.ReadInt();
                _skillContract[iterator].Mana = new int[countMana];
                for (int iteratorMana = 0; iteratorMana < countMana; iteratorMana++)
                    _skillContract[iterator].Mana[iteratorMana] = networkPacket.ReadInt();
            }

            Debug.LogWarning($"Loaded {count} skills.");
        }

        private readonly ClientProcessor _client;
        private readonly SkillContract[] _skillContract;

        public override PacketImplementCodeResult RunImpl()
        {
#if UNITY_EDITOR
            Debug.Log($"Execute {nameof(LoaderSkillsCharacter)}.");
#endif
            PacketImplementCodeResult codeError = new PacketImplementCodeResult();

            try
            {
                _client.GetSkillContracts = new List<SkillContract>(_skillContract);
                _client.SetLoadedSkillCharacter();
            }
            catch (Exception exception)
            {
                codeError.ErrorCode = -1;
                codeError.ErrorMessage = exception.Message;
                codeError.InnerException = exception;
                codeError.FireException = nameof(LoaderSkillsCharacter);
            }

            return codeError;
        }
    }
}