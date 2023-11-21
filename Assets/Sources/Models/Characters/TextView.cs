using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Network;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Camera;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models.Characters
{
    public readonly struct Damage
    {
        public Damage(bool isBot, int value, bool isCriticalDamage, bool damageMiss)
        {
            ClientDamageIsBot = isBot;
            ClientDamageValue = value;
            IsCriticalDamage = isCriticalDamage;
            DamageMiss = damageMiss;
        }

        public readonly bool ClientDamageIsBot;
        public readonly int ClientDamageValue;
        public readonly bool IsCriticalDamage;
        public readonly bool DamageMiss;
    }

    public sealed class TextView : MonoBehaviour
    {
        [SerializeField] private Transform _damagePointSpawn;
        [SerializeField] private GameObject _baseDamageYellow;
        [SerializeField] private GameObject _baseDamageRed;
        [SerializeField] private GameObject _baseDamageYellowCrit;
        [SerializeField] private GameObject _baseDamageRedCrit;
        [SerializeField] private GameObject _baseHealth;
        [SerializeField] private GameObject _baseDamageMiss;
        [Space, SerializeField] private GameObject _effectHeal;

        private List<Damage> _contain = new List<Damage>();
        private Transform _spawnTargetDamage;
        private Transform _spawnMeDamage;
        private ObjectData _playre;
        private ObjectData _enemy;

        public void SetTargetTransformForSpawnDamage(Transform target) => _spawnTargetDamage = target;
        public void SetMeTransformForSpawnDamage(Transform target) => _spawnMeDamage = target;
        

        public Transform GetTargetTransformParent()
        {
            return _damagePointSpawn;
        }

        public void AddDamage(Damage damage)
        {
            _contain.Add(damage);
        }

        public bool ShowDamage(ObjectData player, ObjectData objectData)
        {
            bool found = false;

            lock(_contain)
            {
                for (int iterator = 0; iterator < _contain.Count; iterator++)
                {
                    GameObject damageView;
                    if (objectData.IsBot)
                    {
                        if (!_contain[iterator].DamageMiss)
                        {
                            if (!_contain[iterator].IsCriticalDamage)
                                damageView = Instantiate(_baseDamageYellow, _spawnTargetDamage);
                            else
                            {
                                damageView = Instantiate(_baseDamageYellowCrit, _spawnTargetDamage);
                                TargetSurveillanceCamera.Instance.Shake(0.1f, 0.02f);
                            }
                        }
                        else
                            damageView = Instantiate(_baseDamageMiss, _spawnTargetDamage);
                    }
                    else
                    {
                        if (!_contain[iterator].DamageMiss)
                        {
                            if (!_contain[iterator].IsCriticalDamage)
                                damageView = Instantiate(_baseDamageRed, _spawnTargetDamage);
                            else
                                damageView = Instantiate(_baseDamageRedCrit, _spawnTargetDamage);
                        }
                        else
                            damageView = Instantiate(_baseDamageMiss, _spawnTargetDamage);
                    }

                    objectData.SoundCharacterLink.CallTakeDamageSoundEffect();

                    if (!_contain[iterator].DamageMiss)
                    {
                        if (!_contain[iterator].IsCriticalDamage)
                            damageView.GetComponent<Text>().text = _contain[iterator].ClientDamageValue.ToString();
                        else
                            damageView.GetComponent<Text>().text = $"{_contain[iterator].ClientDamageValue}!!";

                        objectData.ObjectContract.MinHealth = Mathf.Clamp(objectData.
                            ObjectContract.MinHealth - _contain[iterator].ClientDamageValue,
                                min: 0, max: objectData.ObjectContract.Health);

                        if (objectData.IsBot)
                            objectData.ClientHud.UpdateEnemyHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
                        else
                            objectData.ClientHud.UpdateHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
                    }
                    else
                        damageView.GetComponent<Text>().text = $"Miss";

                    Destroy(damageView, 1.5f);
                    found = true;
                }

                _contain.Clear();
            }

            return found;
        }

        public void HealTarget(ObjectData objectData, int health)
        {
            GameObject damageView = Instantiate(_baseHealth, _spawnMeDamage);
            Instantiate(_effectHeal, transform);

            damageView.GetComponent<Text>().text = health.ToString();
            objectData.ObjectContract.MinHealth = Mathf.Clamp(objectData.
                ObjectContract.MinHealth + health, min: 0, max: objectData.ObjectContract.Health);

            if (objectData.IsBot)
                objectData.ClientHud.UpdateEnemyHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
            else
                objectData.ClientHud.UpdateHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);

            Destroy(damageView, 1.5f);
        }

        public void ReduceNumberOfHealth(ObjectData objectData, int health)
        {
            GameObject damageView = Instantiate(_baseDamageYellow, _spawnMeDamage);

            damageView.GetComponent<Text>().text = health.ToString();
            objectData.ObjectContract.MinHealth = Mathf.Clamp(objectData.
                ObjectContract.MinHealth - health, min: 0, max: objectData.ObjectContract.Health);

            if (objectData.IsBot)
                objectData.ClientHud.UpdateEnemyHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
            else
                objectData.ClientHud.UpdateHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);

            Destroy(damageView, 1.5f);
        }
    }
}