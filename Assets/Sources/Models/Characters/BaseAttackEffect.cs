using System.Linq;
using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Base;

#pragma warning disable

namespace Assets.Sources.Models.Characters
{
    public sealed class BaseAttackEffect : MonoBehaviour
    {
        private INetworkProcessor _networkProcessor;
        private BaseClass _currentBaseClass;

        [SerializeField] private ParticleSystem[] _effect;

        public BaseAttackEffect Init(BaseClass baseClass)
        {
            _networkProcessor = ClientProcessor.Instance;
            _currentBaseClass = baseClass;

            return this;
        }

        public IEnumerator PlayEffectLoop(float duration, BaseAttackSpawnEffect baseAttackSpawnEffect)
        {
            int damageIndex = 1;
            bool nullEffectReference = false;
            if (_effect.Length > 0)
            {
                if (_effect[0] == null)
                    nullEffectReference = true;
            }
            else if (_effect == null || _effect.Length == 0)
                nullEffectReference = true;

            if (!nullEffectReference)
            {
                foreach (ParticleSystem particleSystem in _effect)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = duration;
                }
            }

            while (true)
            {
                float lengthClip = 0f;
                float pauseTakeDamage = 0f;
                if (_currentBaseClass == BaseClass.Mage)
                {
                    lengthClip = 2.283334f / duration;
                    pauseTakeDamage = (lengthClip * 40f) / 100f;
                }
                else if (_currentBaseClass == BaseClass.Warrior)
                {
                    lengthClip = 1.500f / duration;
                    pauseTakeDamage = (lengthClip * 50f) / 100f;
                }

                if (!nullEffectReference)
                    _effect[0]?.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(pauseTakeDamage);
                baseAttackSpawnEffect.SpawnFireBaseAttack(damageIndex);
                yield return new WaitForSecondsRealtime(lengthClip - pauseTakeDamage);
                if (!nullEffectReference)
                    _effect[0]?.gameObject.SetActive(false);

                damageIndex++;
            }
        }
    }
}