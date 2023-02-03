using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationAttackCast1 : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationMagicAttack1;
        public bool KnocksDownOtherAnimation => true;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
        }
    }
}