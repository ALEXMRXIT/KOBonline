using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Models.Characters.Smile
{
    public sealed class SmileAPI : MonoBehaviour
    {
        public void ShowSmile()
        {
            gameObject.SetActive(true);
        }

        public void HideSmile()
        {
            gameObject.SetActive(false);
        }
    }
}