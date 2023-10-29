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
        [SerializeField] private bool _ignoreSlot = false;

        private Image _image;
        private long _itemId;
        private PanelObject _panelObject;
        private Canvas _canvas;

        public void Init(Canvas canvas)
        {
            _image = GetComponent<Image>();
            _canvas = canvas;

            SetEffectDisable();
        }

        public bool IfIgnoreSlots()
        {
            return _ignoreSlot;
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
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFFFFF>", "[Ordinary] Sugar star", "<color=#FFFFFF>",
                        ParseArgs("A forbidden spell from a dark magician imbues candy with the knowledge of undefeated warriors. Gives you %d experience.", true, new object[] { 5 })); break;
                case 1:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0085DE>", "[Good] Elixir of the damned", "<color=#FFFFFF>",
                        ParseArgs("The plea for mercy, the last breath and the blood of the defeated are contained in an elixir that gives you experience. Gives you %d experience.", true, new object[] { 15 })); break;
                case 2:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Bottle of lost souls", "<color=#FFFFFF>",
                        ParseArgs("Skilled magicians of Tangramia imprisoned the souls and experience of the best warriors in an enchanted jar. Gives you %d experience.", true, new object[] { 45 })); break;
                case 3:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#00FF09>", "[Daedalus] World Tree grain", "<color=#FFFFFF>",
                        ParseArgs("The imprisoned gods of war want revenge on those responsible. Born from the wrath of the gods, a potion gives you the power to take revenge. Gives you %d experience.", true, new object[] { 100 })); break;
                case 4:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Stones of power", "<color=#FFFFFF>",
                        ParseArgs("For hundreds of years, the deadly beast tore and devoured the flesh of all who stood in its path, causing horror and madness. Golden Lion Stones grants you agility. Increases strength by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 5:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Golden Lion Stones", "<color=#FFFFFF>",
                        ParseArgs("For hundreds of years, the deadly beast tore and devoured the flesh of all who stood in its path, causing horror and madness. Golden Lion Stones grants you agility. Increases agility by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 6:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Sorcerer's stones", "<color=#FFFFFF>",
                        ParseArgs("All the accumulated knowledge of the ancient sorcerers gives you intellect. Increases intelligence by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 7:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Celestial stones", "<color=#FFFFFF>",
                        ParseArgs("To summon the gods of war, great magicians have encapsulated the elements of all things in celestial stones that gives you stamina. Increases endurance by %d for %d minutes.", true, new object[] { 10, 10 })); break;
                case 8:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFFFFF>", "[Ordinary] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 1 })); break;
                case 9:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0085DE>", "[Good] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 5 })); break;
                case 10:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 10 })); break;
                case 11:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#00FF09>", "[Daedalus] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 15 })); break;
                case 12:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#B7007A>", "[Epic] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 20 })); break;
                case 13:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#B7007A>", "[Epic] Monolith fragment", "<color=#FFFFFF>",
                        ParseArgs("For thousands of years, the monolith generated by dark magicians fed on their powers. A forbidden spell split him into many parts. The monolith fragment randomly selects an ability and increases it by %d amount of experience.", true, new object[] { 25 })); break;
                case 14:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#00FF09>", "[Daedalus] Potion of God's Revenge", "<color=#FFFFFF>",
                        ParseArgs("The imprisoned gods of war want revenge on those responsible. Born from the wrath of the gods, a potion gives you the power to take revenge. Increases your strength by %d units for %d minutes.", true, new object[] { 30, 10 })); break;
                case 15:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#00FF09>", "[Daedalus] Potion of hatred", "<color=#FFFFFF>",
                        ParseArgs("The cursed gods, seeking vengeance, have used dark rituals to encapsulate their hatred in a potion that gives you intellect. Increases your intellect by %d points for %d minutes.", true, new object[] { 30, 10 })); break;
                case 16:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#B7007A>", "[Epic] Devil's manuscript", "<color=#FFFFFF>",
                        ParseArgs("The pages of the manuscript are made of flesh, and the spells are written in the blood of immortal magicians. Increases your strength and intelligence by %d% and regenerates your health from base damage over %d minutes.", true, new object[] { 10, 30 })); break;
            }

            _panelObject.InformationComponentObject.SetText(stringBuilder.ToString());
            Canvas.ForceUpdateCanvases();
            _panelObject.contentSizeFilterCustom.ContentUpdate();

            _panelObject.RectTransformPanelInformation.position = transform.position;
            Vector2 anchoredPosition = _panelObject.RectTransformPanelInformation.position;
            float leftEdgeToScreenEdgeDistance = 0 - (_panelObject.RectTransformPanelInformation.position.x - _panelObject.
                RectTransformPanelInformation.rect.width * _canvas.scaleFactor / 2);
            if (leftEdgeToScreenEdgeDistance > 0) anchoredPosition.x += leftEdgeToScreenEdgeDistance;

            float downEdgeToScreenEdgeDistance = 0 - (_panelObject.RectTransformPanelInformation.position.y - _panelObject.
                RectTransformPanelInformation.rect.height * _canvas.scaleFactor);
            if (downEdgeToScreenEdgeDistance > 0) anchoredPosition.y += downEdgeToScreenEdgeDistance;
            _panelObject.RectTransformPanelInformation.position = anchoredPosition;
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