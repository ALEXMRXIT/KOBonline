using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.States.StateAnimations
{
    public sealed class StateAnimationCastSpell2 : IStateAnimation
    {
        public StateAnimationIndex AnimationIndex => StateAnimationIndex.AnimationCastSpell2;
        public bool KnocksDownOtherAnimation => true;

        public void ExecuteAnimation(Animator animator)
        {
            animator.SetInteger("Index", (int)AnimationIndex);
        }
    }
}