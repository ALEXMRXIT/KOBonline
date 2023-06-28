using System;
using UnityEngine;
using System.Linq;
using Assets.Sources.UI;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using System.Collections.Generic;
using Assets.Sources.Models.Characters.Tools;
using Assets.Sources.Models.Characters.Skills;

namespace Assets.Sources.Models.Characters
{
    public sealed class VisualModelOfAbilityExecution : MonoBehaviour
    {
        private VisualAbility _ability;
        private Transform _playerCanvas;
        private List<VisualAbility> _visualAbilities;
        private IReadOnlyCollection<SkillContract> _skillContracts;
        private IReadOnlyCollection<Skill> _skills;
        private Coroutine _coroutine;
        private AbilityEffectLink _abilityEffectLink;

        public void Init(ClientProcessor clientProcessor)
        {
            _visualAbilities = new List<VisualAbility>();

            _skillContracts = clientProcessor.GetSkillContracts;
            _skills = clientProcessor.GetSkills;
        }

        public void SetAblilityEffect(AbilityEffectLink abilityEffectLink)
        {
            _abilityEffectLink = abilityEffectLink;
        }

        public void SetAbilityPrefab(VisualAbility visualAbility)
        {
            _ability = visualAbility;
        }

        public void SetPlayerTransformWithCanvas(Transform player)
        {
            _playerCanvas = player;
        }

        public void StartHandler()
        {
            _coroutine = StartCoroutine(HandlerAbility());
        }

        public void StopHandler()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        public void AddVisualAbility(long skillId, int level = 0)
        {
            if (_skillContracts.Count == 0 || _skills.Count == 0)
                return;

            Sprite spriteAbility = _skills.FirstOrDefault(s => s.Id == skillId).SkillSprite;
            SkillContract skillContract = _skillContracts.FirstOrDefault(s => s.Id == skillId);

            int timeUse = 0;
            if (skillContract.DeBuff) timeUse = skillContract.TimeBuffUse[level - 1];
            else timeUse = skillContract.TimeUse;

            if (!Instantiate(_ability.gameObject, _playerCanvas).TryGetComponent<VisualAbility>(out VisualAbility ability))
                throw new ArgumentNullException(nameof(VisualAbility));

            switch (skillId)
            {
                case 1: ability.SetEffectForAbility(_abilityEffectLink.CreateMagicShieldPermanentEffect()); break;
                case 4: ability.SetEffectForAbility(_abilityEffectLink.CreateStrongBodyPermanentEffect()); break;
                case 5: ability.SetEffectForAbility(_abilityEffectLink.CreateHeroesPowerPermanentEffect()); break;
                default: ability.SetEffectForAbility(null); break;
            }

            ability.SetAbilityIcon(spriteAbility);
            ability.SetOriginalIntTime(timeUse);
            ability.SetAbilityText(Parser.ConvertTimeInt32ToTimeString(timeUse));
            _visualAbilities.Add(ability);
        }

        private IEnumerator HandlerAbility()
        {
            while (true)
            {
                foreach (VisualAbility visualAbility in _visualAbilities)
                {
                    if (!visualAbility.DecrementTime())
                    {
                        visualAbility.AbilitiIfNotDeactivate();
                        visualAbility.DestroyEffectForAbility();
                        Destroy(visualAbility.gameObject);
                        yield return null;
                    }
                    else
                    {
                        visualAbility.SetAbilityText(Parser.
                            ConvertTimeInt32ToTimeString(visualAbility.RemainingRunningTime()));
                    }
                }

                _visualAbilities.RemoveAll(x => !x.AbilitiIfNotDeactivate());
                yield return new WaitForSecondsRealtime(1f);
            }
        }
    }
}