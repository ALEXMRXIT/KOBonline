using UnityEngine;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.States.StateAnimations;

#pragma warning disable CS4014

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(CharacterState))]
    public sealed class CharacterMovement : MonoBehaviour
    {
        private CharacterState _characterState;
        private CharacterTarget _characterTarget;
        private ObjectData _objectData;
        private StateAnimationRun _animationRun;
        private StateAnimationAttack _animationAttack;

        public void Init(CharacterTarget characterTarget, ObjectData data)
        {
            _characterState = GetComponent<CharacterState>();

            _characterTarget = characterTarget;
            _objectData = data;

            _animationRun = new StateAnimationRun();
            _animationAttack = new StateAnimationAttack();
        }

        private void Update()
        {
            if (!_characterTarget.IsTargetHook())
                return;

            float dist = Vector3.Distance(transform.position,
                _characterTarget.GetCurrentTarget().position);
            if (dist < _objectData.ObjectContract.AttackDistance)
            {
                _characterState.SetCharacterState(_animationAttack, _objectData.ObjectContract.AttackSpeed);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position,
                _characterTarget.GetCurrentTarget().position,
                Time.deltaTime * _objectData.ObjectContract.MoveSpeed);
            _characterState.SetCharacterState(_animationRun, _objectData.ObjectContract.MoveSpeed / 2f);

            dist = Vector3.Distance(transform.position, new Vector3(
                _objectData.UpdatePositionInServer.x,
                _objectData.UpdatePositionInServer.y,
                _objectData.UpdatePositionInServer.z));

            if (dist > 3f)
            {
                transform.position = new Vector3(
                    _objectData.UpdatePositionInServer.x,
                    _objectData.UpdatePositionInServer.y,
                    _objectData.UpdatePositionInServer.z);
            }
        }
    }
}