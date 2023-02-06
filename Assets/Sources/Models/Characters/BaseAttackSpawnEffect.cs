using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Effects;

namespace Assets.Sources.Models.Characters
{
    public sealed class BaseAttackSpawnEffect : MonoBehaviour
    {
        [SerializeField] private FireBallLogic _fire;
        [SerializeField] private GameObject _effectTrigger;
        [SerializeField] private Transform _hand;

        private BaseClass _baseClass;
        private ObjectData _data;

        public void Init(BaseClass baseClass, ObjectData data)
        {
            _baseClass = baseClass;
            _data = data;
        }

        public IEnumerator SpawnFireBaseAttack(float endDuration)
        {
            if (_baseClass == BaseClass.Mage)
            {
                yield return new WaitForSecondsRealtime(endDuration);
                GameObject obj = Instantiate(_fire.gameObject, null)
                    .GetComponent<FireBallLogic>().Init(_data.GameObjectModel.transform, _effectTrigger);
                obj.transform.position = _hand.transform.position;
            }
        }
    }
}