using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;
using Assets.Sources.UI.Models;
using Assets.Sources.UI.Utilites;

namespace Assets.Sources.UI
{
    [Serializable]
    public sealed class PresentModel
    {
        [HideInInspector] public int _slotIndex;
        public GameObject _childObject;
        public Image _image;
        public Button _button;
        public Image _buttonImage;
        public Text _buttonText;
        public Text _timeText;
        [HideInInspector] public PresentContract _presentContract;
        [HideInInspector] public DateTime _dateTime;
    }

    public sealed class PresentManager : MonoBehaviour
    {
        [SerializeField] private Color _buttonNoActivate;
        [SerializeField] private Color _buttonActivate;
        [SerializeField] private Sprite _emptySlot;
        [SerializeField] private Sprite _buttonNoActiveSprite;
        [SerializeField] private Sprite _buttonActiveSprite;
        [SerializeField] private PresentView _presentView;
        [SerializeField] private GameObject _panelInformation;
        [SerializeField] private Transform _spawnPanelPresentInformation;
        [SerializeField] private Sprite[] _presentsType;
        [SerializeField] private PresentModel[] _presentModels;
        [SerializeField] private PresentMachine _presentMachine;
        [SerializeField] private Canvas _canvas;

        private INetworkProcessor _networkProcessor;
        private int _howMuchWillCostReRollGiftlvl1;
        private int _howMuchWillCostReRollGiftlvl2;
        private PanelObject _viewPanelInformer;

        public static PresentManager Instance;

        public IEnumerator InternalLoadPresentWithCharacter(INetworkProcessor networkProcessor)
        {
            _networkProcessor = networkProcessor;
            Instance = this;

            GameObject panelInformation = Instantiate(_panelInformation, _spawnPanelPresentInformation);
            PanelObject panelObject = new PanelObject();

            if (!panelInformation.TryGetComponent(out ContentSizeFilterCustom contentSizeFilterCustom))
                throw new MissingComponentException(nameof(ContentSizeFilterCustom));

            if (!panelInformation.TryGetComponent(out InformationComponent informationComponent))
                throw new MissingComponentException(nameof(InformationComponent));

            if (!panelInformation.TryGetComponent(out RectTransform rectTransform))
                throw new MissingComponentException(nameof(RectTransform));

            contentSizeFilterCustom.Initialized();

            panelObject.RectTransformPanelInformation = rectTransform;
            panelObject.contentSizeFilterCustom = contentSizeFilterCustom;
            panelObject.panelInformationObject = panelInformation;
            panelObject.InformationComponentObject = informationComponent;

            panelInformation.SetActive(false);

            _viewPanelInformer = panelObject;

            for (int iterator = 0; iterator < _presentModels.Length; iterator++)
            {
                int index = iterator;

                _presentModels[iterator]._button.onClick.AddListener(() => InternalButtonOnClickHandlerOpenPresent(index));
                _presentModels[iterator]._slotIndex = iterator;
                _presentModels[iterator]._button.interactable = false;
                _presentModels[iterator]._timeText.text = "empty";
            }

            for (int iterator = 0; iterator < _presentMachine.MachineCount(); iterator++)
            {
                _presentMachine.InitSlotFromMachine(iterator, networkProcessor, _canvas);
                _presentMachine.SetStatus(iterator, false);
            }

            _presentView.Init(networkProcessor);
            networkProcessor.SendPacketAsync(ServiceGetPresents.ToPacket());
            yield return new WaitUntil(() => _networkProcessor.GetParentObject().IsLoadedPresents);
            _networkProcessor.GetParentObject().ResetFlagIsLoadedPresents();
            StartCoroutine(InternalUpdateTimePresent());
        }

        public PresentManager GetInstance()
        {
            return this;
        }

        public void SetPresentContract(PresentContract[] presentContracts)
        {
            for (int iterator = 0; iterator < presentContracts.Length; iterator++)
            {
                _presentModels[presentContracts[iterator].Slot]._presentContract = presentContracts[iterator];

                DateTime dateTime = new DateTime(presentContracts[iterator].Time);
                _presentModels[presentContracts[iterator].Slot]._dateTime = dateTime;

                _presentModels[presentContracts[iterator].Slot]._image.sprite = InternalGetSpriteWithPresentType(presentContracts[iterator].PresentType);
                _presentModels[presentContracts[iterator].Slot]._button.interactable = true;
                _presentModels[presentContracts[iterator].Slot]._buttonImage.sprite = _buttonActiveSprite;
                _presentModels[presentContracts[iterator].Slot]._buttonText.color = _buttonActivate;

                if (_presentModels[presentContracts[iterator].Slot]._dateTime.CompareTo(DateTime.Now) == -1)
                    _presentModels[presentContracts[iterator].Slot]._timeText.text = "Open!";
                else
                {
                    TimeSpan timeSpan = _presentModels[presentContracts[iterator].Slot]._dateTime.Subtract(DateTime.Now);
                    _presentModels[presentContracts[iterator].Slot]._timeText.text = Parser.ConvertTimeSpanToTimeString(timeSpan);
                }
            }
        }

