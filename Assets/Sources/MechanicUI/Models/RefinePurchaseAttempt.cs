using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.MechanicUI.Models
{
    public sealed class RefinePurchaseAttempt : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Text _noEnoughCrowns;
        [SerializeField] private Text _crownGoldText;
        [SerializeField] private Text _crownSilverText;
        [SerializeField] private Text _crownCopperText;
        [SerializeField] private Button _ok;
        [SerializeField] private Button _no;

        private int _price;
        private INetworkProcessor _networkProcessor;
        private FuncCall _func;
        private int _attempt;

        public delegate void FuncCall();

        public void SetPrice(int price)
        {
            _price = price;
        }

        public void SetNetworkProcessor(INetworkProcessor networkProcessor)
        {
            _networkProcessor = networkProcessor;
        }

        public void InitButton()
        {
            _ok.onClick.AddListener(InternalOkOnClickHandler);
            _no.onClick.AddListener(InternalNoOnClickHandler);
            _noEnoughCrowns.gameObject.SetActive(false);
        }

        public void Show(FuncCall funcCall)
        {
            int[] goldSplit = Parser.SplitIntToMoney(_price << _attempt);

            _crownGoldText.text = goldSplit[2].ToString();
            _crownSilverText.text = goldSplit[1].ToString();
            _crownCopperText.text = goldSplit[0].ToString();

            _func = funcCall;
            transform.SetAsLastSibling();
        }

        public void ShowNoEnoughCrowns()
        {
            gameObject.SetActive(true);
            _noEnoughCrowns.gameObject.SetActive(true);
        }

        public void ResetAttempt()
        {
            _attempt = 0;
        }

        private void InternalOkOnClickHandler()
        {
            _noEnoughCrowns.gameObject.SetActive(false);
            _func();
            _attempt++;
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