using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.States.StateAnimations;

namespace Assets.Sources.Models.Characters
{
    public enum DamageFrom : byte
    {
        DamageFromServer,
        DamageFromClient
    }

    public readonly struct Damage
    {
        public Damage(DamageFrom damageFrom, bool isBot, int value)
        {
            ClientDamageFrom = damageFrom;
            ClientDamageIsBot = isBot;
            ClientDamageValue = value;
        }

        public readonly DamageFrom ClientDamageFrom;
        public readonly bool ClientDamageIsBot;
        public readonly int ClientDamageValue;
    }

    public sealed class TextView : MonoBehaviour
    {
        [SerializeField] private Transform _damagePointSpawn;
        [SerializeField] private GameObject _baseDamageYellow;
        [SerializeField] private GameObject _baseDamageRed;
        [SerializeField] private GameObject _baseDamageYellowCrit;
        [SerializeField] private GameObject _baseDamageRedCrit;

        private Queue<Damage> _contain = new Queue<Damage>();

        public void AddDamage(Damage damage)
        {
            _contain.Enqueue(damage);
        }

        public void ShowDamage(ObjectData objectData)
        {
            if (!_contain.TryDequeue(out Damage strDamage))
            {
                Debug.LogWarning($"Unable to display damage, buffer container is empty!");
                return;
            }

            GameObject damageView = null;
            if (strDamage.ClientDamageIsBot)
                damageView = Instantiate(_baseDamageYellow, _damagePointSpawn);
            else
                damageView = Instantiate(_baseDamageRed, _damagePointSpawn);

            damageView.GetComponent<Text>().text = strDamage.ClientDamageValue.ToString();
            objectData.ObjectContract.MinHealth = Mathf.Clamp(objectData.
                ObjectContract.MinHealth - strDamage.ClientDamageValue, min: 0, max: objectData.ObjectContract.Health);

            if (objectData.IsBot)
                objectData.ClientHud.UpdateEnemyHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
            else
                objectData.ClientHud.UpdateHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);

            Destroy(damageView, 1.5f);
        }

        public bool PeekStack()
        {
            return _contain.TryPeek(out _);
        }
    }
}