using UnityEngine;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.UI;

namespace Assets.Sources.MechanicUI
{
    public sealed class PresentMachine : MonoBehaviour
    {
        [SerializeField] private RefinePurchaseAttempt _refine;
        [Space, SerializeField] private MachineModel[] _machineModel;
        [SerializeField] private Sprite[] _itemsView;

        private ItemContract[] _itemContracts;

        public void InitSlotFromMachine(int index,
            INetworkProcessor networkProcessor, Canvas canvas)
        {
            if (_refine.gameObject.activeSelf)
            {
                _refine.InitButton();
                _refine.SetNetworkProcessor(networkProcessor);
                _refine.gameObject.SetActive(false);

                Debug.Log($"Initialise: {nameof(RefinePurchaseAttempt)} window.");
            }

            _machineModel[index].Init(networkProcessor, _refine, canvas);
        }

        public void SetInMachineTypePresent(int index, int typePresent)
        {
            _machineModel[index].SetInMachineTypePresent(typePresent);
        }

        public void SetInMachineViewPanelInformer(int index, PanelObject panelObject)
        {
            _machineModel[index].SetViewInformer(panelObject);
        }

        public void ShowNoEnoughStartMachine()
        {
            _refine.ShowNoEnoughCrowns();
        }

        public void StartMachineWithIndex(int index)
        {
            StartCoroutine(_machineModel[index].StartMachine());
        }

        public void SetStatus(int index, bool status)
        {
            _machineModel[index].SetStatusMachine(status);
        }

        public void SetStatusMachine(int index, bool status,
            int howMuchWillCostReRollGiftlvl1, int howMuchWillCostReRollGiftlvl2)
        {
            _machineModel[index].SetStatusMachine(status);
            _machineModel[index].SetItems(_itemContracts);
            _machineModel[index].SetItemView(_itemsView);
            _machineModel[index].StartBuild(howMuchWillCostReRollGiftlvl1, howMuchWillCostReRollGiftlvl2);

            StartMachineWithIndex(index);
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