using UnityEngine;
using UnityEngine.UI;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.MechanicUI.Models
{
    public sealed class SingleBattleWindow : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Text _noEnoughCrowns;
        [SerializeField] private Text _crownGoldText;
        [SerializeField] private Text _crownSilverText;
        [SerializeField] private Text _crownCopperText;
        [SerializeField] private GameObject _panelCrowns;
        [SerializeField] private Button _ok;
        [SerializeField] private Button _no;

        private FuncCall _func;

        public delegate void FuncCall();

        public void InitButton()
        {
            _ok.onClick.AddListener(InternalOkOnClickHandler);
            _no.onClick.AddListener(InternalNoOnClickHandler);
            _noEnoughCrowns.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _panelCrowns.SetActive(false);
        }

        public void Show(FuncCall funcCall, string message, string error, int price)
        {
            gameObject.SetActive(true);
            int[] goldSplit = Parser.SplitIntToMoney(price);

            if (!string.IsNullOrEmpty(message))
                _text.text = message;

            if (!string.IsNullOrEmpty(error))
            {
                _noEnoughCrowns.gameObject.SetActive(true);
                _noEnoughCrowns.text = error;
            }

            if (price > 0)
            {
                _panelCrowns.SetActive(true);
                _crownGoldText.text = goldSplit[2].ToString();
                _crownSilverText.text = goldSplit[1].ToString();
                _crownCopperText.text = goldSplit[0].ToString();

                _func = funcCall;
                transform.SetAsLastSibling();
            }
        }

        private void InternalOkOnClickHandler()
        {
            _noEnoughCrowns.gameObject.SetActive(false);
            if (_func != null)
                _func();
            _panelCrowns.SetActive(false);
            gameObject.SetActive(false);
        }

        private void InternalNoOnClickHandler()
        {
            _noEnoughCrowns.gameObject.SetActive(false);
            _func = null;
            _panelCrowns.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}