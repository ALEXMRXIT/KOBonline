using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;
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

        public GameObject Init(Transform target, ObjectData enemyData,
            GameObject effectTrigger, ObjectData objectData)
        {
            _target = target;
            _enemyData = enemyData;
            _effectTrigger = effectTrigger;
            _objectData = objectData;

            if (_objectData.ClientTextView.ShowDamage(_objectData, _enemyData))
            {
                GameObject gameObjectD = Instantiate(_effectTrigger, null);
                gameObjectD.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

                Destroy(gameObject);
            }

            return gameObject;
        }
    }
}