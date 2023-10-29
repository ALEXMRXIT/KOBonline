using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Scenes
{
    public sealed class Logo : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(PlayLogo(8f));
        }

        private IEnumerator PlayLogo(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            SceneManager.LoadScene("Login");
        }
    }
}