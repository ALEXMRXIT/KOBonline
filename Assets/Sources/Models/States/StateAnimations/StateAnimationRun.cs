using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationRun : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationRun;
        public bool KnocksDownOtherAnimation => true;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
            animator.SetFloat("MoveSpeed", speed);
        }
    }
}