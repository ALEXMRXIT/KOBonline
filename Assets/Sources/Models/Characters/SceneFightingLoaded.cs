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
using Assets.Sources.Models.Characters.Skills;

namespace Assets.Sources.Models.Characters
{
    [RequireComponent(typeof(TimeRound))]
    public sealed class SceneFightingLoaded : MonoBehaviour
    {
        [SerializeField] private CustomerModelView _customerModelView;
        [SerializeField] private TargetSurveillanceCamera _mainCamera;
        [SerializeField] private GameObject _panelWinner;
        [SerializeField] private VisualAbility _visualAbility;
        [SerializeField] private Transform _canvasPlayer;
        [SerializeField] private Transform _canvasEnemy;
        [SerializeField] private Button _buttonAttacke;

        [Space]
        [SerializeField] private Vector3 _positionCamera2;
        [SerializeField] private Vector3 _rotationCamera2;

        [Space]
        [SerializeField] private GameObject _skillObject;
        [SerializeField] private Transform _spawnContentSkills;
        [SerializeField] private List<GameObject> _slots = new List<GameObject>();

        [Space]
        [SerializeField] private AudioSource _backgroundMusic;
        [SerializeField] private AudioSource _winSound;
        [SerializeField] private AudioSource _loseSound;

        private INetworkProcessor _networkProcessor;
        private List<SlotBattle> _slotBattles;
        private TimeRound _timeRound;

        private void Awake()
        {
            _timeRound = GetComponent<TimeRound>();
            _slotBattles = new List<SlotBattle>(capacity: _slots.Count);
            for (int iterator = 0; iterator < _slots.Count; iterator++)
            {
                if (!_slots[iterator].TryGetComponent(out SlotBattle slotBattle))
                    throw new MissingComponentException(nameof(SlotBattle));

                _slotBattles.Add(slotBattle);
            }
        }

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;
            _networkProcessor.GetParentObject().GetTimeRound = _timeRound;
            StartCoroutine(SetBattleSceneForActors());
            _panelWinner.SetActive(false);
            _buttonAttacke.onClick.AddListener(InternalOnButtonCkickAttackingHandler);
        }

