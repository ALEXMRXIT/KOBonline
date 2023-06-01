using System;
using UnityEngine;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Sources.Enums;
using Assets.Sources.Network;
using UnityEngine.EventSystems;
using Assets.Sources.Contracts;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.UI.Utilites;

#pragma warning disable

namespace Assets.Sources.UI.Models
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class SkillHandler : MonoBehaviour, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IPointerClickHandler, ICloneable
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _experienceText;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _upgradeColor;
        [SerializeField] private Color _maxLevelColor;
        [Space, SerializeField] private string _skillNameColor;
        [SerializeField] private string _costManaColor;
        [SerializeField] private string _typeSkillColor;
        [SerializeField] private string _requereWeaponSkillColor;
        [SerializeField] private string _damageColor;
        [Space, SerializeField] private GameObject _upgradeEffect;
        [SerializeField] private Animator _buttonsClickSkillHandler;
        [SerializeField] private Button _maskButton;
        [SerializeField] private Button _impactButton;
        [SerializeField] private Button _informationButton;
        [SerializeField] private Button _closeButton2;
        [SerializeField] private RectTransform _effectSelectableImage;
        [SerializeField] private RectTransform _effectSelectableEffect;
        [SerializeField] private GameObject _upgradeEffectSpawn;

        private Transform _spawnCloseButton;
        private int[] _experience;
        private int _currentexperience;
        private int _level;
        private GameObject _closeButton;
        private SkillContract _skillContract;
        private ClientProcessor _clientProcessor;
        private Transform _parentScroll;
        private Skill _skill;
        private PanelObject _informationSkillPanel;
        private bool _cloneableObject;
        private Vector3 _offsetPositionInDragged;
        private SkillManager _skillManager;
        private CanvasGroup _canvasGroup;

        public void SetSpriteToSkill(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetCloentObject()
        {
            _cloneableObject = true;
        }

        public void SetSkillExperience(params int[] experience)
        {
            _experience = experience;
        }

        public Animator GetButtonClickHandlerAnimator()
        {
            return _buttonsClickSkillHandler;
        }

        public Skill GetSkill()
        {
            return _skill;
        }

        public void SetSpawnCloseButtonAnTransform(Transform transform, Transform parentScroll)
        {
            _spawnCloseButton = transform;
            _parentScroll = parentScroll;
        }

        public void SetInformationObject(PanelObject panelObject)
        {
            _informationSkillPanel = panelObject;
        }

        public void SetSkillCurrentExperience(int experience)
        {
            _currentexperience = experience;
        }

        public void SetSkillLevel(int level)
        {
            _level = level;
        }

        public CanvasGroup GetOriginalCavasGroup()
        {
            return _canvasGroup;
        }

        public void ParseSkill(ClientProcessor clientProcessor, Skill skill,
            SkillContract skillContract, SkillManager skillManager)
        {
            _clientProcessor = clientProcessor;
            _skill = skill;
            _skillContract = skillContract;
            _skillManager = skillManager;

            if (!gameObject.TryGetComponent(out CanvasGroup canvasGroup))
                throw new MissingComponentException(nameof(CanvasGroup));

            _canvasGroup = canvasGroup;

            if (!_cloneableObject)
                InternalUpdateText();
            else
                InternalCloneableSetsState();

            _impactButton.onClick.AddListener(InternalClickHandlerImpactSkill);
            _informationButton.onClick.AddListener(InternalClickHandlerInformationSkill);
            _closeButton2.onClick.AddListener(InternalClickHandlerCloseSkillPanel);

            _informationSkillPanel.contentSizeFilterCustom.Initialized();
            InternalParseText();
            _informationSkillPanel.panelInformationObject.SetActive(false);
        }

        public void OpenButtons()
        {
            _buttonsClickSkillHandler.SetBool("manager", true);

            if (_closeButton != null)
                Destroy(_closeButton);

            _effectSelectableImage.localScale = new Vector3(1.1f, 1.1f, 1.0f);
            _effectSelectableEffect.localScale = new Vector3(1.1f, 1.1f, 1.0f);

            GameObject gameObjectButton = Instantiate(_maskButton.gameObject, _spawnCloseButton);
            gameObjectButton.transform.SetAsFirstSibling();

            if (!gameObjectButton.TryGetComponent(out Button button))
                throw new MissingComponentException(nameof(Button));

            button.onClick.AddListener(InternalClickHandlerCloseSkillPanel);
            _closeButton = gameObjectButton;
        }

        public void CloseButtons()
        {
            _buttonsClickSkillHandler.SetBool("manager", false);
            if (_closeButton != null)
                Destroy(_closeButton);

            _informationSkillPanel.panelInformationObject.SetActive(false);
            _effectSelectableImage.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _effectSelectableEffect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private void InternalParseText()
        {
            StringBuilder stringBuilder = new StringBuilder(capacity: 512);

            bool distance = _skillContract.Distance > 10;
            string melee = distance ? "(Melee)" : string.Empty;
            string requestWeaponString = !distance ? "Requeres a melee weapon" : "Requeres a ranged weapon";
            string appendLine = "\n";
            string appendLineNextLevel = string.Empty;

            if (_skillContract.TypeSkill == SkillType.Passive && !distance)
            {
                requestWeaponString = string.Empty;
                appendLine = string.Empty;
            }

            if (_level > 0)
            {
                stringBuilder.AppendFormat("<color={0}>{1}</color> (Level {2}/{3})\n" +
                    "<color={4}>{5}</color> mana {6}\n{7}<color={8}>{9}</color>\n{10}",
                    _skillNameColor, _skillContract.Name, _level, _experience.Length,
                    _costManaColor, _skillContract.Mana[_level > 0 ? _level - 1 : 0], melee, appendLine,
                    _requereWeaponSkillColor, requestWeaponString, ParseArgs(_skillContract.Description, true, SetArgsWithSkillId(_skillContract.Id, false)));
                appendLineNextLevel = "\n\n\n";
            }

            if (_level < _experience.Length)
            {
                stringBuilder.AppendFormat("{0}<color=#C3C3C3>{1} (Level {2}/{3})\n{4} mana {5}\n{6}{7}\n{8}</color>", appendLineNextLevel,
                    _skillContract.Name, _level + 1, _experience.Length, _skillContract.Mana[_level >= _experience.Length - 1 ? _level : _level + 1],
                    melee, appendLine, requestWeaponString, ParseArgs(_skillContract.Description, false, SetArgsWithSkillId(_skillContract.Id, true)));
            }

            _informationSkillPanel.InformationComponentObject.SetText(stringBuilder.ToString());
            Canvas.ForceUpdateCanvases();
            _informationSkillPanel.contentSizeFilterCustom.ContentUpdate();
        }

        private string ParseArgs(string description, bool isParseColor, object[] args)
        {
            string temp = description;

            int lastIndex = 0;
            for (int iterator = 0; iterator < args.Length; iterator++)
            {
                lastIndex = temp.IndexOf("%d", lastIndex);

                if (lastIndex == -1)
                    break;

                string arg = args[iterator].ToString();
                temp = temp.Remove(lastIndex, 2);
                temp = temp.Insert(lastIndex, arg);

                if (isParseColor)
                {
                    temp = temp.Insert(lastIndex, _damageColor);
                    temp = temp.Insert(lastIndex + arg.Length + _damageColor.Length, "</color>");
                }
            }

            return temp;
        }

        private object[] SetArgsWithSkillId(long skillId, bool nextLevel)
        {
            switch (skillId)
            {
                case 0:
                    if (nextLevel) return new object[] { _skillContract.BaseDamage[_level] };
                    else return new object[] { _skillContract.BaseDamage[_level > 0 ? _level - 1 : 0] };
                case 1:
                    if (nextLevel) return new object[] { _skillContract.AddPhysDef[_level], _skillContract.TimeUse };
                    else return new object[] { _skillContract.AddPhysDef[_level > 0 ? _level - 1 : 0], _skillContract.TimeUse };
                case 2:
                    if (nextLevel) return new object[] { _skillContract.BaseDamage[_level], _skillContract.AttackSpeed[_level] * 100f };
                    else return new object[] { _skillContract.BaseDamage[_level > 0 ? _level - 1 : 0], _skillContract.AttackSpeed[_level > 0 ? _level - 1 : 0] * 100f };
                case 3:
                    if (nextLevel) return new object[] { _skillContract.BaseDamage[_level] };
                    else return new object[] { _skillContract.BaseDamage[_level > 0 ? _level - 1 : 0] };
                case 4:
                    if (nextLevel) return new object[] { _skillContract.AddPhysDef[_level], _skillContract.TimeUse };
                    else return new object[] { _skillContract.AddPhysDef[_level > 0 ? _level - 1 : 0], _skillContract.TimeUse };
                case 5:
                    if (nextLevel) return new object[] { _skillContract.AddHealth[_level], _skillContract.HealthRegeneration[_level], _skillContract.TimeUse };
                    else return new object[] { _skillContract.AddHealth[_level > 0 ? _level - 1 : 0], _skillContract.HealthRegeneration[_level > 0 ? _level - 1 : 0], _skillContract.TimeUse };
                default: return null;
            }
        }

        private void InternalUpdateText()
        {
            _experienceText.gameObject.SetActive(true);
            bool tempBoolean;

            if (_level < _experience.Length)
                tempBoolean = _currentexperience >= _experience[_level];
            else
                tempBoolean = false;

            if (!tempBoolean && _level < _experience.Length)
            {
                _experienceText.text = $"{_currentexperience}/{_experience[_level]}";
                _experienceText.color = _defaultColor;
                _upgradeEffect.SetActive(tempBoolean);
            }
            else if (_level < _experience.Length)
            {
                _experienceText.text = $"{_currentexperience}/{_experience[_level]}";
                _experienceText.color = _upgradeColor;
            }
            else
            {
                _experienceText.text = $"MAX. LVL.";
                _experienceText.color = _maxLevelColor;
            }

            _upgradeEffect.SetActive(tempBoolean);

            if (_level > 0)
                SetSpriteToSkill(_skill.SkillSprite);
        }

        private void InternalCloneableSetsState()
        {
            _experienceText.gameObject.SetActive(false);
            _upgradeEffect.SetActive(false);

            if (_level > 0)
                SetSpriteToSkill(_skill.SkillSprite);
        }

        private void InternalClickHandlerImpactSkill()
        {
            if (_level >= _experience.Length)
                return;

            if (_currentexperience < _experience[_level])
                return;

            _currentexperience -= _experience[_level++];
            InternalUpdateText();

            if (_informationSkillPanel.panelInformationObject.activeSelf)
                InternalParseText();

            _clientProcessor.SendPacketAsync(SendUpgradeSkill.ToPacket(_skillContract.Id));
            GameObject obj = Instantiate(_upgradeEffectSpawn, gameObject.transform);
            obj.transform.SetParent(_parentScroll);
            obj.transform.SetAsLastSibling();
            Destroy(obj, 1f);
        }

        private void InternalClickHandlerInformationSkill()
        {
            _informationSkillPanel.panelInformationObject.SetActive(true);
            InternalParseText();
        }

        private void InternalClickHandlerCloseSkillPanel()
        {
            CloseButtons();
            if (_closeButton != null)
                Destroy(_closeButton);
            _informationSkillPanel.panelInformationObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_level == 0)
                return;

            if (!_cloneableObject)
            {
                _offsetPositionInDragged = transform.position - Input.mousePosition;
                eventData.pointerDrag = CreateCloneSkill();

                return;
            }

            GetOriginalCavasGroup().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_cloneableObject || _level == 0)
                return;

            if (eventData.dragging)
                transform.position = Input.mousePosition + _offsetPositionInDragged;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_cloneableObject || _level == 0)
                return;

            RaycastResult raycastResult = eventData.pointerCurrentRaycast;
            if (raycastResult.gameObject != null)
            {
                if (raycastResult.gameObject.TryGetComponent(out Slot slot))
                {
                    if (!slot.IsSlotEmpty())
                    {
                        _clientProcessor.GetParentObject().GetSkillDatas.Where(
                            skill => skill.SkillId == _skill.Id).FirstOrDefault().SlotId = -1;
                        slot.DestroyItem();
                    }

                    SkillHandler skillHandler = eventData.pointerDrag.GetComponent<SkillHandler>();
                    slot.EnterToSlotObject(eventData.pointerDrag, skillHandler.GetSkill());

                    _clientProcessor.GetParentObject().GetSkillDatas.Where(
                        skill => skill.SkillId == _skill.Id).FirstOrDefault().SlotId = slot.GetSlotId();

                    _clientProcessor.SendPacketAsync(SendUpgradeSkill.ToPacket(skillHandler.GetSkill().Id, true, slot.GetSlotId()));
                    skillHandler.GetOriginalCavasGroup().blocksRaycasts = true;
                }
                else
                    Destroy(eventData.pointerDrag);
            }
            else
                Destroy(eventData.pointerDrag);
        }

        public GameObject CreateCloneSkill()
        {
            GameObject skillDragging = Instantiate(gameObject, _parentScroll);
            SkillHandler skillHandler = skillDragging.GetComponent<SkillHandler>();
            skillHandler.SetCloentObject();
            Destroy(skillHandler.GetButtonClickHandlerAnimator().gameObject);
            skillHandler.SetSkillExperience(_experience);
            skillHandler.SetSkillCurrentExperience(_currentexperience);
            skillHandler.SetSkillLevel(_level);
            skillHandler.SetInformationObject(_informationSkillPanel);
            skillHandler.SetSpawnCloseButtonAnTransform(_spawnCloseButton, _parentScroll);
            skillHandler.SetSpriteToSkill(_image.sprite);
            skillHandler.ParseSkill(_clientProcessor, _skill, _skillContract, _skillManager);
            RectTransform rectTransform = skillDragging.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(120.0f, 120.0f);
            skillHandler.GetOriginalCavasGroup().blocksRaycasts = false;

            return skillDragging;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _skillManager.CloseAllSelectedMenu();
            OpenButtons();
            _skillManager.SetLastUseSkillHandler(this);
        }

        public object Clone()
        {
            return CreateCloneSkill();
        }
    }
}