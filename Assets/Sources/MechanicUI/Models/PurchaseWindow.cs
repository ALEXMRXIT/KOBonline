using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.MechanicUI.Models
{
    public sealed class PurchaseWindow : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Text _noEnoughCrowns;
        [SerializeField] private Text _crownGoldText;
        [SerializeField] private Text _crownSilverText;
        [SerializeField] private Text _crownCopperText;
        [SerializeField] private Button _ok;
        [SerializeField] private Button _no;

        private FuncCall _func;
        private string _id;
        private string _itemName;
        private int _price;

        public delegate void FuncCall(string id);

        public void InitButton()
        {
            _ok.onClick.AddListener(InternalOkOnClickHandler);
            _no.onClick.AddListener(InternalNoOnClickHandler);
            _noEnoughCrowns.gameObject.SetActive(false);
        }

        public void SetItem(string id, FuncCall funcCall)
        {
            _func = funcCall;
            _id = id;

            switch (id)
            {
                case "com.gentech.kob.smallbox":
                    _itemName = "<color=#0085DE>Small Box</color>";
                    _price = 4535;
                    break;
                case "com.gentech.kob.bigbox":
                    _itemName = "<color=#FFCC00>Big Box</color>";
                    _price = 15015;
                    break;
                case "com.gentech.kob.devilmanuscript":
                    _itemName = "<color=#B7007A>Devil's manuscript</color>";
                    _price = 3005;
                    break;
                case "com.gentech.kob.energyitem":
                    _itemName = "<color=#FFCC00>100 energy</color>";
                    _price = 1000;
                    break;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);

            int[] goldSplit = Parser.SplitIntToMoney(_price);

            _text.text = $"Do you really want to purchase: {_itemName}\nAttempt cost:";

            _crownGoldText.text = goldSplit[2].ToString();
            _crownSilverText.text = goldSplit[1].ToString();
            _crownCopperText.text = goldSplit[0].ToString();

            transform.SetAsLastSibling();
        }

        public void ShowNoEnoughCrowns()
        {
            gameObject.SetActive(true);
            _noEnoughCrowns.gameObject.SetActive(true);
        }

        private void InternalOkOnClickHandler()
        {
            _noEnoughCrowns.gameObject.SetActive(false);
            _func(_id);
            gameObject.SetActive(false);
        }

        private void InternalNoOnClickHandler()
        {
            _noEnoughCrowns.gameObject.SetActive(false);
            _func = null;
            gameObject.SetActive(false);
        }
    }
}