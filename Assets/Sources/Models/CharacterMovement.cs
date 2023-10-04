using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.States.StateAnimations;

#pragma warning disable

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(CharacterState))]
    public sealed class CharacterMovement : MonoBehaviour
    {
        private CharacterState _characterState;
        private CharacterTarget _characterTarget;
        private ObjectData _objectData;
        private StateAnimationRun _animationRun;
        private StateAnimationDeath _animationDeath;
        private StateAnimationIdle _animationIdle;
        private Coroutine _coroutine;
        private ClientProcessor _processor;

        public void Init(CharacterTarget characterTarget, ObjectData data, ClientProcessor processor)
        {
            _characterState = GetComponent<CharacterState>();

            _characterTarget = characterTarget;
            _objectData = data;
            _processor = processor;

            _animationRun = new StateAnimationRun();
            _animationDeath = new StateAnimationDeath();
            _animationIdle = new StateAnimationIdle();

            _coroutine = StartCoroutine(InternalUpdate());
        }

        public void InternalStopCoroutine()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator InternalUpdate()
        {
            while (true)
            {
                _processor.SendPacketAsync(InternalUpdatePosition.ToPacket(transform.position.x, transform.position.y, transform.position.z));
                yield return new WaitForSeconds(0.4f);
            }
        }

        private void Update()
        {
            if (!_characterTarget.IsTargetHook())
                return;

            if (_objectData.IsDeath)
            {
                _characterTarget.ClearTarget();
                return;
            }

            float dist = Vector3.Distance(transform.position,
                _characterTarget.GetCurrentTarget().position);

            if (dist < _objectData.ObjectContract.AttackDistance)
                return;

            transform.position = Vector3.MoveTowards(transform.position,
                _characterTarget.GetCurrentTarget().position,
                Time.deltaTime * _objectData.ObjectContract.MoveSpeed);
            _characterState.SetCharacterState(_animationRun, _objectData.ObjectContract.MoveSpeed / 2f);

            //dist = Vector3.Distance(transform.position, new Vector3(
            //    _objectData.UpdatePositionInServer.x,
            //    _objectData.UpdatePositionInServer.y,
            //    _objectData.UpdatePositionInServer.z));

            //if (dist > 4f)
            //{
            //    transform.position = new Vector3(
            //        _objectData.UpdatePositionInServer.x,
            //        _objectData.UpdatePositionInServer.y,
            //        _objectData.UpdatePositionInServer.z);
            //}
        }
    }
}