        public void SetPrice(int howMuchWillCostReRollGiftlvl1, int howMuchWillCostReRollGiftlvl2)
        {
            _howMuchWillCostReRollGiftlvl1 = howMuchWillCostReRollGiftlvl1;
            _howMuchWillCostReRollGiftlvl2 = howMuchWillCostReRollGiftlvl2;
        }

        public void SetPresentContractWithOnlyAlone(PresentContract presentContract)
        {
            if (_presentModels[presentContract.Slot]._presentContract != null)
                throw new ArgumentException(nameof(PresentContract));

            _presentModels[presentContract.Slot]._presentContract = presentContract;

            DateTime dateTime = new DateTime(presentContract.Time);
            _presentModels[presentContract.Slot]._dateTime = dateTime;

            _presentModels[presentContract.Slot]._image.sprite = InternalGetSpriteWithPresentType(presentContract.PresentType);
            _presentModels[presentContract.Slot]._button.interactable = true;
            _presentModels[presentContract.Slot]._buttonImage.sprite = _buttonActiveSprite;
            _presentModels[presentContract.Slot]._buttonText.color = _buttonActivate;

            if (_presentModels[presentContract.Slot]._dateTime.CompareTo(DateTime.Now) == -1)
                _presentModels[presentContract.Slot]._timeText.text = "Open!";
            else
            {
                TimeSpan timeSpan = _presentModels[presentContract.Slot]._dateTime.Subtract(DateTime.Now);
                _presentModels[presentContract.Slot]._timeText.text = Parser.ConvertTimeSpanToTimeString(timeSpan);
            }
        }

        public void NoEnoughtCrownsForStartReRollMachine()
        {
            _presentMachine.ShowNoEnoughStartMachine();
        }

        public void StartMachineForPresent(int index)
        {
            _presentMachine.StartMachineWithIndex(index);
        }

        public void DeletePresentWithSlotIndex(int slot)
        {
            PresentModel model = _presentModels.Where(x => x._slotIndex == slot).FirstOrDefault();

            if (model == null)
                throw new ArgumentNullException(nameof(PresentModel));

            model._image.sprite = _emptySlot;
            model._button.interactable = false;
            model._timeText.text = "empty";
            model._buttonImage.sprite = _buttonNoActiveSprite;
            model._buttonText.color = _buttonNoActivate;

            int presentType = model._presentContract.PresentType;

            _presentMachine.SetInMachineTypePresent(presentType, presentType);
            _presentMachine.SetInMachineViewPanelInformer(presentType, _viewPanelInformer);
            _presentMachine.SetStatusMachine(presentType, true, _howMuchWillCostReRollGiftlvl1, _howMuchWillCostReRollGiftlvl2);

            model._presentContract = null;
        }

        public void SetItemForPresentMachine(ItemContract[] itemContracts)
        {
            _presentMachine.InternalTempItems(itemContracts);
        }

        private IEnumerator InternalUpdateTimePresent()
        {
            while (true)
            {
                for (int iterator = 0; iterator < _presentModels.Length; iterator++)
                {
                    if (_presentModels[iterator]._presentContract != null)
                    {
                        if (_presentModels[iterator]._dateTime < DateTime.Now && _presentModels[iterator]._timeText.text == "Open!")
                            continue;

                        TimeSpan timeSpan = _presentModels[iterator]._dateTime.Subtract(DateTime.Now);
                        _presentModels[iterator]._timeText.text = Parser.ConvertTimeSpanToTimeString(timeSpan);

                        int compareResult = _presentModels[iterator]._dateTime.CompareTo(DateTime.Now);
                        if (compareResult == -1 || compareResult == 0)
                            _presentModels[iterator]._timeText.text = "Open!";
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void InternalButtonOnClickHandlerOpenPresent(int slotIndex)
        {
            PresentModel presentModel = _presentModels[slotIndex];

            _presentView.gameObject.SetActive(true);
            _presentView.SetupWindow(presentModel);
        }

        private Sprite InternalGetSpriteWithPresentType(int type)
        {
            return _presentsType[type];
        }

        public void OpenOrCloseWindow()
        {
            for (int iterator = 0; iterator < _presentModels.Length; iterator++)
            {
                GameObject child = _presentModels[iterator]._childObject;
                child.SetActive(!child.activeSelf);
            }
        }
    }
}