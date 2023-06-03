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
        public Damage(DamageFrom damageFrom, bool isBot, int value,
            int damageIndex, bool create = false)
        {
            ClientDamageFrom = damageFrom;
            ClientDamageIsBot = isBot;
            ClientDamageValue = value;
            DamageIndex = damageIndex;
            AutoCreate = create;
        }

        public readonly DamageFrom ClientDamageFrom;
        public readonly bool ClientDamageIsBot;
        public readonly int ClientDamageValue;
        public readonly int DamageIndex;
        public readonly bool AutoCreate;
    }

    public sealed class TextView : MonoBehaviour
    {
        [SerializeField] private Transform _damagePointSpawn;
        [SerializeField] private GameObject _baseDamageYellow;
        [SerializeField] private GameObject _baseDamageRed;
        [SerializeField] private GameObject _baseDamageYellowCrit;
        [SerializeField] private GameObject _baseDamageRedCrit;

        private Dictionary<int, Damage> _contain = new Dictionary<int, Damage>();

        public void AddDamage(Damage damage)
        {
            _contain.TryAdd(damage.DamageIndex, damage);
        }

        public bool ShowDamage(ObjectData objectData, int damageIndex)
        {
            Damage strDamage;
            if (_contain.ContainsKey(damageIndex))
                _contain.Remove(damageIndex, out strDamage);
            else
                return false;

            GameObject damageView = null;
            if (strDamage.ClientDamageIsBot)
                damageView = Instantiate(_baseDamageYellow, _damagePointSpawn);
            else
                damageView = Instantiate(_baseDamageRed, _damagePointSpawn);

            damageView.GetComponent<Text>().text = strDamage.ClientDamageValue.ToString();
            objectData.ObjectContract.MinHealth = Mathf.Clamp(objectData.
                ObjectContract.MinHealth - strDamage.ClientDamageValue,
                    min: 0, max: objectData.ObjectContract.Health);

            if (objectData.IsBot)
                objectData.ClientHud.UpdateEnemyHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);
            else
                objectData.ClientHud.UpdateHealthBar(objectData.ObjectContract.MinHealth, objectData.ObjectContract.Health);

            if (objectData.ObjectContract.MinHealth <= 0)
                objectData.ClientAnimationState.SetCharacterState(new StateAnimationIdle());

            Destroy(damageView, 1.5f);
            return true;
        }
    }
}