using UnityEngine;
using System.Collections;

namespace Assets.Sources.Models.Characters.Skills
{
    public sealed class AbilityEffectLink : MonoBehaviour
    {
        [SerializeField] private GameObject _strongBodyEffectStart;
        [SerializeField] private GameObject _strongBodyEffectEnd;
        [SerializeField] private GameObject _heroesPowerEffectStart;
        [SerializeField] private GameObject _heroesPowerEffectEnd;
        [SerializeField] private GameObject _mageShieldEffectStart;
        [SerializeField] private GameObject _mageShieldEffectEnd;
        [SerializeField] private GameObject _strongBodyPermanentEffect;
        [SerializeField] private GameObject _heroesPowerPermanentEffect;
        [SerializeField] private GameObject _mageShieldPermanentEffect;

        public void StrongBodyEffectPlay()
        {
            StartCoroutine(InternalStrongBodyEffectPlay());
        }

        public void HeroesPowerEffectPlay()
        {
            StartCoroutine(InternalHeroesPowerEffectPlay());
        }

        public void MagicShieldEffectPlay()
        {
            StartCoroutine(InternalMagicShieldEffectPlay());
        }

        public GameObject CreateStrongBodyPermanentEffect()
        {
            return Instantiate(_strongBodyPermanentEffect, transform);
        }

        public GameObject CreateHeroesPowerPermanentEffect()
        {
            return Instantiate(_heroesPowerPermanentEffect, transform);
        }

        public GameObject CreateMagicShieldPermanentEffect()
        {
            return Instantiate(_mageShieldPermanentEffect, transform);
        }

        private IEnumerator InternalStrongBodyEffectPlay()
        {
            GameObject first = Instantiate(_strongBodyEffectStart, transform);
            yield return new WaitForSecondsRealtime(0.8f);
            GameObject second = Instantiate(_strongBodyEffectEnd, transform);
            yield break;
        }

        private IEnumerator InternalHeroesPowerEffectPlay()
        {
            GameObject first = Instantiate(_heroesPowerEffectStart, transform);
            yield return new WaitForSecondsRealtime(0.8f);
            GameObject second = Instantiate(_heroesPowerEffectEnd, transform);
            yield break;
        }

        private IEnumerator InternalMagicShieldEffectPlay()
        {
            GameObject first = Instantiate(_mageShieldEffectStart, transform);
            yield return new WaitForSecondsRealtime(0.8f);
            GameObject second = Instantiate(_mageShieldEffectEnd, transform);
            yield break;
        }
    }
}