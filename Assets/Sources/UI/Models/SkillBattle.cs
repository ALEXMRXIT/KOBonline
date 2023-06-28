using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Network;
using UnityEngine.EventSystems;
using Assets.Sources.Contracts;
using System.Collections.Generic;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI.Models
{
    public sealed class SkillBattle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _imageCooldownSkill;

        private SkillContract _skillContract;
        private ClientProcessor _clientProcessor;
        private List<SlotBattle> _slotBattles;
        private Coroutine _coroutine;
        private float _currentTime;
        private bool _isUse = false;
        private float _originalTime = 0f;

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (_isUse)
                return;

            await _clientProcessor.SendPacketAsync(UseSkill.ToPacket(_skillContract.Id));
        }

        public void PlayerCooldown()
        {
            _currentTime = _skillContract.Recharge;
            _originalTime = _skillContract.Recharge;
            _isUse = true;
        }

        public void CooldownWithFixedTime(float time)
        {
            _currentTime = time;
            _originalTime = time;
            _isUse = true;
        }

        private void Update()
        {
            if (!_isUse)
                return;

            _currentTime -= Time.deltaTime;
            _imageCooldownSkill.fillAmount = 1f - ((_originalTime - _currentTime) / _originalTime);

            if (_currentTime <= 0f)
                _isUse = false;
        }

        public void SetProcessor(ClientProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
            _imageCooldownSkill.fillAmount = 0f;
        }

        public void SetSkill(SkillContract skillContract)
        {
            _skillContract = skillContract;
        }

        public long GetSkillId() => _skillContract.Id;
        public bool IsSkillReloaded() => _isUse;

        public void SetRefSlots(List<SlotBattle> slotBattles)
        {
            _slotBattles = slotBattles;
        }

        public void ResetFlags()
        {
            _isUse = false;
        }
    }
}