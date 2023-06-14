using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationAttackMagic : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationAttackMagic;

        public void ExecuteAnimation(Animator animator, float speed = 1f)
        {
            animator.Play("MagicAttack");
            animator.SetFloat("AttackSpeed", speed);
        }
    }
}