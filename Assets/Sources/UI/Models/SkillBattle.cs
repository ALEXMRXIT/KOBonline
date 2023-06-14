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

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (_isUse)
                return;

            await _clientProcessor.SendPacketAsync(UseSkill.ToPacket(_skillContract.Id));

            foreach (SlotBattle slotBattle in _slotBattles)
            {
                if (slotBattle.GetSkillSlot() == null)
                    continue;

                if (slotBattle.GetSkillSlot().GetSkillId() == _skillContract.Id)
                    slotBattle.GetSkillSlot().PlayerCooldown();
            }
        }

        public void PlayerCooldown()
        {
            _currentTime = _skillContract.Recharge;
            _isUse = true;
        }

        private void Update()
        {
            if (!_isUse)
                return;

            _currentTime -= Time.deltaTime;
            _imageCooldownSkill.fillAmount = 1f - ((_skillContract.Recharge - _currentTime) / _skillContract.Recharge);

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

        public void SetRefSlots(List<SlotBattle> slotBattles)
        {
            _slotBattles = slotBattles;
        }
    }
}