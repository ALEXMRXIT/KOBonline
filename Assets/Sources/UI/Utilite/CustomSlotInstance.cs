using UnityEngine;

namespace Assets.Sources.UI.Utilites
{
    public sealed class CustomSlotInstance : MonoBehaviour
    {
        [SerializeField] private Slot[] _slots;

        private Slot _lastSelectableSlot;

        public static CustomSlotInstance Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CloseSelecatbleSlot(Slot newSlot)
        {
            if (newSlot == _lastSelectableSlot)
                return;

            if (_lastSelectableSlot != null)
                _lastSelectableSlot.CloseButton();

            _lastSelectableSlot = newSlot;
        }

        public void SetObjectBySlotId(int slotId, GameObject item, Skill skill)
        {
            _slots[slotId].EnterToSlotObject(item, skill);
        }

        public void UpdateLastSelectableSlot()
        {
            _lastSelectableSlot = null;
        }
    }
}