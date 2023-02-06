using UnityEngine;

namespace Assets.Sources.Models.Effects
{
    public sealed class FireBallLogic : MonoBehaviour
    {
        private Transform _target;
        private GameObject _effectTrigger;

        public GameObject Init(Transform target, GameObject effectTrigger)
        {
            _target = target;
            _effectTrigger = effectTrigger;

            return gameObject;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10f);
            float dist = Vector3.Distance(_target.position, transform.position);
            if (dist < 1f)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameObject gameObject = Instantiate(_effectTrigger, null);
            gameObject.transform.position = new Vector3(_target.position.x, -3.86f, _target.position.z);

            Destroy(gameObject, 2f);
        }
    }
}