        private void InternalOnButtonCkickAttackingHandler()
        {
            _networkProcessor.SendPacketAsync(RequestAttacking.ToPacket());
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

                firstEnemy.ClientTextView.SetTargetTransformForSpawnDamage(secondEnemy.ClientTextView.GetTargetTransformParent());
                firstEnemy.ClientTextView.SetMeTransformForSpawnDamage(firstEnemy.ClientTextView.GetTargetTransformParent());
                secondEnemy.ClientTextView.SetTargetTransformForSpawnDamage(firstEnemy.ClientTextView.GetTargetTransformParent());
                secondEnemy.ClientTextView.SetMeTransformForSpawnDamage(secondEnemy.ClientTextView.GetTargetTransformParent());

                firstEnemy.ObjectTarget = firstEnemyTarget;
                secondEnemy.ObjectTarget = secondEnemyTarget;

                firstEnemy.SoundCharacterLink = firstEnemy.GameObjectModel.GetComponent<SoundLink>();
                firstEnemy.SoundCharacterLink.SetBackgroundSound(_backgroundMusic);
                firstEnemy.SoundCharacterLink.SetRoundSound(_winSound, _loseSound);
                secondEnemy.SoundCharacterLink = secondEnemy.GameObjectModel.GetComponent<SoundLink>();
                secondEnemy.SoundCharacterLink.SetBackgroundSound(_backgroundMusic);
                secondEnemy.SoundCharacterLink.SetRoundSound(_winSound, _loseSound);

                AbilityEffectLink abilityEffectLinkFirstEnemy = firstEnemy.GameObjectModel.GetComponent<AbilityEffectLink>();
                AbilityEffectLink abilityEffectLinkSecondEnemy = secondEnemy.GameObjectModel.GetComponent<AbilityEffectLink>();
                firstEnemy.ClientAbilityEffectLink = abilityEffectLinkFirstEnemy;
                secondEnemy.ClientAbilityEffectLink = abilityEffectLinkSecondEnemy;

                if (!firstEnemy.GameObjectModel.TryGetComponent(out VisualModelOfAbilityExecution visualModelOfAbilityExecution))
                    visualModelOfAbilityExecution = firstEnemy.GameObjectModel.AddComponent<VisualModelOfAbilityExecution>();
                firstEnemy.ClientVisualModelOfAbilityExecution = visualModelOfAbilityExecution;
                firstEnemy.ClientVisualModelOfAbilityExecution.Init(_networkProcessor.GetParentObject());
                firstEnemy.ClientVisualModelOfAbilityExecution.SetAblilityEffect(firstEnemy.ClientAbilityEffectLink);
                firstEnemy.ClientVisualModelOfAbilityExecution.SetPlayerTransformWithCanvas(_canvasPlayer);
                firstEnemy.ClientVisualModelOfAbilityExecution.SetAbilityPrefab(_visualAbility);
                firstEnemy.ClientVisualModelOfAbilityExecution.StartHandler();

                if (!secondEnemy.GameObjectModel.TryGetComponent(out VisualModelOfAbilityExecution visualModelOfAbilityExecution1))
                    visualModelOfAbilityExecution1 = secondEnemy.GameObjectModel.AddComponent<VisualModelOfAbilityExecution>();
                secondEnemy.ClientVisualModelOfAbilityExecution = visualModelOfAbilityExecution1;
                secondEnemy.ClientVisualModelOfAbilityExecution.Init(_networkProcessor.GetParentObject());
                secondEnemy.ClientVisualModelOfAbilityExecution.SetAblilityEffect(secondEnemy.ClientAbilityEffectLink);
                secondEnemy.ClientVisualModelOfAbilityExecution.SetPlayerTransformWithCanvas(_canvasEnemy);
                secondEnemy.ClientVisualModelOfAbilityExecution.SetAbilityPrefab(_visualAbility);
                secondEnemy.ClientVisualModelOfAbilityExecution.StartHandler();

                firstEnemy.GameObjectModel.GetComponent<BaseAttackSpawnEffect>().
                    Init(firstEnemy.ObjectContract.CharacterBaseClass, firstEnemy, secondEnemy);
                secondEnemy.GameObjectModel.GetComponent<BaseAttackSpawnEffect>().
                    Init(secondEnemy.ObjectContract.CharacterBaseClass, secondEnemy, firstEnemy);

                if (!firstEnemyTarget.IsTargetHook())
                    firstEnemyTarget.SetTarget(secondEnemy.GameObjectModel.transform, secondEnemy);
                if (!secondEnemyTarget.IsTargetHook())
                    secondEnemyTarget.SetTarget(firstEnemy.GameObjectModel.transform, firstEnemy);
            }

            foreach (SkillData skillData in _networkProcessor.GetParentObject().GetSkillDatas)
            {
                if (skillData.SlotId == -1)
                    continue;

                Skill skill = _networkProcessor.GetParentObject()
                    .GetSkills.Where(x => x.Id == skillData.SkillId).FirstOrDefault();

                SkillContract skillContract = _networkProcessor.GetParentObject()
                    .GetSkillContracts.Where(x => x.Id == skillData.SkillId).FirstOrDefault();

                if (skill == null)
                    throw new NullReferenceException(nameof(Skill));

                GameObject item = Instantiate(_skillObject, _spawnContentSkills);

                item.transform.SetParent(_slots[skillData.SlotId - 1].transform);
                RectTransform rectTransform = item.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(0f, 0f, rectTransform.localPosition.z);

                if (!item.TryGetComponent(out Image image))
                    throw new MissingComponentException(nameof(Image));

                image.sprite = skill.SkillSprite;

                if (!item.TryGetComponent(out SkillBattle skillBattle))
                    throw new MissingComponentException(nameof(SkillBattle));

                skillBattle.SetProcessor(_networkProcessor.GetParentObject());
                skillBattle.SetSkill(skillContract);
                skillBattle.SetRefSlots(_slotBattles);
                skillBattle.ResetFlags();

                _slotBattles[skillData.SlotId - 1].SetSkill(skillBattle);
            }

            _networkProcessor.GetParentObject().SetAbilityWithBattleMode(_slotBattles);
            _networkProcessor.SendPacketAsync(LoadSceneFightingSuccess.ToPacket());
        }

        private void OnDestroy()
        {
            _networkProcessor.GetParentObject().GetTimeRound = null;
            _networkProcessor.GetParentObject().GetPlayers.RemoveAll(x => x.IsBot);
            _networkProcessor.GetParentObject().SetAbilityWithBattleMode(null);
            _networkProcessor.GetParentObject().GetNetworkDataLoader.Reset();
        }
    }
}