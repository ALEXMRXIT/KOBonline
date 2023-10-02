using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.UI.Utilites;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    [System.Serializable]
    public sealed class Skill
    {
        public int Id;
        public Sprite SkillSprite;
        public Sprite SkillCloseSprite;

        [HideInInspector] public SkillHandler Handler;
    }

    public sealed class PanelObject
    {
        public ContentSizeFilterCustom contentSizeFilterCustom;
        public GameObject panelInformationObject;
        public InformationComponent InformationComponentObject;
        public RectTransform RectTransformPanelInformation;
    }

    public sealed class SkillManager : MonoBehaviour
    {
        [SerializeField] private GameObject _skillObject;
        [SerializeField] private Transform _spawnContentSkills;
        [SerializeField] private Transform _spawnScrollingParent;
        [SerializeField] private Transform _spawnCloseButtonForSkill;
        [SerializeField] private Transform _spawnPanelSkillInformation;
        [SerializeField] private GameObject _panelInformation;
        [Space, SerializeField] private AudioSource _skillTakeSoundEffect;
        [SerializeField] private AudioSource _skillDropSoundEffect;
        [Space, SerializeField] private List<Skill> _skills = new List<Skill>();

        public static SkillManager Instance;

        private INetworkProcessor _networkProcessor;
        private Dictionary<long, KeyValuePair<Skill, SkillContract>> _skillContracts;
        private SkillHandler _lastUseSkillHandler;
        private bool _statusWindow = false;

        public IEnumerator Initialize()
        {
            _networkProcessor = ClientProcessor.Instance;
            Instance = this;
            _skillContracts = new Dictionary<long, KeyValuePair<Skill, SkillContract>>();

            if (!_networkProcessor.GetParentObject().IsFirstLoadedSkillData)
            {
                _networkProcessor.SendPacketAsync(GetSkillsData.ToPacket());
                yield return new WaitUntil(() => _networkProcessor.GetParentObject().IsLoadedSkillData);
            }

            _networkProcessor.GetParentObject().GetSkills = new List<Skill>();

            foreach (SkillContract skillContract in _networkProcessor.GetParentObject().GetSkillContracts)
            {
                ClientProcessor clientProcessor = _networkProcessor.GetParentObject();
                SkillData skillData = null;

                skillData = clientProcessor.GetSkillDatas.Where(
                        sk => sk.SkillId == skillContract.Id).FirstOrDefault();

                if (skillData.WorksInNonCombat)
                    continue;

                Skill tempSkill = null;
                foreach (Skill skill in _skills)
                {
                    if (skill.Id == skillContract.Id)
                    {
                        tempSkill = skill;
                        break;
                    }
                }

                if (tempSkill == null)
                    throw new KeyNotFoundException(nameof(Skill));

                _skillContracts.TryAdd(tempSkill.Id, new KeyValuePair<Skill, SkillContract>(tempSkill, skillContract));

                GameObject skillGameObject = Instantiate(_skillObject, _spawnContentSkills);

                if (!skillGameObject.TryGetComponent(out SkillHandler skillHandler))
                    throw new MissingComponentException(nameof(SkillHandler));

                int experience = 0;
                int level = 0;

                if (skillData != null)
                {
                    experience = skillData.Experience;
                    level = skillData.Level;
                }

                tempSkill.Handler = skillHandler;

                GameObject panelInformation = Instantiate(_panelInformation, _spawnPanelSkillInformation);
                PanelObject panelObject = new PanelObject();

                if (!panelInformation.TryGetComponent(out ContentSizeFilterCustom contentSizeFilterCustom))
                    throw new MissingComponentException(nameof(ContentSizeFilterCustom));

                if (!panelInformation.TryGetComponent(out InformationComponent informationComponent))
                    throw new MissingComponentException(nameof(InformationComponent));

                panelObject.contentSizeFilterCustom = contentSizeFilterCustom;
                panelObject.panelInformationObject = panelInformation;
                panelObject.InformationComponentObject = informationComponent;

                skillHandler.SetsSoundEffects(_skillTakeSoundEffect, _skillDropSoundEffect);
                skillHandler.SetSpriteToSkill(tempSkill.SkillCloseSprite);
                skillHandler.SetSkillExperience(skillContract.Experience);
                skillHandler.SetSkillCurrentExperience(experience);
                skillHandler.SetSkillLevel(level);
                skillHandler.SetInformationObject(panelObject);
                skillHandler.SetSpawnCloseButtonAnTransform(_spawnCloseButtonForSkill, _spawnScrollingParent);
                skillHandler.ParseSkill(clientProcessor, tempSkill, skillContract, this);

                if (!_networkProcessor.GetParentObject().IsFirstLoadedSkillData)
                {
                    if (skillData != null && skillData.SlotId != -1)
                    {
                        CustomSlotInstance.Instance.SetObjectBySlotId(
                            skillData.SlotId - 1, skillHandler.CreateCloneSkill(), tempSkill);
                    }
                }

                Debug.Log($"Added new Skill {skillContract.Name} to list");
            }

            _networkProcessor.GetParentObject().GetSkills.AddRange(_skills);

            if (_networkProcessor.GetParentObject().IsFirstLoadedSkillData)
            {
                IEnumerable<SkillData> skillDatas = _networkProcessor.GetParentObject()
                    .GetSkillDatas.Where(skill => skill.SlotId != -1);

                foreach (SkillData skillData in skillDatas)
                {
                    Skill skill = _skillContracts[skillData.SkillId].Key;
                    
                    CustomSlotInstance.Instance.SetObjectBySlotId(
                        skillData.SlotId - 1, skill.Handler.CreateCloneSkill(), skill);
                }
            }

            _networkProcessor.GetParentObject().SetFirstLoadedSkillData();
        }

        public void ForceUpdateSkillInformation(SkillData skillData)
        {
            _statusWindow = true;
            gameObject.SetActive(true);

            foreach (Skill skill in _skills)
            {
                if (skill.Id == skillData.SkillId)
                {
                    skill.Handler.UpdateSkillVisual(skillData.Experience);
                    break;
                }
            }
        }

        public void SetLastUseSkillHandler(SkillHandler skillHandler)
        {
            _lastUseSkillHandler = skillHandler;
        }

        public void CloseAllSelectedMenu()
        {
            if (_lastUseSkillHandler != null)
            {
                _lastUseSkillHandler.CloseButtons();
                _lastUseSkillHandler = null;
            }
        }

        public void OpenOrClosePanel()
        {
            CloseAllSelectedMenu();
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);
        }
    }
}