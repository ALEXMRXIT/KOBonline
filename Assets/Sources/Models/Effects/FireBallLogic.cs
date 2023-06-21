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
        private ObjectData _enemyData;
        private ObjectData _objectData;
        private bool _moving = true;
        private bool _checkingDistance;

        public GameObject Init(Transform target, ObjectData enemyData,
            GameObject effectTrigger, ObjectData objectData, bool checkDistance)
        {
            _target = target;
            _enemyData = enemyData;
            _effectTrigger = effectTrigger;
            _objectData = objectData;
            _checkingDistance = checkDistance;

            return gameObject;
        }

        private void Update()
        {
            if (!_moving)
                return;

            if (_checkingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10f);
                float distance = Vector3.Distance(transform.position, _target.position);

                if (distance < 0.1f)
                {
                    if (_objectData.ClientTextView.ShowDamage(_objectData, _enemyData))
                    {
                        _moving = false;

                        GameObject gameObjectD = Instantiate(_effectTrigger, null);
                        gameObjectD.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (_objectData.ClientTextView.ShowDamage(_objectData, _enemyData))
                {
                    _moving = false;

                    GameObject gameObjectD = Instantiate(_effectTrigger, null);
                    gameObjectD.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

                    Destroy(gameObject);
                }
            }
        }
    }
}