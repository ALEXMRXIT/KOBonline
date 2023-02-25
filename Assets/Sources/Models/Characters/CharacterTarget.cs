using UnityEngine;
using Assets.Sources.Models.Base;

namespace Assets.Sources.Models.Characters
{
    public sealed class CharacterTarget : MonoBehaviour
    {
        private Transform _target;
        private ObjectData _objectData;
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

        public void SetTarget(Transform target, ObjectData data)
        {
            _target = target;
            _objectData = data;
            _isTargetHooked = true;
        }

        public Transform GetCurrentTarget()
        {
            return _target;
        }

        public void ClearTarget()
        {
            _target = null;
            _isTargetHooked = false;
        }

        public bool IsObjectDeath()
        {
            return _objectData.IsDeath == true;
        }
    }
}