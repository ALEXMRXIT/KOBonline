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
        private bool _isAttacked = false;
        private Coroutine _coroutineEffectBaseAttack = null;

        private void Start()
        {
            _stateMachineAnimation = GetComponent<StateMachineAnimation>();
            _baseAttackEffect = GetComponent<BaseAttackEffect>();
        }

        public void CheckGettingComponent()
        {
            if (_stateMachineAnimation == null)
                _stateMachineAnimation = GetComponent<StateMachineAnimation>();

            _stateMachineAnimation.CheckGettingComponent();
        }

        public void SetCharacterState(IStateAnimation stateAnimation, float speed = 1f)
        {
            if (stateAnimation is StateAnimationAttack || _isAttacked)
            {
                if (stateAnimation.KnocksDownOtherAnimation)
                {
                    if (_isAttacked)
                        StopCoroutine(_coroutineEffectBaseAttack);

                    _isAttacked = false;
                }
                else
                {
                    _stateMachineAnimation.SetAnimation(stateAnimation, speed);
                    if (!_isAttacked)
                        _coroutineEffectBaseAttack = StartCoroutine(PlayEffect(speed));

                    _isAttacked = true;

                    return;
                }
            }

            if (_isAttacked)
                StopCoroutine(_coroutineEffectBaseAttack);
            _stateMachineAnimation.SetAnimation(stateAnimation, speed);
        }

        private IEnumerator PlayEffect(float speedAttack)
        {
            while (true)
            {
                yield return _baseAttackEffect.PlayEffectLoop(speedAttack);
            }
        }
    }
}