using UnityEngine;

namespace Assets.Sources
{
    internal sealed class DontDestroyComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}