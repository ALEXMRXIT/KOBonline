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
                _skillContract[iterator].Id = networkPacket.ReadLong();
                _skillContract[iterator].Class = (BaseClass)networkPacket.ReadInt();
                _skillContract[iterator].Name = networkPacket.ReadString();
                _skillContract[iterator].Description = networkPacket.ReadString();
                _skillContract[iterator].TypeSkill = (SkillType)networkPacket.ReadByte();
                _skillContract[iterator].UseType = (SkillUseType)networkPacket.ReadByte();
                _skillContract[iterator].Invoke = networkPacket.InternalReadBool();
                _skillContract[iterator].IDInvoke = networkPacket.ReadLong();
                _skillContract[iterator].DamageType = (SkillDamageType)networkPacket.ReadByte();
                _skillContract[iterator].Distance = networkPacket.ReadInt();
                int length = networkPacket.ReadInt();
                int iteratorOther = 0;
                _skillContract[iterator].BaseDamage = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].BaseDamage[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].MultiplyDamage = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].MultiplyDamage[iteratorOther] = networkPacket.ReadFloat();
                _skillContract[iterator].Buff = networkPacket.InternalReadBool();
                _skillContract[iterator].DeBuff = networkPacket.InternalReadBool();
                length = networkPacket.ReadInt();
                _skillContract[iterator].PrecentPhysAtk = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].PrecentPhysAtk[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].PrecentMagAtk = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].PrecentMagAtk[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddPhysAtk = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddPhysAtk[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddMagAtk = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddMagAtk[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].PrecentPhysDef = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].PrecentPhysDef[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].PrecentMagDef = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].PrecentMagDef[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddPhysDef = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddPhysDef[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddMagDef = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddMagDef[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddStrength = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddStrength[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddAgility = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddAgility[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddIntelligence = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddIntelligence[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddEndurance = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddEndurance[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddLuck = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddLuck[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AttackSpeed = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AttackSpeed[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddCriteRate = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddCriteRate[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddCritDamageMultiply = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddCritDamageMultiply[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddMissRate = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddMissRate[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddHitRate = new float[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddHitRate[iteratorOther] = networkPacket.ReadFloat();
                length = networkPacket.ReadInt();
                _skillContract[iterator].HealthRegeneration = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].HealthRegeneration[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].ManaRegeneration = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].ManaRegeneration[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddHealth = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddHealth[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].AddMana = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].AddMana[iteratorOther] = networkPacket.ReadInt();
                _skillContract[iterator].TimeUse = networkPacket.ReadInt();
                _skillContract[iterator].Recharge = networkPacket.ReadInt();
                _skillContract[iterator].TimeDelay = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].Experience = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].Experience[iteratorOther] = networkPacket.ReadInt();
                length = networkPacket.ReadInt();
                _skillContract[iterator].Mana = new int[length];
                for (iteratorOther = 0; iteratorOther < length; iteratorOther++)
                    _skillContract[iterator].Mana[iteratorOther] = networkPacket.ReadInt();
            }

            Debug.Log($"Loaded {count} skills.");
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