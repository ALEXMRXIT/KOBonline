using UnityEngine;

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(Target))]
    public sealed class CharacterFindTarget : MonoBehaviour
    {
        private Target _target;
        private bool _targetFinder;

        [Range(1f, 100f)]
        [SerializeField] private float FindRadius;

        public bool IsTargetFind => _targetFinder;

        private void Awake()
        {
            _target = GetComponent<Target>();
        }

        private void OnEnable()
        {
            _target.OnCharacterHooked += TargetSetHandler;
        }

        private void OnDisable()
        {
            _target.OnCharacterHooked -= TargetSetHandler;
        }

        private void TargetSetHandler(Character obj)
        {
            
        }

        public void FindTarget()
        {
            Collider[] colliders = new Collider[10];
            int numCol = Physics.OverlapSphereNonAlloc(transform.position, FindRadius, colliders);

            for (int iterator = 0; iterator < numCol; iterator++)
            {
                if (colliders[iterator].TryGetComponent<Character>(out Character character))
                {
                    Debug.Log($"Find {character.name}");
                    _target.SetTarget(character);
                    break;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                FindTarget();
        }
    }
}