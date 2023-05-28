using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    [System.Serializable]
    internal sealed class Skill
    {
        public int Id;
        public Sprite SkillSprite;
        public Sprite SkillCloseSprite;

        [HideInInspector] public GameObject SkillObject;
        [HideInInspector] public Button SkillButton;
    }

    public sealed class SkillManager : MonoBehaviour
    {
        [SerializeField] private GameObject _skillObject;
        [SerializeField] private Transform _spawnContentSkills;
        [SerializeField] private List<Skill> _skills = new List<Skill>();

        private INetworkProcessor _networkProcessor;
        private List<KeyValuePair<int, KeyValuePair<Skill, SkillContract>>> _skillContracts;

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

                if (!skillGameObject.TryGetComponent(out SkillHandler skillHandler))
                    throw new MissingComponentException(nameof(SkillHandler));

                int experience = 0;
                int level = 0;

                if (_networkProcessor.GetParentObject().GetSkillDatas.Count > 0)
                {
                    if (iterator > _networkProcessor.GetParentObject().GetSkillDatas.Count)
                    {
                        experience = _networkProcessor.GetParentObject().GetSkillDatas[iterator].Experience;
                        level = _networkProcessor.GetParentObject().GetSkillDatas[iterator].Level;
                    }
                }

                tempSkill.SkillObject = skillGameObject;
                skillHandler.SetSpriteToSkill(tempSkill.SkillCloseSprite);
                skillHandler.SetSkillExperience(skillContract.Experience);
                skillHandler.SetSkillCurrentExperience(experience);
                skillHandler.SetSkillLevel(level);
                skillHandler.ParseSkill();

                if (!skillGameObject.TryGetComponent(out Button button))
                    throw new MissingComponentException(nameof(Button));

                tempSkill.SkillButton = button;
                tempSkill.SkillButton.onClick.AddListener(() => InternalButtonClickHandler(skillContract.Id));

                Debug.Log($"Added new Skill {skillContract.Name} to list");
                iterator++;
            }

            yield break;
        }

        private void InternalButtonClickHandler(int id)
        {
            KeyValuePair<Skill, SkillContract> skill = InternalGetSkillById(id);
            Debug.Log($"Skill click: {skill.Value.Name}");
        }

        private KeyValuePair<Skill, SkillContract> InternalGetSkillById(int id)
        {
            KeyValuePair<Skill, SkillContract> skill = default;
            foreach (KeyValuePair<int, KeyValuePair<Skill, SkillContract>> item in _skillContracts)
            {
                if (item.Value.Value.Id == id)
                {
                    skill = item.Value;
                    break;
                }    
            }

            return skill;
        }
    }
}