using System;
using UnityEngine;
using Assets.Sources.Enums;
using System.Collections.Generic;

namespace Assets.Sources.Models
{
    public struct PoolModelObject
    {
        public GameObject _object;
        public Component _component;
    }

    [Serializable]
    internal sealed class ModelPoolingWithComponent
    {
        [Space] public PoolObjecPoolObjectIdentifier _identifier;
        public GameObject _object;
        public int count;
        public PoolModelObject[] _poolModel;

        private int _add = 0;

        public void TryFindObjectComponentAndInitialization
            <T>(int addDefaultIfPoolLow = 5) where T : Component
        {
            _poolModel = new PoolModelObject[count];
            _add = addDefaultIfPoolLow;

            for (int iterator = 0; iterator < count; iterator++)
            {
                _poolModel[iterator] = new PoolModelObject();
                _poolModel[iterator]._object = GameObject.Instantiate(_object);

                if (!_poolModel[iterator]._object.TryGetComponent<T>(out T component))
                    throw new MissingComponentException(nameof(component));

                _poolModel[iterator]._component = component;
                _poolModel[iterator]._object.SetActive(false);
            }
        }

        public PoolModelObject TryGetModelNext<T>(int skipOffset = 0)
            where T : Component
        {
            for (int iterator = skipOffset; iterator < _poolModel.Length; iterator++)
            {
                if (!_poolModel[iterator]._object.activeSelf)
                {
                    _poolModel[iterator]._object.SetActive(true);
                    return _poolModel[iterator];
                }
            }

            UpdatePoolingInternal<T>(_add);
            TryGetModelNext<T>(_poolModel.Length - _add);

            return default;
        }

        private void UpdatePoolingInternal<T>(int capacity)
            where T : Component
        {
            if (capacity == 0)
                return;

            int length = _poolModel.Length + capacity;
            PoolModelObject[] temp = new PoolModelObject[length];

            Array.Copy(_poolModel, 0, temp, 0, _poolModel.Length);
            _poolModel = temp;

            for (int iterator = _poolModel.Length; iterator < length; iterator++)
            {
                _poolModel[iterator] = new PoolModelObject();
                _poolModel[iterator]._object = GameObject.Instantiate(_object);

                if (!_poolModel[iterator]._object.TryGetComponent<T>(out T component))
                    throw new MissingComponentException(nameof(component));

                _poolModel[iterator]._component = component;
                _poolModel[iterator]._object.SetActive(false);
            }
        }
    }

    public sealed class PoolObjects : MonoBehaviour
    {
        [SerializeField]
        private List<ModelPoolingWithComponent> _modelPoolings
            = new List<ModelPoolingWithComponent>();

        private void Start()
        {
            foreach (ModelPoolingWithComponent model in _modelPoolings)
                model.TryFindObjectComponentAndInitialization<ParticleSystem>();
        }

        public PoolModelObject GetModelByIdentifier<T>(PoolObjecPoolObjectIdentifier poolObjecPoolObjectIdentifier)
            where T : Component
        {
            for (int iterator = 0; iterator < _modelPoolings.Count; iterator++)
            {
                if (_modelPoolings[iterator]._identifier == poolObjecPoolObjectIdentifier)
                    return _modelPoolings[iterator].TryGetModelNext<T>();
            }

            return default;
        }
    }
}