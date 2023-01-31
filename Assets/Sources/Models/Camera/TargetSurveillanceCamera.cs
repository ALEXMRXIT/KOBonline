using UnityEngine;

namespace Assets.Sources.Models.Camera
{
    public sealed class TargetSurveillanceCamera : MonoBehaviour
    {
        private bool _init;
        private Vector3 _offset;
        private Transform _target;

        private void LateUpdate()
        {
            if (!_init)
                return;

            transform.position = _target.position + _offset;
        }

        public void SetupCameraMovedForTarget(Transform target)
        {
            _offset = transform.position - target.position;
            _target = target;
            _init = true;
        }
    }
}