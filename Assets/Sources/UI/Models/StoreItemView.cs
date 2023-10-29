using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.UI.Models
{
    public sealed class StoreItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string _colorArguments;

        private PanelObject _panelObject;
        private Canvas _canvas;
        private int _index;

        public void Init(PanelObject panelObject, Canvas canvas, int index)
        {
            _panelObject = panelObject;
            _canvas = canvas;
            _index = index;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _panelObject.panelInformationObject.SetActive(true);

            StringBuilder stringBuilder = new StringBuilder(capacity: 256);

            switch (_index)
            {
                case 0:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFFFFF>", "[Ordinary] Small box", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d silver coins.", true, new object[] { 25 })); break;
                case 1:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0085DE>", "[Good] Small chest", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d silver coins.", true, new object[] { 50 })); break;
                case 2:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Medium chest with gold", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d gold coins.", true, new object[] { 1 })); break;
                case 3:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#00FF09>", "[Daedalus] Large golden chest", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d gold coins.", true, new object[] { 25 })); break;
                case 4:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#B7007A>", "[Epic] Huge chest of gold", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d gold coins.", true, new object[] { 100 })); break;
                case 5:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#0085DE>", "[Good] Small box", "<color=#FFFFFF>",
                        "You can get one of the following: <color=#FFFFFF>[Ordinary] Sugar star</color> X1, " +
                        "<color=#0085DE>[Good] Elixir of the damned</color> X1, <color=#FFCC00>[Rare] Bottle of lost souls</color> X1, " +
                        "<color=#FFCC00>[Rare] Stones of power</color> X1, <color=#FFCC00>[Rare] Golden Lion Stones</color> X1, " +
                        "<color=#FFCC00>[Rare] Sorcerer's stones</color> X1, <color=#FFCC00>[Rare] Celestial stones</color> X1, " +
                        "<color=#FFFFFF>[Ordinary] Monolith fragment</color> X1, <color=#0085DE>[Good] Monolith fragment</color> X1, " +
                        "<color=#FFCC00>[Rare] Monolith fragment</color> X1, <color=#00FF09>[Daedalus] Monolith fragment</color> X1, " +
                        "<color=#B7007A>[Epic] Devil's manuscript</color> X1"); break;
                case 6:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Big box", "<color=#FFFFFF>",
                        "You can get one of the following: <color=#FFCC00>[Rare] Bottle of lost souls</color> X1, " +
                        "<color=#00FF09>[Daedalus] World Tree grain</color> X1, " +
                        "<color=#FFCC00>[Rare] Stones of power</color> X1, <color=#FFCC00>[Rare] Golden Lion Stones</color> X1, " +
                        "<color=#FFCC00>[Rare] Sorcerer's stones</color> X1, <color=#FFCC00>[Rare] Celestial stones</color> X1, " +
                        "<color=#FFFFFF>[Ordinary] Monolith fragment</color> X1, <color=#0085DE>[Good] Monolith fragment</color> X1, " +
                        "<color=#FFCC00>[Rare] Monolith fragment</color> X1, <color=#00FF09>[Daedalus] Monolith fragment</color> X1, " +
                        "<color=#B7007A>[Epic] Devil's manuscript</color> X1, <color=#B7007A>[Epic] Monolith fragment</color> X2, " +
                        "<color=#00FF09>[Daedalus] Potion of God's Revenge</color> X1, <color=#00FF09>[Daedalus] Potion of hatred</color> X1"); break;
                case 7:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#B7007A>", "[Epic] Devil's manuscript", "<color=#FFFFFF>",
                        ParseArgs("The pages of the manuscript are made of flesh, and the spells are written in the blood of immortal magicians. Increases your strength and intelligence by %d% and regenerates your health from base damage over %d minutes.", true, new object[] { 10, 30 })); break;
                case 8:
                    stringBuilder.AppendFormat("{0}{1}</color>\n\n{2}{3}</color>", "<color=#FFCC00>", "[Rare] Energy", "<color=#FFFFFF>",
                        ParseArgs("You will receive %d energy.", true, new object[] { 100 })); break;
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