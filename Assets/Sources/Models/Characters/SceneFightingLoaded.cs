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
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                if (_networkProcessor.GetParentObject().GetPlayerPacketLoaded)
                {
                    _networkProcessor.GetParentObject().GetMainPlayer = _customerModelView.ModelFinalBuild(
                        _networkProcessor.GetParentObject().GetPlayerContract,
                        showModel: true).ShowModel(updatePosition: true);
                    _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());

                    if (_networkProcessor.GetParentObject().GetPlayerContract.RotationY == 0f)
                    {
                        _mainCamera.transform.position = _positionCamera2;
                        _mainCamera.transform.rotation = Quaternion.Euler(_rotationCamera2);
                    }

                    break;
                }
            }
        }
    }
}