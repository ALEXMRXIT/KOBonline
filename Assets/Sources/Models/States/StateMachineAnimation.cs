using UnityEngine;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models.States
{
    [RequireComponent(typeof(Animator))]
    public sealed class StateMachineAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            SetAnimation(new StateAnimationIdle());
        }

        public void CheckGettingComponent()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        public void SetAnimation(IStateAnimation stateAnimation, float speed = 1f)
        {
            stateAnimation.ExecuteAnimation(_animator, speed);
        }
    }
}