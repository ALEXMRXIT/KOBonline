using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Assets.Sources.Models
{
    public sealed class LoaderSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _musics;
        [SerializeField] private AudioMixerGroup _sounds;
        [SerializeField] private AudioMixerGroup _death;
        [SerializeField] private Slider _sliderMusics;
        [SerializeField] private Slider _sliderSounds;
        [SerializeField] private Text _sliderTextViewMusics;
        [SerializeField] private Text _sliderTextViewSounds;

        private void Awake()
        {
            _sliderMusics.onValueChanged.AddListener(InternalOnvalueChangedSliderMusicsHandler);
            _sliderSounds.onValueChanged.AddListener(InternalOnvalueChangedSliderSoundsHandler);

            if (PlayerPrefs.HasKey("musicsKeyValue"))
            {
                StaticFields._musicBackground = PlayerPrefs.GetFloat("musicsKeyValue");
                _musics.audioMixer.SetFloat("MasterMusics", Mathf.Lerp(-80.0f, 0.0f, StaticFields._musicBackground));
                _sliderMusics.value = StaticFields._musicBackground;

                _sliderTextViewMusics.text = $"{StaticFields._musicBackground * 100:#}%";

                if (StaticFields._musicBackground <= 0)
                    _sliderTextViewMusics.text = "0%";
            }

            if (PlayerPrefs.HasKey("soundsKeyValue"))
            {
                StaticFields._otherMusicEffects = PlayerPrefs.GetFloat("soundsKeyValue");
                _sounds.audioMixer.SetFloat("MasterMusics", Mathf.Lerp(-80.0f, 0.0f, StaticFields._otherMusicEffects));
                _death.audioMixer.SetFloat("MasterSounds", Mathf.Lerp(-80.0f, 0.0f, StaticFields._otherMusicEffects));
                _sliderSounds.value = StaticFields._otherMusicEffects;

                _sliderTextViewSounds.text = $"{StaticFields._otherMusicEffects * 100:#}%";

                if (StaticFields._otherMusicEffects <= 0)
                    _sliderTextViewSounds.text = "0%";
            }
        }

        private void InternalOnvalueChangedSliderMusicsHandler(float value)
        {
            _musics.audioMixer.SetFloat("MasterMusics", Mathf.Lerp(-80.0f, 0.0f, value));
            _sliderTextViewMusics.text = $"{value * 100:#}%";

            if (value <= 0)
                _sliderTextViewMusics.text = "0%";
            PlayerPrefs.SetFloat("musicsKeyValue", value);
        }

        private void InternalOnvalueChangedSliderSoundsHandler(float value)
        {
            _sounds.audioMixer.SetFloat("MasterSounds", Mathf.Lerp(-80.0f, 0.0f, value));
            _death.audioMixer.SetFloat("MasterSounds", Mathf.Lerp(-80.0f, 0.0f, value));
            _sliderTextViewSounds.text = $"{value * 100:#}%";

            if (value <= 0)
                _sliderTextViewSounds.text = "0%";
            PlayerPrefs.SetFloat("soundsKeyValue", value);
        }
    }
}