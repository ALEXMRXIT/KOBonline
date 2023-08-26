using UnityEngine;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;

namespace Assets.Sources.MechanicUI
{
    public sealed class PresentMachine : MonoBehaviour
    {
        [Space, SerializeField] private MachineModel[] _machineModel;
        [SerializeField] private Sprite[] _itemsView;

        private ItemContract[] _itemContracts;

        public void InitSlotFromMachine(int index, INetworkProcessor networkProcessor)
        {
            _machineModel[index].Init(networkProcessor);
        }

        public void SetStatus(int index, bool status)
        {
            _machineModel[index].SetStatusMachine(status);
        }

        public void SetStatusMachine(int index, bool status)
        {
            _machineModel[index].SetStatusMachine(status);
            _machineModel[index].SetItems(_itemContracts);
            _machineModel[index].SetItemView(_itemsView);
            _machineModel[index].StartBuild();

            StartCoroutine(_machineModel[index].StartMachine());
        }

        public void InternalTempItems(ItemContract[] itemContracts)
        {
            _itemContracts = itemContracts;
        }

        public int MachineCount()
        {
            return _machineModel.Length;
        }
    }
}