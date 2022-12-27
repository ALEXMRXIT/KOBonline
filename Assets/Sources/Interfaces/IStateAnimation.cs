using UnityEngine;
using Assets.Sources.Enums;

namespace Assets.Sources.Interfaces
{
    public interface IStateAnimation
    {
        public StateAnimationIndex AnimationIndex { get; }
        public bool KnocksDownOtherAnimation { get; }

        public void ExecuteAnimation(Animator animator);
    }
}