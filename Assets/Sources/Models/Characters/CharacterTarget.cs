using UnityEngine;

namespace Assets.Sources.Models.Characters
{
    public sealed class CharacterTarget : MonoBehaviour
    {
        private Transform _target;
        private bool _isTargetHooked;

        private void Awake()
        {
            _target = null;
            _isTargetHooked = false;
        }

        public bool IsTargetHook()
        {
            return _isTargetHooked;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _isTargetHooked = true;
        }

        public Transform GetCurrentTarget()
        {
            return _target;
        }
    }
}