using UnityEngine;
using System.Collections;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models.Characters
{
    public sealed class SceneFightingLoaded : MonoBehaviour
    {
        [SerializeField] private CustomerModelView _customerModelView;
        [SerializeField] private GameObject _mainCamera;

        [Space]
        [SerializeField] private Vector3 _positionCamera2;
        [SerializeField] private Vector3 _rotationCamera2;

        private INetworkProcessor _networkProcessor;

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;
            StartCoroutine(WaitPacketCharacterPosition());
        }

        private IEnumerator WaitPacketCharacterPosition()
        {
            yield return new WaitUntil(() => _networkProcessor.GetParentObject().GetPlayerPacketLoaded);
            _networkProcessor.GetParentObject().GetMainPlayer = _customerModelView.ModelFinalBuild(
                _networkProcessor.GetParentObject().GetPlayerContract, showModel: false).
                    ShowModel(updatePosition: true, blockDisableActive: true);

            if (_networkProcessor.GetParentObject().GetPlayerContract.RotationY == 0f)
            {
                _mainCamera.transform.position = _positionCamera2;
                _mainCamera.transform.rotation = Quaternion.Euler(_rotationCamera2);
            }

            yield return new WaitUntil(() => _networkProcessor.GetParentObject().GetPlayerPacketEnemyInfoLoaded);
            _networkProcessor.GetParentObject().GetEnemyPlayer = _customerModelView.ModelFinalBuild(
                _networkProcessor.GetParentObject().GetEnemyContract, showModel: false).
                    ShowModel(updatePosition: true, blockDisableActive: true);

            _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());
        }

        private void OnDestroy()
        {
            _networkProcessor.GetParentObject().GetPlayerPacketLoaded = false;
            _networkProcessor.GetParentObject().GetPlayerPacketEnemyInfoLoaded = false;
        }
    }
}