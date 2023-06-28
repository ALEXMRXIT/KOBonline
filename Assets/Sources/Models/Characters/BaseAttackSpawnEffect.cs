using System;
using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Effects;
using static UnityEngine.ParticleSystem;

namespace Assets.Sources.Models.Characters
{
    [RequireComponent(typeof(TextView))]
    public sealed class BaseAttackSpawnEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _swordEffects;
        [Space, SerializeField] private FireBallLogic _fire;
        [Space, SerializeField] private GameObject _spawnEffectStrongSkill;
        [SerializeField] private GameObject _spawnEffectFireBallSkill;
        [SerializeField] private GameObject _spawnEffectFlamesSkill;
        [SerializeField] private GameObject _effectTrigger;
        [SerializeField] private GameObject _effectMagicHitTrigger;
        [SerializeField] private GameObject _effectFlamesHitTrigger;
        [SerializeField] private AudioSource _baseAttackSound;
        [SerializeField] private AudioSource _flamesAttackSound;
        [Space, SerializeField] private AudioSource _magicAttackSound;
        [SerializeField] private Transform _hand;

        private BaseClass _baseClass;
        private Transform _enemyTransform;
        private ObjectData _player;
        private ObjectData _enemy;

        public void Init(BaseClass baseClass, ObjectData player, ObjectData enemy)
        {
            _baseClass = baseClass;
            _player = player;
            _enemy = enemy;
            _enemyTransform = enemy.GameObjectModel.transform;
        }

        public void PlaySwordEffect(int code)
        {
            if (code == 0) _swordEffects.SetActive(false);
            else if (code == 1) _swordEffects.SetActive(true);
        }

        public void SpawnStrongHitEffect()
        {
            Instantiate(_spawnEffectStrongSkill, transform);
        }

        public void SpawnFireBallHitEffect()
        {
            Instantiate(_spawnEffectFireBallSkill, transform);
        }

        public void SpawnFlameBallHitEffect()
        {
            Instantiate(_spawnEffectFlamesSkill, transform);
        }

        public void PlaySound(int code)
        {
            if (code == 1) _magicAttackSound.Play();
            else if (code == 0) _baseAttackSound.Play();
            else if (code == 2) _flamesAttackSound.Play();
        }

        public void SpawnFireBaseAttack(int code)
        {
            GameObject obj = null;

            if (code == 2) // flames attack code
            {
                obj = Instantiate(_fire.gameObject, null).GetComponent<FireBallLogic>()
                    .Init(_enemyTransform, _enemy, _effectFlamesHitTrigger, _player);
            }
            else if (code == 1) // mage attack code
            {
                obj = Instantiate(_fire.gameObject, null).GetComponent<FireBallLogic>()
                    .Init(_enemyTransform, _enemy, _effectMagicHitTrigger, _player);
            }
            else if (code == 0) // base attack code
            {
                obj = Instantiate(_fire.gameObject, null).GetComponent<FireBallLogic>()
                    .Init(_enemyTransform, _enemy, _effectTrigger, _player);
            }

            if (obj != null)
                obj.transform.position = _hand.transform.position;
        }
    }
}