using System;
using UnityEngine;
using System.Collections;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters;

namespace Assets.Sources.Models.Effects
{
    public sealed class FireBallLogic : MonoBehaviour
    {
        private Transform _target;
        private GameObject _effectTrigger;
        private ObjectData _objectData;
        private bool _moving = true;
        private int _damageIndex;

        public GameObject Init(Transform target,
            GameObject effectTrigger, ObjectData objectData, int damageIndex)
        {
            _target = target;
            _effectTrigger = effectTrigger;
            _objectData = objectData;
            _damageIndex = damageIndex;

            return gameObject;
        }
        
        private void Update()
        {
            if (!_moving)
                return;

            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10f);
            if (_objectData.ClientTextView.ShowDamage(_objectData, _damageIndex))
            {
                _moving = false;
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_target == null)
                return;

            GameObject gameObject = Instantiate(_effectTrigger, null);
            gameObject.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

            Destroy(gameObject, 2f);
        }
    }
}