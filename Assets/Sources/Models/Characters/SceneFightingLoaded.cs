using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.UI;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Models.Base;
using System.Collections.Generic;
using Assets.Sources.UI.Utilites;
using Assets.Sources.Models.Camera;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.Models.Characters
{
    public sealed class SceneFightingLoaded : MonoBehaviour
    {
        [SerializeField] private CustomerModelView _customerModelView;
        [SerializeField] private TargetSurveillanceCamera _mainCamera;
        [SerializeField] private GameObject _panelWinner;

        [Space]
        [SerializeField] private Vector3 _positionCamera2;
        [SerializeField] private Vector3 _rotationCamera2;

        [Space]
        [SerializeField] private GameObject _skillObject;
        [SerializeField] private Transform _spawnContentSkills;
        [SerializeField] private List<GameObject> _slots = new List<GameObject>();

        private INetworkProcessor _networkProcessor;

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;
            StartCoroutine(SetBattleSceneForActors());
            _panelWinner.SetActive(false);
        }

        private IEnumerator SetBattleSceneForActors()
        {
            foreach (ObjectData objectData in _networkProcessor.GetParentObject().GetPlayers)
            {
                if (!objectData.IsBot)
                {
                    yield return new WaitUntil(() => objectData.ObjectIsLoadData);

                    objectData.GameObjectModel = _customerModelView.ModelFinalBuild(
                        objectData.ObjectContract, showModel: false).ShowModel(updatePosition: true, blockDisableActive: true);

                    if (objectData.ObjectContract.RotationY == 0f)
                    {
                        _mainCamera.gameObject.transform.position = _positionCamera2;
                        _mainCamera.gameObject.transform.rotation = Quaternion.Euler(_rotationCamera2);
                    }

                    _mainCamera.SetupCameraMovedForTarget(objectData.GameObjectModel.transform);
                    break;
                }
            }

            yield return new WaitUntil(() => _networkProcessor.GetParentObject().GetNetworkDataLoader.IsDataLoader());

            int countPlayersInGameScene = _networkProcessor.GetParentObject().GetPlayers.Count;
            for (int iterator = 0; iterator < countPlayersInGameScene - 1; iterator += 2)
            {
                ObjectData firstEnemy = _networkProcessor.GetParentObject().GetPlayers[iterator];
                ObjectData secondEnemy = _networkProcessor.GetParentObject().GetPlayers[iterator + 1];

                if (firstEnemy.IsBot)
                {
                    firstEnemy.GameObjectModel = _customerModelView.ModelFinalBuild(
                        firstEnemy.ObjectContract, showModel: false).ShowModel(updatePosition: true, blockDisableActive: true);
                }

                if (secondEnemy.IsBot)
                {
                    secondEnemy.GameObjectModel = _customerModelView.ModelFinalBuild(
                        secondEnemy.ObjectContract, showModel: false).ShowModel(updatePosition: true, blockDisableActive: true);
                }

                CharacterTarget firstEnemyTarget = firstEnemy.GameObjectModel.AddComponent<CharacterTarget>();
                CharacterTarget secondEnemyTarget = secondEnemy.GameObjectModel.AddComponent<CharacterTarget>();
                firstEnemy.ClientAnimationState = firstEnemy.GameObjectModel.GetComponent<CharacterState>();
                secondEnemy.ClientAnimationState = secondEnemy.GameObjectModel.GetComponent<CharacterState>();
                firstEnemy.ClientHud = HudCharacter.Instance;
                secondEnemy.ClientHud = HudCharacter.Instance;

                firstEnemy.ClientTextView = firstEnemy.GameObjectModel.GetComponent<TextView>();
                secondEnemy.ClientTextView = secondEnemy.GameObjectModel.GetComponent<TextView>();

                firstEnemy.ObjectTarget = firstEnemyTarget;
                secondEnemy.ObjectTarget = secondEnemyTarget;

                firstEnemy.ObjectBaseEffectWhereAttack = firstEnemy.GameObjectModel.
                    GetComponent<BaseAttackEffect>().Init(firstEnemy.ObjectContract.CharacterBaseClass);
                firstEnemy.GameObjectModel.GetComponent<BaseAttackSpawnEffect>().
                    Init(firstEnemy.ObjectContract.CharacterBaseClass, secondEnemy);

                secondEnemy.ObjectBaseEffectWhereAttack = secondEnemy.GameObjectModel.
                    GetComponent<BaseAttackEffect>().Init(secondEnemy.ObjectContract.CharacterBaseClass);
                secondEnemy.GameObjectModel.GetComponent<BaseAttackSpawnEffect>().
                    Init(secondEnemy.ObjectContract.CharacterBaseClass, firstEnemy);

                if (!firstEnemyTarget.IsTargetHook())
                    firstEnemyTarget.SetTarget(secondEnemy.GameObjectModel.transform, secondEnemy);
                if (!secondEnemyTarget.IsTargetHook())
                    secondEnemyTarget.SetTarget(firstEnemy.GameObjectModel.transform, firstEnemy);

                foreach (SkillData skillData in _networkProcessor.GetParentObject().GetSkillDatas)
                {
                    if (skillData.SlotId == -1)
                        continue;

                    Skill skill = _networkProcessor.GetParentObject()
                        .GetSkills.Where(x => x.Id == skillData.SkillId).FirstOrDefault();

                    if (skill == null)
                        throw new NullReferenceException(nameof(Skill));

                    GameObject item = Instantiate(_skillObject, _spawnContentSkills);

                    item.transform.SetParent(_slots[skillData.SlotId - 1].transform);
                    RectTransform rectTransform = item.GetComponent<RectTransform>();
                    rectTransform.localPosition = new Vector3(0f, 0f, rectTransform.localPosition.z);

                    if (!item.TryGetComponent(out Image image))
                        throw new MissingComponentException(nameof(Image));

                    image.sprite = skill.SkillSprite;
                }
            }
            
            _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());
        }

        private void OnDestroy()
        {
            _networkProcessor.GetParentObject().GetPlayers.RemoveAll(x => x.IsBot);
            _networkProcessor.GetParentObject().GetNetworkDataLoader.Reset();
        }
    }
}