using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationAttack : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationBaseAttack;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.Play("BaseAttack");
            animator.SetFloat("AttackSpeed", speed);
        }
    }
}