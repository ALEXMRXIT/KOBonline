using UnityEngine;
using System.Collections;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Camera;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models.Characters
{
    public sealed class SceneFightingLoaded : MonoBehaviour
    {
        [SerializeField] private CustomerModelView _customerModelView;
        [SerializeField] private TargetSurveillanceCamera _mainCamera;

        [Space]
        [SerializeField] private Vector3 _positionCamera2;
        [SerializeField] private Vector3 _rotationCamera2;

        private INetworkProcessor _networkProcessor;

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;
            StartCoroutine(SetBattleSceneForActors());
        }

        private IEnumerator SetBattleSceneForActors()
        {
            yield return new WaitUntil(() => _networkProcessor.GetParentObject().GetPlayerData.ObjectIsLoadData);
            _networkProcessor.GetParentObject().GetPlayerData.GameObjectModel = _customerModelView.ModelFinalBuild(
                _networkProcessor.GetParentObject().GetPlayerData.ObjectContract, showModel: false).
                    ShowModel(updatePosition: true, blockDisableActive: true);

            if (_networkProcessor.GetParentObject().GetPlayerData.ObjectContract.RotationY == 0f)
            {
                _mainCamera.gameObject.transform.position = _positionCamera2;
                _mainCamera.gameObject.transform.rotation = Quaternion.Euler(_rotationCamera2);
            }

            _mainCamera.SetupCameraMovedForTarget(_networkProcessor.GetParentObject().GetPlayerData.GameObjectModel.transform);

            yield return new WaitUntil(() => _networkProcessor.GetParentObject().GetEnemyData.ObjectIsLoadData);
            _networkProcessor.GetParentObject().GetEnemyData.GameObjectModel = _customerModelView.ModelFinalBuild(
                _networkProcessor.GetParentObject().GetEnemyData.ObjectContract, showModel: false).
                    ShowModel(updatePosition: true, blockDisableActive: true);

            CharacterTarget mainTarget = _networkProcessor.GetParentObject()
                .GetPlayerData.GameObjectModel.AddComponent<CharacterTarget>();
            CharacterTarget enemyTarget = _networkProcessor.GetParentObject()
                .GetEnemyData.GameObjectModel.AddComponent<CharacterTarget>();
            HUDCharacterComponent hudMain = _networkProcessor.GetParentObject()
                .GetPlayerData.GameObjectModel.GetComponent<HUDCharacterComponent>();
            HUDCharacterComponent hudEnemy = _networkProcessor.GetParentObject()
                .GetEnemyData.GameObjectModel.GetComponent<HUDCharacterComponent>();

            _networkProcessor.GetParentObject().GetPlayerData.ObjectTarget = mainTarget;
            _networkProcessor.GetParentObject().GetPlayerData.ObjectHUD = hudMain;
            _networkProcessor.GetParentObject().GetEnemyData.ObjectTarget = enemyTarget;
            _networkProcessor.GetParentObject().GetEnemyData.ObjectHUD = hudEnemy;

            if (!mainTarget.IsTargetHook())
                mainTarget.SetTarget(_networkProcessor.GetParentObject().GetEnemyData.GameObjectModel.transform);
            else throw new System.ArgumentException(nameof(mainTarget));

            if (!enemyTarget.IsTargetHook())
                enemyTarget.SetTarget(_networkProcessor.GetParentObject().GetPlayerData.GameObjectModel.transform);
            else throw new System.ArgumentException(nameof(enemyTarget));

            _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());
        }

        private void OnDestroy()
        {
            _networkProcessor.GetParentObject().GetPlayerData.ObjectIsLoadData = false;
            _networkProcessor.GetParentObject().GetPlayerData.ObjectTarget = null;
            _networkProcessor.GetParentObject().GetPlayerData.GameObjectModel = null;

            _networkProcessor.GetParentObject().GetEnemyData.ObjectIsLoadData = false;
            _networkProcessor.GetParentObject().GetEnemyData.ObjectTarget = null;
            _networkProcessor.GetParentObject().GetEnemyData.GameObjectModel = null;
        }
    }
}