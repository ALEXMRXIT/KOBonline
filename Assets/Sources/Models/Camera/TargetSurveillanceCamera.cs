using UnityEngine;

namespace Assets.Sources.Models.Camera
{
    public sealed class TargetSurveillanceCamera : MonoBehaviour
    {
        private bool _init;
        private Vector3 _offset;
        private Transform _target;
        private Vector3 _originalPosition;
        private float _shakeDuration;
        private float _forceMagnitude;
        private bool _isShake = false;

        public static TargetSurveillanceCamera Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Shake(float time, float forceMagnitude)
        {
            if (!_isShake)
            {
                _originalPosition = transform.localPosition;
                _shakeDuration = time;
                _forceMagnitude = forceMagnitude;
                _isShake = true;
            }
        }

        private void LateUpdate()
        {
            if (!_init)
                return;

            if (_isShake)
            {
                _shakeDuration -= Time.deltaTime;

                float xPos = (Random.Range(-1f, 1f) * _forceMagnitude) + transform.localPosition.x;
                float yPos = (Random.Range(-1f, 1f) * _forceMagnitude) + transform.localPosition.y;
                float zPos = (Random.Range(-1f, 1f) * _forceMagnitude) + transform.localPosition.z;

                transform.localPosition = new Vector3(xPos, yPos, zPos);

                if (_shakeDuration <= 0.0f)
                {
                    transform.localPosition = _originalPosition;
                    _isShake = false;
                    _shakeDuration = 0.0f;
                }
            }
            else
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