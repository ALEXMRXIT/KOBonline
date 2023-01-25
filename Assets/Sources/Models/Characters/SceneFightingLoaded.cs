using UnityEngine;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models.Characters
{
    public sealed class SceneFightingLoaded : MonoBehaviour
    {
        [SerializeField] private CustomerModelView _customerModelView;

        private INetworkProcessor _networkProcessor;

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;

            _networkProcessor.GetParentObject().GetMainPlayer = _customerModelView.ModelFinalBuild(
                _networkProcessor.GetParentObject().GetPlayerContract, showModel: true).ShowModel();
            _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());
        }
    }
}