using UnityEngine;
using System.Collections;

namespace Assets.Sources.Tools
{
    public sealed class DisableTimeObject : MonoBehaviour
    {
        [SerializeField] private float _time;

        private void OnEnable()
        {
            StartCoroutine(InternalActiveManager());
        }

        private IEnumerator InternalActiveManager()
        {
            yield return new WaitForSeconds(_time);
            gameObject.SetActive(false);
        }
    }
}