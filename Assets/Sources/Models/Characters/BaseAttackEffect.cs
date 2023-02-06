using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;

namespace Assets.Sources.Models.Characters
{
    public sealed class BaseAttackEffect : MonoBehaviour
    {
        private INetworkProcessor _networkProcessor;
        private BaseClass _currentBaseClass;

        [SerializeField] private ParticleSystem[] _effect;
        [SerializeField] private float _effectSpeed = 1;

        public BaseAttackEffect Init(BaseClass baseClass)
        {
            _networkProcessor = ClientProcessor.Instance;
            _currentBaseClass = baseClass;

            return this;
        }

        public IEnumerator PlayEffectLoop(float duration, BaseAttackSpawnEffect baseAttackSpawnEffect)
        {
            if (_currentBaseClass == BaseClass.Mage)
            {
                float lengthClip = 2.283334f / duration;
                foreach (ParticleSystem particleSystem in _effect)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = duration;
                }

                _effect[0].gameObject.SetActive(true);
                yield return baseAttackSpawnEffect.SpawnFireBaseAttack(lengthClip);
                _effect[0].gameObject.SetActive(false);
            }
        }
    }
}