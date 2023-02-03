using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationAttack : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationBaseAttack;
        public bool KnocksDownOtherAnimation => false;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
            animator.SetFloat("AttackSpeed", speed);
        }
    }
}