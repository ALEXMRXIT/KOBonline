using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class HudCharacter : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _manaBar;
        [SerializeField] private Text _characterName;
        [SerializeField] private Image _iconMainImage;
        [SerializeField] private Sprite _wariorrIcon;
        [SerializeField] private Sprite _mageOcon;
        [SerializeField] private Image _playerImageRank;

        [Space]
        [SerializeField] private GameObject _panelEnemyBar;
        [SerializeField] private Image _healthBarEnemy;
        [SerializeField] private Image _manaBarEnemy;
        [SerializeField] private Text _characterNameEnemy;
        [SerializeField] private Image _iconMainImageEnemy;
        [SerializeField] private Image _enemyImageRank;

        [Space]
        [SerializeField] private Sprite[] _ranksSprite;

        public static HudCharacter Instance;
        private INetworkProcessor _networkProcessor;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;
        }

        public void SetHud(PlayerContract playerContract)
        {
            if (playerContract.Sex == PlayerSex.Man)
                _iconMainImage.sprite = _wariorrIcon;
            else
                _iconMainImage.sprite = _mageOcon;

            _characterName.text = playerContract.CharacterName;
            UpdateHealthBar(playerContract.MinHealth, playerContract.Health);
            UpdateManaBar(playerContract.MinMana, playerContract.Mana);

            _playerImageRank.sprite = _ranksSprite[_networkProcessor.GetParentObject().GetRank.GetIndexByRankTable(playerContract.PlayerRank)];
        }

        public void ActivateHudEnemy(PlayerContract playerContract)
        {
            if (playerContract.Sex == PlayerSex.Man)
                _iconMainImageEnemy.sprite = _wariorrIcon;
            else
                _iconMainImageEnemy.sprite = _mageOcon;

            _characterNameEnemy.text = playerContract.CharacterName;
            _panelEnemyBar.SetActive(true);
            UpdateEnemyHealthBar(playerContract.MinHealth, playerContract.Health);
            UpdateEnemyManaBar(playerContract.MinMana, playerContract.Mana);

            _enemyImageRank.sprite = _ranksSprite[_networkProcessor.GetParentObject().GetRank.GetIndexByRankTable(playerContract.PlayerRank)];
        }

        public void UpdateHealthBar(float health, float maxHealth)
        {
            _healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        }

        public void UpdateManaBar(float mana, float maxMana)
        {
            _manaBar.fillAmount = Mathf.Clamp01(mana / maxMana);
        }

        public void UpdateEnemyHealthBar(float health, float maxHealth)
        {
            _healthBarEnemy.fillAmount = Mathf.Clamp01(health / maxHealth);
        }

        public void UpdateEnemyManaBar(float mana, float maxMana)
        {
            _manaBarEnemy.fillAmount = Mathf.Clamp01(mana / maxMana);
        }
    }
}