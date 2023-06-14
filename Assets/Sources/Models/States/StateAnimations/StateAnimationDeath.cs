using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationDeath : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationDeath;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.Play("Death");
        }
    }
}