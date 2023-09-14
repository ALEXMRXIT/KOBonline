using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.UI;
using System.Collections;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    public sealed class MachineModel : MonoBehaviour
    {
        [SerializeField] private Image _coreSlot;
        [SerializeField] private Button _takeItem;
        [SerializeField] private Button _playAgain;
        [SerializeField] private SlotPresent[] _slots;

        private ItemContract[] _itemContracts;
        private Sprite[] _itemView;
        private INetworkProcessor _networkProcessor;
        private float _duration;
        private int _indexItem;
        private const float DURATION_MAX = 0.15f;
        private long[] _itemId;
        private int _typePresent;
        private RefinePurchaseAttempt _refine;
        private PanelObject _panelObject;

        public void Init(INetworkProcessor networkProcessor, RefinePurchaseAttempt refinePurchaseAttempt)
        {
            for (int iterator = 0; iterator < _slots.Length; iterator++)
                _slots[iterator].Init();

            _networkProcessor = networkProcessor;
            _refine = refinePurchaseAttempt;

            _takeItem.onClick.AddListener(InternalTakeItemClickHandler);
            _playAgain.onClick.AddListener(InternalPlayAgainClickHandler);
        }

        public void SetInMachineTypePresent(int typePresent)
        {
            _typePresent = typePresent;
        }

        public void SetViewInformer(PanelObject panelObject)
        {
            _panelObject = panelObject;
        }

        public void SetStatusMachine(bool status)
        {
            gameObject.SetActive(status);
        }

        public void SetItems(ItemContract[] itemContracts)
        {
            _itemContracts = itemContracts;
        }

        public void SetItemView(Sprite[] view)
        {
            _itemView = view;
        }

        public void StartBuild(int howMuchWillCostReRollGiftlvl1, int howMuchWillCostReRollGiftlvl2)
        {
            try
            {
                switch (_typePresent)
                {
                    case 0: _refine.SetPrice(howMuchWillCostReRollGiftlvl1); break;
                    case 1: _refine.SetPrice(howMuchWillCostReRollGiftlvl2); break;
                }

                _itemId = new long[_itemContracts.Length];

                for (int iterator = 0; iterator < _itemContracts.Length; iterator++)
                {
                    _slots[iterator].SetImage(_itemView[_itemContracts[iterator].ItemId]);
                    _slots[iterator].SetItemId(_itemContracts[iterator].ItemId);
                    _slots[iterator].SetPanelView(_panelObject);
                    _itemId[iterator] = _itemContracts[iterator].ItemId;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"EXCEPTION: {exception.Message}");
            }
        }

        public IEnumerator StartMachine()
        {
            int index = UnityEngine.Random.Range(0, _slots.Length - 1);
            _duration = 0f;
            float smoothed = 0.01f;
            GameObject effect = null;

            _coreSlot.sprite = _slots[index].GetCurrentSlotSprite();

            while (_duration <= DURATION_MAX)
            {
                if (effect != null)
                    effect.SetActive(false);

                int tempIndex = IncrementIndex(ref index);
                effect = _slots[tempIndex].SetEffectEnable();
                _coreSlot.sprite = _slots[tempIndex].GetCurrentSlotSprite();

                yield return new WaitForSeconds(smoothed += Time.deltaTime);
                _duration += UnityEngine.Random.Range(0.002f, 0.004f);
            }

            if (_duration >= DURATION_MAX)
                _indexItem = index;
        }

        private int IncrementIndex(ref int value)
        {
            if (value >= 12)
                return value = 0;

            return value++;
        }

        private void InternalTakeItemClickHandler()
        {
            if (_duration <= DURATION_MAX)
                return;

            _networkProcessor.SendPacketAsync(TakeItemFromPresentWinner.ToPacket(0x00, _itemId[_indexItem == 0 ? 0 : _indexItem - 1], _typePresent));
            gameObject.SetActive(false);

            _refine.gameObject.SetActive(false);
            _refine.ResetAttempt();
        }

        private void InternalPlayAgainClickHandler()
        {
            if (_duration <= DURATION_MAX)
                return;

            _refine.gameObject.SetActive(true);
            _refine.Show(() => SendinBuyReRollPresent());
        }

        private void SendinBuyReRollPresent()
        {
            _networkProcessor.SendPacketAsync(TakeItemFromPresentWinner.ToPacket(0x01, _itemId[_indexItem == 0 ? 0 : _indexItem - 1], _typePresent));
        }
    }
}