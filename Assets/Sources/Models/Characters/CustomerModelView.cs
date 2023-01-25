using System;
using UnityEngine;
using System.Collections;
using Assets.Sources.Enums;
using Assets.Sources.Models;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models.Characters
{
    internal readonly struct ModelObject
    {
        public readonly GameObject _playerModel;
        public readonly CharacterState _characterState;

        public ModelObject(GameObject model)
        {
            if (!model.TryGetComponent<CharacterState>(out CharacterState characterState))
                throw new MissingComponentException(nameof(CharacterState));

            characterState.CheckGettingComponent();

            _playerModel = model;
            _characterState = characterState;
        }
    }

    public sealed class CustomerModelView : MonoBehaviour
    {
        [SerializeField] private Transform _characterSpawn;
        [SerializeField] private GameObject[] _characters;
        [SerializeField] private PoolObjects _poolObject;

        private List<ModelObject> _tempCharacter;
        private int _currentActiveModel = 0;

        public PlayerContract BuildPlayerContract { get; private set; }
        public static CustomerModelView Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            BuildPlayerContract = new PlayerContract();
            _tempCharacter = new List<ModelObject>(capacity: _characters.Length);

            for (int iterator = 0; iterator < _characters.Length; iterator++)
            {
                GameObject character = Instantiate(_characters[iterator], _characterSpawn);
                character.transform.SetParent(null);
                character.transform.position = _characterSpawn.position;

                _tempCharacter.Add(new ModelObject(character));
                _tempCharacter[iterator]._playerModel.SetActive(false);
            }
        }

        public CustomerModelView ModelSetSex(PlayerSex playerSex, bool showModel = false)
        {
            BuildPlayerContract.Sex = playerSex;

            if (showModel)
                ShowModel();

            return this;
        }

        public CustomerModelView ModelSetBaseClass(BaseClass baseClass, bool showModel = false)
        {
            BuildPlayerContract.CharacterBaseClass = baseClass;

            if (showModel)
                ShowModel();

            return this;
        }

        public CustomerModelView ModelFinalBuild(PlayerContract playerContract, bool showModel = false)
        {
            if (playerContract == null)
                throw new ArgumentNullException(nameof(playerContract));

            BuildPlayerContract = playerContract;

            if (showModel)
                ShowModel();

            return this;
        }

        public GameObject ShowModel(bool updatePosition = false)
        {
            GameObject model = ShowModelExecute(updatePosition);

            PoolModelObject poolModelObject = _poolObject.GetModelByIdentifier
                    <ParticleSystem>(PoolObjecPoolObjectIdentifier.EffectSpawnCharacter);
            ParticleSystem particleSystem = (ParticleSystem)poolModelObject._component;
            particleSystem.gameObject.transform.position = model.transform.position;
            particleSystem.Play();

            StartCoroutine(ParticleDurationOff(particleSystem.main.duration, poolModelObject));

            return model;
        }

        private GameObject ShowModelExecute(bool updatePosition = false)
        {
            int index = 0;

            switch (BuildPlayerContract.CharacterBaseClass)
            {
                case BaseClass.Warrior:
                    if (BuildPlayerContract.Sex == PlayerSex.Man)
                        index = 0;
                    else if (BuildPlayerContract.Sex == PlayerSex.Woman)
                        index = 2;
                    break;
                case BaseClass.Mage:
                    if (BuildPlayerContract.Sex == PlayerSex.Man)
                        index = 1;
                    else if (BuildPlayerContract.Sex == PlayerSex.Woman)
                        index = 3;
                    break;
            }

            GameObject model = _tempCharacter[index]._playerModel;

            _tempCharacter[_currentActiveModel]._playerModel.SetActive(false);
            model.SetActive(true);

            if (updatePosition)
            {
                model.transform.position = new Vector3(BuildPlayerContract.PositionX,
                    BuildPlayerContract.PositionY, BuildPlayerContract.PositionZ);
                model.transform.rotation = Quaternion.Euler(new Vector3(BuildPlayerContract.RotationX,
                    BuildPlayerContract.RotationY, BuildPlayerContract.RotationZ));
            }

            _tempCharacter[index]._characterState.SetCharacterState(new StateAnimationIdle());
            _currentActiveModel = index;

            return model;
        }

        private IEnumerator ParticleDurationOff(float duration, PoolModelObject poolModelObject)
        {
            yield return new WaitForSecondsRealtime(duration);
            ((ParticleSystem)poolModelObject._component).Stop();
            poolModelObject._object.SetActive(false);
        }
    }
}