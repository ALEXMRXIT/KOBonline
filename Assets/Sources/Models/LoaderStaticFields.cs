using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Assets.Sources.Models
{
    public sealed class LoaderStaticFields : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _button;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private InputField _inputField;
        [SerializeField] private AudioMixerGroup _musics;
        [SerializeField] private AudioMixerGroup _sounds;
        [SerializeField] private AudioMixerGroup _death;

        private void Awake()
        {
            if (PlayerPrefs.HasKey("musicsKeyValue"))
            {
                StaticFields._musicBackground = PlayerPrefs.GetFloat("musicsKeyValue");
                _musics.audioMixer.SetFloat("MasterMusics", Mathf.Lerp(-80.0f, 0.0f, StaticFields._musicBackground));
            }

            if (PlayerPrefs.HasKey("soundsKeyValue"))
            {
                StaticFields._otherMusicEffects = PlayerPrefs.GetFloat("soundsKeyValue");
                _sounds.audioMixer.SetFloat("MasterMusics", Mathf.Lerp(-80.0f, 0.0f, StaticFields._otherMusicEffects));
                _death.audioMixer.SetFloat("MasterSounds", Mathf.Lerp(-80.0f, 0.0f, StaticFields._otherMusicEffects));
            }

            _button.onClick.AddListener(InternalButtonClickHandler);
            _toggle.onValueChanged.AddListener(InternalToggleClickHandler);

            if (PlayerPrefs.HasKey("TermsOfUse"))
                StaticFields.TermsOfUseCheck = PlayerPrefs.GetInt("TermsOfUse") != 0;

            if (PlayerPrefs.HasKey("LoginRemember"))
                StaticFields.LoginRemember = PlayerPrefs.GetString("LoginRemember");

            _inputField.text = StaticFields.LoginRemember ?? string.Empty;

            if (!string.IsNullOrEmpty(StaticFields.LoginRemember))
                _toggle.isOn = true;

            if (StaticFields.TermsOfUseCheck)
                _panel.SetActive(false);
        }

        private void InternalButtonClickHandler()
        {
            _panel.SetActive(false);
            PlayerPrefs.SetInt("TermsOfUse", 1);
        }

        private void InternalToggleClickHandler(bool isOn)
        {
            if (isOn && _inputField.text.Length == 0)
                return;

            PlayerPrefs.SetString("LoginRemember", _inputField.text);
        }
    }
}