using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Effects;

namespace Assets.Sources.Models.Characters
{
    [RequireComponent(typeof(TextView))]
    public sealed class BaseAttackSpawnEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _swordEffects;
        [Space, SerializeField] private FireBallLogic _fire;
        [Space, SerializeField] private GameObject _spawnEffectStrongSkill;
        [SerializeField] private GameObject _effectTrigger;
        [SerializeField] private GameObject _strongHitTrigger;
        [SerializeField] private AudioSource _baseAttackSound;
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

        public void PlaySound(int code)
        {
            if (code == 1)
            {
                _magicAttackSound.Play();
                return;
            }

            _baseAttackSound.Play();
        }

        public void SpawnFireBaseAttack(int code)
        {
            GameObject obj = null;

            if (code == 1)
            {
                obj = Instantiate(_fire.gameObject, null).GetComponent<FireBallLogic>()
                    .Init(_enemyTransform, _enemy, _strongHitTrigger, _player, false);
                return;
            }
            else if (code == 0)
                obj = Instantiate(_fire.gameObject, null).GetComponent<FireBallLogic>()
                    .Init(_enemyTransform, _enemy, _effectTrigger, _player, _baseClass == BaseClass.Mage);

            if (obj != null)
                obj.transform.position = _hand.transform.position;
        }
    }
}