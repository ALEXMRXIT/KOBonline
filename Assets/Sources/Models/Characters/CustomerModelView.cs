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
        [SerializeField] private GameObject _effectSpawnObjectWithPlayer;

        private List<ModelObject> _tempCharacter;

        public PlayerContract BuildPlayerContract { get; private set; }
        public static CustomerModelView Instance;

        private void Awake()
        {
            Instance = this;

            BuildPlayerContract = new PlayerContract();
            _tempCharacter = new List<ModelObject>(capacity: _characters.Length);

            for (int iterator = 0; iterator < _characters.Length; iterator++)
            {
                GameObject character = Instantiate(_characters[iterator]);
                character.transform.position = _characterSpawn.position;

                _tempCharacter.Add(new ModelObject(character));
                _tempCharacter[iterator]._playerModel.SetActive(false);
            }
        }

        public GameObject ModelSetSex(PlayerSex playerSex, bool showModel = false)
        {
            BuildPlayerContract.Sex = playerSex;

            GameObject model = null;
            if (showModel)
                model = ShowModel();

            return model;
        }

        public GameObject ModelSetBaseClass(BaseClass baseClass, bool showModel = false)
        {
            BuildPlayerContract.CharacterBaseClass = baseClass;

            GameObject model = null;
            if (showModel)
                model = ShowModel();

            return model;
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

        public GameObject ShowModel(bool updatePosition = false, bool blockDisableActive = false)
        {
            GameObject model = ShowModelExecute(updatePosition, blockDisableActive);

            GameObject effectSpawnModel = Instantiate(_effectSpawnObjectWithPlayer);
            effectSpawnModel.transform.position = model.transform.position;

            if (!effectSpawnModel.TryGetComponent(out ParticleSystem particleSystem))
                throw new MissingComponentException(nameof(ParticleSystem));

            particleSystem.Play();

            StartCoroutine(InternalParticleDurationOff(particleSystem.main.duration,
                particleSystem, (ParticleSystem effect) =>
                {
                    effect.Stop();
                    Destroy(effect.gameObject);
                }));

            return model;
        }

        private GameObject ShowModelExecute(bool updatePosition = false, bool blockDisableActive = false)
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

            bool activeSelf = false;
            if (model != null || model.activeSelf)
            {
                model = Instantiate(_characters[index]);
                model.transform.position = Vector3.zero;
                activeSelf = true;
            }
            else
                model.SetActive(true);

            if (updatePosition)
            {
                model.transform.position = new Vector3(BuildPlayerContract.PositionX,
                    BuildPlayerContract.PositionY, BuildPlayerContract.PositionZ);
                model.transform.rotation = Quaternion.Euler(new Vector3(BuildPlayerContract.RotationX,
                    BuildPlayerContract.RotationY, BuildPlayerContract.RotationZ));
            }

            if (!activeSelf)
                _tempCharacter[index]._characterState.SetCharacterState(new StateAnimationIdle());
            else
            {
                if (!model.TryGetComponent(out CharacterState characterState))
                    throw new MissingComponentException(nameof(CharacterState));

                characterState.CheckGettingComponent();
                characterState.SetCharacterState(new StateAnimationIdle());
            }    

            return model;
        }

        private IEnumerator InternalParticleDurationOff(float duration,
            ParticleSystem effect,Action<ParticleSystem> action)
        {
            yield return new WaitForSecondsRealtime(duration);
            action?.Invoke(effect);
        }
    }
}