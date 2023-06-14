using UnityEngine;
using System.Collections;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.States;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(BaseAttackEffect))]
    [RequireComponent(typeof(StateMachineAnimation))]
    public sealed class CharacterState : MonoBehaviour
    {
        private StateMachineAnimation _stateMachineAnimation;
        private BaseAttackEffect _baseAttackEffect;
        private BaseAttackSpawnEffect _baseAttackSpawnEffect;
        private Coroutine _coroutine;

        private void Start()
        {
            _stateMachineAnimation = GetComponent<StateMachineAnimation>();
            _baseAttackEffect = GetComponent<BaseAttackEffect>();
            _baseAttackSpawnEffect = GetComponent<BaseAttackSpawnEffect>();
        }

        public void CheckGettingComponent()
        {
            if (_stateMachineAnimation == null)
                _stateMachineAnimation = GetComponent<StateMachineAnimation>();

            _stateMachineAnimation.CheckGettingComponent();
        }

        public void SetCharacterState(IStateAnimation stateAnimation, float speed = 1f)
        {
            if (stateAnimation is StateAnimationAttack || stateAnimation is StateAnimationAttackMagic)
                _coroutine = StartCoroutine(PlayOneEffect(speed));

            _stateMachineAnimation.SetAnimation(stateAnimation, speed);
        }

        private IEnumerator PlayOneEffect(float speedAttack)
        {
            yield return _baseAttackEffect.PlayOneEffect(speedAttack, _baseAttackSpawnEffect);
        }
    }
}