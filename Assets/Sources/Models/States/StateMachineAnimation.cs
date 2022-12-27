using UnityEngine;
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
    }
}