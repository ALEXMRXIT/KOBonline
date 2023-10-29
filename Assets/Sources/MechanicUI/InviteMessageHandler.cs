using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class InviteMessageHandler : MonoBehaviour
    {
        private delegate void OnCallMethod(bool status);

        [SerializeField] private GameObject _panelAnswerEnergy;
        [SerializeField] private Text _panelAnswerEnergyDescription;
        [SerializeField] private Text _panelAnswerEnergyError;
        [SerializeField] private Text _panelAnswerEnergyButtonText;
        [SerializeField] private Button _panelAnswerEnergyOk;
        [SerializeField] private Button _panelAnswerEnergyCancel;

        private OnCallMethod _onCall;
        private INetworkProcessor _networkProcessor;
        private string _characterName;

        public IEnumerator Initialization(INetworkProcessor networkProcessor)
        {
            _panelAnswerEnergyOk.onClick.AddListener(InternalOnClickHandlerOk);
            _panelAnswerEnergyCancel.onClick.AddListener(InternalOnClickHandlerCancel);
            _networkProcessor = networkProcessor;
            HidenInviteMessage();

            yield break;
        }

        public void HidenInviteMessage()
        {
            _panelAnswerEnergy.SetActive(false);
        }

        public void InviteShowInformationForEnergy(byte inviteCode, int energyCost, string characterName)
        {
            _onCall = OnCallInvoke;
            _characterName = characterName;

            switch (inviteCode)
            {
                case 0x00:
                    _panelAnswerEnergy.SetActive(true);
                    _panelAnswerEnergyDescription.text = $"Do you really want to start a duel with player: <color=green>{characterName}</color>. It will cost <color=green>{energyCost}</color> energy!";
                    break;
                case 0x01:
                    _panelAnswerEnergy.SetActive(true);
                    _panelAnswerEnergyDescription.text = $"Player <color=green>{characterName}</color> invites you to a duel, it will cost <color=green>{energyCost}</color> energy.";
                    break;
            }
        }

        private void OnCallInvoke(bool status)
        {
            if (status)
                _networkProcessor.SendPacketAsync(InviteOnDuelService.ToPacket(0x01, _characterName));
            else
                _networkProcessor.SendPacketAsync(InviteOnDuelService.ToPacket(0x02, string.Empty));
        }

        private void InternalOnClickHandlerOk()
        {
            _onCall?.Invoke(true);
            _panelAnswerEnergy.SetActive(false);
        }

        private void InternalOnClickHandlerCancel()
        {
            _onCall?.Invoke(false);
            _panelAnswerEnergy.SetActive(false);
        }
    }
}