using UnityEngine;

namespace Assets.Sources.MechanicUI
{
    public sealed class MessageBox : MonoBehaviour
    {
        public static MessageBox Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}