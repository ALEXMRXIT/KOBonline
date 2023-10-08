using System;
using UnityEngine;
using System.Text;
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
        [SerializeField] private GameObject _windowMessage;
        [SerializeField] private AudioSource _open;
        [SerializeField] private AudioSource _close;

        public static ButtleUI Instance;

        private INetworkProcessor _networkProcessor;
        private Coroutine _coroutine;
        private StringBuilder _strinBuilder;

        private void Awake()
        {
            Instance = this;
            _networkProcessor = ClientProcessor.Instance;
            gameObject.SetActive(false);
            _windowMessage.SetActive(false);

            _strinBuilder = new StringBuilder(capacity: 512);
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(UpdateWaitTextRequest());
        }

        public void UpdateDescription(int[] values, int games)
        {
            try
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

                _strinBuilder.AppendFormat("<size=36>NUMBER OF PLAYERS WAITING:</size>\nCurrently playing: {0}\n\n", games);

                for (int iterator = 0; iterator < 5; iterator++)
                {
                    if (values == null || (values.Length - 1) < iterator)
                    {
                        switch (iterator)
                        {
                            case 0: _strinBuilder.AppendFormat("Level 1-4\t(0 players)\n"); break;
                            case 1: _strinBuilder.AppendFormat("Level 5-8\t(0 players)\n"); break;
                            case 2: _strinBuilder.AppendFormat("Level 9-12\t(0 players)\n"); break;
                            case 3: _strinBuilder.AppendFormat("Level 13-16\t(0 players)\n"); break;
                            case 4: _strinBuilder.AppendFormat("Level 17-20\t(0 players)"); break;
                        }
                    }
                    else
                    {
                        switch (iterator)
                        {
                            case 0: _strinBuilder.AppendFormat("Level 1-4\t({0} players)\n", values[iterator]); break;
                            case 1: _strinBuilder.AppendFormat("Level 5-8\t({0} players)\n", values[iterator]); break;
                            case 2: _strinBuilder.AppendFormat("Level 9-12\t({0} players)\n", values[iterator]); break;
                            case 3: _strinBuilder.AppendFormat("Level 13-16\t({0} players)\n", values[iterator]); break;
                            case 4: _strinBuilder.AppendFormat("Level 17-20\t({0} players)", values[iterator]); break;
                        }
                    }
                }

                _strinBuilder.AppendFormat("\n\n<color=#ff0000ff>Update time 1 second. Approximate waiting time is 5-10 seconds.</color>");

                _description.text = _strinBuilder.ToString();
                _strinBuilder.Clear();
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
            _close.Play();
        }

        public void ShowWindow()
        {
            _windowMessage.SetActive(true);
            _open.Play();
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