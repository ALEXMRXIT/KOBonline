using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationIdle : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationIdle;

        public void ExecuteAnimation(Animator animator)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
        }
    }
}