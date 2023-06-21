using UnityEngine;

namespace Assets.Sources.Tools
{
    public sealed class DestroyTimeObject : MonoBehaviour
    {
        [SerializeField] private float _time;

        private void OnEnable()
        {
            Destroy(gameObject, _time);
        }
    }
}