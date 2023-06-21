using UnityEngine;

namespace Assets.Sources.Models.Characters
{
    public sealed class SoundLink : MonoBehaviour
    {
        [SerializeField] private AudioSource _takeDamage;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private AudioSource _mageShieldSoundEffect;
        [SerializeField] private AudioSource _strongBodySoundEffect;
        [SerializeField] private AudioSource _heroesPowerSoundEffects;

        private AudioSource _backgroundCacheSoundBattle;
        private AudioSource _winSound;
        private AudioSource _loseSound;

        public void SetBackgroundSound(AudioSource audioSource)
        {
            _backgroundCacheSoundBattle = audioSource;
        }

        public void StopBackgroundSound()
        {
            if (_backgroundCacheSoundBattle != null)
                _backgroundCacheSoundBattle.Stop();
        }

        public void SetRoundSound(AudioSource win, AudioSource lose)
        {
            _winSound = win;
            _loseSound = lose;
        }

        public void PlaySoundIfWinOrLosse(bool isWin)
        {
            if (isWin) _winSound.Play();
            else _loseSound.Play();
        }

        public void CallDeathSoundEffect()
        {
            _deathSound.Play();
        }

        public void CallTakeDamageSoundEffect()
        {
            _takeDamage.Play();
        }

        public void CallMageShieldSoundEffect()
        {
            _mageShieldSoundEffect.Play();
        }

        public void CallStrongBodySoundEffect()
        {
            _strongBodySoundEffect.Play();
        }

        public void CallHeroesPowerSoundEffect()
        {
            _heroesPowerSoundEffects.Play();
        }
    }
}