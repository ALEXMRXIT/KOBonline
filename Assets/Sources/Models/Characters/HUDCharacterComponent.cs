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
                _canvasHud.SetActive(true);

            _health.value = Mathf.Clamp01(unchecked(playerContract.MinHealth / playerContract.Health));
            _mana.value = Mathf.Clamp01(unchecked(playerContract.MinMana / playerContract.Mana));
            _characterNameText.text = playerContract.CharacterName;
        }
    }
}