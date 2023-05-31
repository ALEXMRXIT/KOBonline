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
    }

    public sealed class SkillManager : MonoBehaviour
    {
        [SerializeField] private GameObject _skillObject;
        [SerializeField] private Transform _spawnContentSkills;
        [SerializeField] private Transform _spawnScrollingParent;
        [SerializeField] private Transform _spawnCloseButtonForSkill;
        [SerializeField] private Transform _spawnPanelSkillInformation;
        [SerializeField] private GameObject _panelInformation;
        [SerializeField] private List<Skill> _skills = new List<Skill>();

        private INetworkProcessor _networkProcessor;
        private List<KeyValuePair<int, KeyValuePair<Skill, SkillContract>>> _skillContracts;
        private SkillHandler _lastUseSkillHandler;

        public IEnumerator Initialize()
        {
            _networkProcessor = ClientProcessor.Instance;
            _skillContracts = new List<KeyValuePair<int, KeyValuePair<Skill, SkillContract>>>();

            _networkProcessor.SendPacketAsync(GetSkillsData.ToPacket());
            yield return new WaitUntil(() => _networkProcessor.GetParentObject().IsLoadedSkillData);

            int iterator = 0;
            foreach (SkillContract skillContract in _networkProcessor.GetParentObject().GetSkillContracts)
            {
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

                _skillContracts.Add(new KeyValuePair<int, KeyValuePair<Skill, SkillContract>>
                    (skillContract.Id, new KeyValuePair<Skill, SkillContract>(tempSkill, skillContract)));

                GameObject skillGameObject = Instantiate(_skillObject, _spawnContentSkills);
                ClientProcessor clientProcessor = _networkProcessor.GetParentObject();

                if (!skillGameObject.TryGetComponent(out SkillHandler skillHandler))
                    throw new MissingComponentException(nameof(SkillHandler));

                int experience = 0;
                int level = 0;

                if (clientProcessor.GetSkillDatas.Count > 0)
                {
                    SkillData skillData = clientProcessor.GetSkillDatas.Where(
                        sk => sk.SkillId == skillContract.Id).FirstOrDefault();

                    if (skillData != null)
                    {
                        experience = skillData.Experience;
                        level = skillData.Level;
                    }
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

                skillHandler.SetSpriteToSkill(tempSkill.SkillCloseSprite);
                skillHandler.SetSkillExperience(skillContract.Experience);
                skillHandler.SetSkillCurrentExperience(experience);
                skillHandler.SetSkillLevel(level);
                skillHandler.SetInformationObject(panelObject);
                skillHandler.SetSpawnCloseButtonAnTransform(_spawnCloseButtonForSkill, _spawnScrollingParent);
                skillHandler.ParseSkill(clientProcessor, tempSkill, skillContract, this);

                if (!skillGameObject.TryGetComponent(out Button button))
                    throw new MissingComponentException(nameof(Button));

                Debug.Log($"Added new Skill {skillContract.Name} to list");
                iterator++;
            }

            yield break;
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
    }
}