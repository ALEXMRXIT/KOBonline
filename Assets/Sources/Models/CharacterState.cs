using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.States;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(StateMachineAnimation))]
    public sealed class CharacterState : MonoBehaviour
    {
        private StateMachineAnimation _stateMachineAnimation;
        private Coroutine _coroutine;
        private DateTime _lastStartMagicAttack;
        private INetworkProcessor _networkProcessor;
        private float _lengthAnimationClip;

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;

            List<ObjectData> players = _networkProcessor.GetParentObject().GetPlayers;
            if (players.Count > 0)
            {
                if (_networkProcessor.GetParentObject().GetPlayers.FirstOrDefault(x => !x.IsBot).ObjectContract.CharacterBaseClass == BaseClass.Warrior)
                    _lengthAnimationClip = 1.5f;
                else
                    _lengthAnimationClip = 2.283334f;
            }

            _stateMachineAnimation = GetComponent<StateMachineAnimation>();
        }

        public void CheckGettingComponent()
        {
            if (_stateMachineAnimation == null)
                _stateMachineAnimation = GetComponent<StateMachineAnimation>();

            _stateMachineAnimation.CheckGettingComponentNew();
        }

        public IStateAnimation GetCurrentPlayingAnimationState()
        {
            if (_stateMachineAnimation.GetCurrentStateAnimationClip() is StateAnimationAttackMagic)
            {
                if (_lastStartMagicAttack.CompareTo(DateTime.UtcNow) == 1)
                    return _stateMachineAnimation.GetCurrentStateAnimationClip();

                return null;
            }

            return null;
        }

        public void SetCharacterState(IStateAnimation stateAnimation, float speed = 1f)
        {
            if (stateAnimation is StateAnimationAttackMagic)
                _lastStartMagicAttack = DateTime.UtcNow.AddSeconds(_lengthAnimationClip / speed);

            _stateMachineAnimation.SetAnimation(stateAnimation, speed);
        }
    }
}