using UnityEngine;
using System.Collections;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models.States
{
    [RequireComponent(typeof(Animator))]
    public sealed class StateMachineAnimation : MonoBehaviour
    {
        private Animator _animator;
        private IStateAnimation _stateAnimation;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            SetAnimation(new StateAnimationIdle());
        }

        public void SetAnimation(IStateAnimation stateAnimation)
        {
            _stateAnimation = stateAnimation;
            _stateAnimation.ExecuteAnimation(_animator);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SetAnimation(new StateAnimationRun());

            if (Input.GetKeyDown(KeyCode.W))
                SetAnimation(new StateAnimationCastSpell1());

            if (Input.GetKeyDown(KeyCode.E))
                SetAnimation(new StateAnimationCastSpell2());

            if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.E))
                SetAnimation(new StateAnimationIdle());
        }
    }
}