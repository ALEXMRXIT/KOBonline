using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationRun : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationRun;
        public bool KnocksDownOtherAnimation => true;

        public void ExecuteAnimation(Animator animator)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
        }
    }
}