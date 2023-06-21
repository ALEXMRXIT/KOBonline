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
        [SerializeField] private VisualAbility _ability;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _enemy;

        private List<VisualAbility> _visualAbilities;
        private IReadOnlyCollection<SkillContract> _skillContracts;
        private IReadOnlyCollection<Skill> _skills;
        private Coroutine _coroutine;

        public void Init(ClientProcessor clientProcessor)
        {
            _visualAbilities = new List<VisualAbility>();

            _skillContracts = clientProcessor.GetSkillContracts;
            _skills = clientProcessor.GetSkills;
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

        public void AddVisualAbility(long skillId, bool forBot = false)
        {
            if (_skillContracts.Count == 0 || _skills.Count == 0)
                return;

            Sprite spriteAbility = _skills.FirstOrDefault(s => s.Id == skillId).SkillSprite;
            int timeUse = _skillContracts.FirstOrDefault(s => s.Id == skillId).TimeUse;

            if (!Instantiate(_ability.gameObject, forBot ? _enemy : _player)
                    .TryGetComponent<VisualAbility>(out VisualAbility ability))
                throw new ArgumentNullException(nameof(VisualAbility));

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