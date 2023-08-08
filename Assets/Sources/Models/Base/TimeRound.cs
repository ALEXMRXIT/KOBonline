using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Sources.Models.Base
{
    public sealed class TimeRound : MonoBehaviour
    {
        [SerializeField] private Text _timeUI;

        private TimeSpan _timeSpan;
        private TimeSpan _seconds;
        private Coroutine _coroutine;
        private StringBuilder _stringBuilder;

        public void AddMinute(int minutes)
        {
            _timeSpan = _timeSpan.Add(new TimeSpan(0, minutes, 0));
            _timeUI.text = GetCurrentTime();
            _stringBuilder.Clear();
        }

        public void AddSeconds(int seconds)
        {
            _timeSpan = _timeSpan.Add(new TimeSpan(0, 0, seconds));
            _timeUI.text = GetCurrentTime();
            _stringBuilder.Clear();
        }

        public string GetCurrentTime()
        {
            if (_timeSpan.Minutes < 10)
                _stringBuilder.AppendFormat("0{0}:", _timeSpan.Minutes);
            else
                _stringBuilder.AppendFormat("{0}:", _timeSpan.Minutes);

            if (_timeSpan.Seconds < 10)
                _stringBuilder.AppendFormat("0{0}", _timeSpan.Seconds);
            else
                _stringBuilder.AppendFormat("{0}", _timeSpan.Seconds);

            return _stringBuilder.ToString();
        }

        public void Init()
        {
            _stringBuilder = new StringBuilder();
            _seconds = new TimeSpan(0, 0, 1);
            _timeSpan = new TimeSpan();
        }

        public void StartTime()
        {
            _coroutine = StartCoroutine(InternalStartTime());
        }

        public void StopTime()
        {
            if (_coroutine == null)
                return;

            StopCoroutine(_coroutine);
        }

        private IEnumerator InternalStartTime()
        {
            while (_timeSpan.Minutes > 0 || _timeSpan.Seconds > 0)
            {
                _timeSpan = _timeSpan.Subtract(_seconds);
                _stringBuilder.Clear();
                _timeUI.text = GetCurrentTime();

                yield return new WaitForSeconds(1f);
            }

            yield break;
        }
    }
}