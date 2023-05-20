using UnityEngine;

namespace Assets.Sources.Models.Base
{
    public class CanvasFollowingUI : MonoBehaviour
    {
        private Transform _object;

        private void Awake()
        {
            _object = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(_object);
        }
    }
}