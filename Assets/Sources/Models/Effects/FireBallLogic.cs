using System;
using UnityEngine;
using Assets.Sources.Models.Base;

namespace Assets.Sources.Models.Effects
{
    public sealed class FireBallLogic : MonoBehaviour
    {
        private Transform _target;
        private GameObject _effectTrigger;
        private Action<ObjectData> _call;
        private ObjectData _objectData;

        public GameObject Init(Transform target, GameObject effectTrigger,
            ObjectData objectData, Action<ObjectData> call)
        {
            _target = target;
            _effectTrigger = effectTrigger;
            _objectData = objectData;
            _call = call;

            return gameObject;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10f);
            float dist = Vector3.Distance(_target.position, transform.position);
            if (dist < 1f)
            {
                _call?.Invoke(_objectData);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            GameObject gameObject = Instantiate(_effectTrigger, null);
            gameObject.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

            Destroy(gameObject, 2f);
        }
    }
}