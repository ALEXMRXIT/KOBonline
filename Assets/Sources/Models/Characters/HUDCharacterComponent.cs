using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;

namespace Assets.Sources.Models.Characters
{
    public sealed class HUDCharacterComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _canvasHud;
        [SerializeField] private Slider _health;
        [SerializeField] private Slider _mana;
        [SerializeField] private Text _characterNameText;

        public void SetHUD(PlayerContract playerContract)
        {
            if (!_canvasHud.activeSelf)
                SetHudActiveStatus(true);

            UpdateHealthData(playerContract);
            UpdateManaData(playerContract);
            _characterNameText.text = playerContract.CharacterName;
        }

        public void UpdateHealthData(PlayerContract playerContract)
        {
            _health.value = Mathf.Clamp(playerContract.MinHealth / (float)playerContract.Health, min: 0, max: 1);
        }

        public void UpdateManaData(PlayerContract playerContract)
        {
            _health.value = Mathf.Clamp(playerContract.MinMana / (float)playerContract.Mana, min: 0, max: 1);
        }

        public void SetHudActiveStatus(bool status)
        {
            _canvasHud.SetActive(status);
        }
    }
}