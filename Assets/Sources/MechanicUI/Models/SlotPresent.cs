using System;
using System.Text;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Assets.Sources.UI;
using UnityEngine.EventSystems;

namespace Assets.Sources.MechanicUI.Models
{
    [RequireComponent(typeof(Image))]
    public sealed class SlotPresent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _effect;
        [SerializeField] private string _colorArguments;

        private Image _image;
        private long _itemId;
        private PanelObject _panelObject;

        public void Init()
        {
            _image = GetComponent<Image>();
            SetEffectDisable();
        }

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetItemId(long itemId)
        {
            _itemId = itemId;
        }

        public void SetPanelView(PanelObject panelObject)
        {
            _panelObject = panelObject;
        }

        public Sprite GetCurrentSlotSprite()
        {
            return _image.sprite;
        }

        public GameObject SetEffectEnable()
        {
            _effect.SetActive(true);
            return _effect;
        }

        public GameObject SetEffectDisable()
        {
            _effect.SetActive(false);
            return _effect;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(_colorArguments))
                throw new ArgumentNullException(nameof(_colorArguments));

            _panelObject.panelInformationObject.SetActive(true);

            StringBuilder stringBuilder = new StringBuilder(capacity: 64);

            switch (_itemId)
            {
                case 0:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFFFFF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Candy filled with a source of joy. Gives you %d experience.", true, new object[] { 5 })); break;
                case 1:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#009FFF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("A vial of demon blood that can give you a boost of energy. Gives you %d experience.", true, new object[] { 15 })); break;
                case 2:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Golden Oracle of Warriors. Gives you %d experience.", true, new object[] { 45 })); break;
                case 3:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0AFF00>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Chocolate pie. Gives you %d experience.", true, new object[] { 100 })); break;
                case 4:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Enhancing strength extract. Increases strength by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 5:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Enhancing agility extract. Increases agility by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 6:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Enhancing intelligence extract. Increases intelligence by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 7:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Enhancing endurance extract. Increases endurance by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 8:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFFFFF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 1 })); break;
                case 9:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#009FFF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 5 })); break;
                case 10:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#F8AA02>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 10 })); break;
                case 11:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0AFF00>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 15 })); break;
                case 12:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#D700FF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 20 })); break;
                case 13:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FF0005>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Get %d upgrade point for a random ability.", true, new object[] { 25 })); break;
                case 14:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0AFF00>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Greater Potion of Empowerment! Increases your strength by %d units for %d minutes.", true, new object[] { 30, 10 })); break;
                case 15:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0AFF00>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Greater Potion of Empowerment! Increases your intellect by %d points for %d minutes.", true, new object[] { 30, 10 })); break;
                case 16:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#D700FF>", "Candy", "<color=#FFFFFF>",
                        ParseArgs("Enchanted Scroll. Increases your strength and intelligence by %d% and regenerates your health from base damage over %d minutes.", true, new object[] { 10, 30 })); break;
            }

            _panelObject.InformationComponentObject.SetText(stringBuilder.ToString());
            Canvas.ForceUpdateCanvases();
            _panelObject.contentSizeFilterCustom.ContentUpdate();

            _panelObject.RectTransformPanelInformation.position = transform.position;
        }

        private string ParseArgs(string description, bool isParseColor, object[] args)
        {
            string temp = description;

            if (args == null || args.Length == 0)
                return string.Empty;

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
                    int countLitter = arg.Where(s => char.IsLetter(s)).Count();

                    temp = temp.Insert(lastIndex, _colorArguments);
                    temp = temp.Insert(lastIndex + arg.Length + _colorArguments.Length - countLitter, "</color>");
                }
            }

            return temp;
        }
    }
}