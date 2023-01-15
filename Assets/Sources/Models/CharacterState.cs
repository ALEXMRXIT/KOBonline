using UnityEngine;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.States;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(StateMachineAnimation))]
    public sealed class CharacterState : MonoBehaviour
    {
        private StateMachineAnimation _stateMachineAnimation;
        private bool _isAttacked = false;

        private void Start()
        {
            _stateMachineAnimation = GetComponent<StateMachineAnimation>();
        }

        public void CheckGettingComponent()
        {
            if (_stateMachineAnimation == null)
                _stateMachineAnimation = GetComponent<StateMachineAnimation>();

            _stateMachineAnimation.CheckGettingComponent();
        }

        public void SetCharacterState(IStateAnimation stateAnimation)
        {
            if (stateAnimation is StateAnimationAttack || _isAttacked)
            {
                if (stateAnimation.KnocksDownOtherAnimation)
                    _isAttacked = false;
                else
                {
                    _stateMachineAnimation.SetAnimation(stateAnimation);
                    _isAttacked = true;
                }
            }

            _stateMachineAnimation.SetAnimation(stateAnimation);
        }
    }
}