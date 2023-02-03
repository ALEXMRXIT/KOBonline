using UnityEngine;

namespace Assets.Sources.MechanicUI
{
    public sealed class HUDFollowingCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        [Space]
        [SerializeField] private Transform _worldNode;
        [SerializeField] private Transform _screenNode;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Vector3 newPosition = _camera.WorldToScreenPoint(_worldNode.position + _offset);

            _screenNode.position = newPosition;
        }
    }
}