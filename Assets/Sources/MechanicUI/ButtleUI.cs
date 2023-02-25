using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class ButtleUI : MonoBehaviour
    {
        [SerializeField] private Text _description;

        public static ButtleUI Instance;

        private INetworkProcessor _networkProcessor;
        private Coroutine _coroutine;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(UpdateWaitTextRequest());
        }

        public void UpdateDescription(int[] values)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            _description.text = $"<size=36>NUMBER OF PLAYERS WAITING:</size>\n\n" +
                $"Level 1-5 ({values[0]} players).\nLevel 6-10 ({values[1]} players).\nLevel 11-15 ({values[2]} players).\n" +
                $"Level 16-20 ({values[3]} players).\n\n<color=#ff0000ff>Update time 1 second. " +
                $"Approximate waiting time is 5-10 seconds.</color>";
        }

        private IEnumerator UpdateWaitTextRequest()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                _networkProcessor.SendPacketAsync(UpdateTimeQuewe.ToPacket());
            }
        }

        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }
}