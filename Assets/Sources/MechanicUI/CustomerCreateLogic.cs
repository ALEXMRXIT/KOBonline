using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Sources.Models.Characters;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.MechanicUI
{
    [Serializable]
    internal sealed class EditCharacterComponent
    {
        public Button _buttonLeft;
        public Button _buttonRight;
        [Space] public Text _textView;
        [HideInInspector] public int _currentIndex = 0;
        public CustomerType _customerType;
        public string[] _names;
    }

    public sealed class CustomerCreateLogic : MonoBehaviour
    {
        [SerializeField]
        private List<EditCharacterComponent> editCharacterComponents =
            new List<EditCharacterComponent>();
        [Space][SerializeField] private CustomerModelView _customerModelView;
        [SerializeField] private Button _createCharacterButton;
        [SerializeField] private InputField _characterNameInputField;
        [SerializeField] private Text _errorTextMessage;
        [SerializeField] private Text _descriptionClass;
        [SerializeField] private Image _iconSlot;
        [SerializeField] private Sprite[] _iconsClass;

        private INetworkProcessor _networkProcessor;
        private GameObject _oldPlayer;

        public static CustomerCreateLogic Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _networkProcessor = ClientProcessor.Instance;

            foreach (EditCharacterComponent editCharacterComponent in editCharacterComponents)
            {
                editCharacterComponent._buttonLeft.onClick.AddListener(() =>
                    ButtonHandler(ButtonDirectionAxis.ButtonClickLeft,
                        editCharacterComponent._customerType, editCharacterComponent));

                editCharacterComponent._buttonRight.onClick.AddListener(() =>
                    ButtonHandler(ButtonDirectionAxis.ButtonClickRight,
                        editCharacterComponent._customerType, editCharacterComponent));
            }

            _createCharacterButton.onClick.AddListener(ButtonCharacterCreateHandler);
        }

        public GameObject ShowModelForCreateVisualPlayer()
        {
            return _oldPlayer = _customerModelView.ShowModel();
        }

        private void ButtonCharacterCreateHandler()
        {
            ShowErrorMessage(string.Empty);

            if (!ValidCharacterName())
                return;

            PlayerSex playerSex = _customerModelView.BuildPlayerContract.Sex;
            BaseClass baseClass = _customerModelView.BuildPlayerContract.CharacterBaseClass;

            _networkProcessor.SendPacketAsync(CreateCharacter.ToPacket
                (_characterNameInputField.text, playerSex, baseClass));
        }

        private bool ValidCharacterName()
        {
            if (!_networkProcessor.IsConnected)
            {
                ShowErrorMessage($"Server connection error, please check your internet connection and try again.");
                return false;
            }

            if (string.IsNullOrEmpty(_characterNameInputField.text))
            {
                ShowErrorMessage($"Name cannot be empty.");
                return false;
            }

            if (_characterNameInputField.text.Length < 4)
            {
                ShowErrorMessage($"Name cannot be less than 4 characters long.");
                return false;
            }

            return true;
        }

        public void ShowErrorMessage(string message)
        {
            _errorTextMessage.text = message;
        }

        private void ButtonHandler
            (ButtonDirectionAxis buttonDirectionAxis, CustomerType customerType,
            EditCharacterComponent editCharacterComponent)
        {
            switch (buttonDirectionAxis)
            {
                case ButtonDirectionAxis.ButtonClickLeft:
                    editCharacterComponent._currentIndex--;
                    break;
                case ButtonDirectionAxis.ButtonClickRight:
                    editCharacterComponent._currentIndex++;
                    break;
            }

            editCharacterComponent._currentIndex = ClampIndexOutOfRangeValid
                (editCharacterComponent._currentIndex, 0, editCharacterComponent._names.Length);

            if (customerType == CustomerType.BaseClass)
            {
                _iconSlot.sprite = _iconsClass[editCharacterComponent._currentIndex];

                switch ((BaseClass)editCharacterComponent._currentIndex)
                {
                    case BaseClass.Warrior:
                        switch (lang.CurrentLangSelected)
                        {
                            case LangType.English: _descriptionClass.text = lang.ClassDescriptionWarrior; break;
                        }
                        break;
                    case BaseClass.Mage:
                        switch (lang.CurrentLangSelected)
                        {
                            case LangType.English: _descriptionClass.text = lang.ClassDescriptionWizzard; break;
                        }
                        break;
                }
            }

            if (_oldPlayer != null)
            {
                Destroy(_oldPlayer);
                _oldPlayer = null;
            }

            switch (customerType)
            {
                case CustomerType.Sex:
                    _oldPlayer = _customerModelView.ModelSetSex((PlayerSex)editCharacterComponent._currentIndex, showModel: true);
                    break;
                case CustomerType.BaseClass:
                    _oldPlayer = _customerModelView.ModelSetBaseClass((BaseClass)editCharacterComponent._currentIndex, showModel: true);
                    break;
            }

            editCharacterComponent._textView.text = editCharacterComponent.
                _names[editCharacterComponent._currentIndex];
        }

        private void OnDisable()
        {
            if (_oldPlayer != null)
            {
                Destroy(_oldPlayer);
                _oldPlayer = null;
            }
        }

        private int ClampIndexOutOfRangeValid(int value, int min, int max)
        {
            return value < min ? max - 1 : (value >= max ? min : value);
        }
    }
}