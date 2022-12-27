using System;
using UnityEngine;

namespace Assets.Sources.Models
{
    public sealed class Target : MonoBehaviour
    {
        public event Action<Character> OnCharacterHooked;

        public void SetTarget(Character character)
        {
            OnCharacterHooked?.Invoke(character);
        }
    }
}