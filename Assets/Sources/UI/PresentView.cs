using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Base;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.UI
{
    public sealed class PresentView : MonoBehaviour
    {
        [SerializeField] private Image _presentImage;
        [SerializeField] private Text _presentCostOpenNow;
        [SerializeField] private Button _presentButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _goldText;
        [SerializeField] private Text _silverText;
        [SerializeField] private Text _copperText;
        [SerializeField] private AudioSource _open;
        [SerializeField] private AudioSource _close;
        [SerializeField] private Sprite[] _presentsType;

        private INetworkProcessor _networkProcessor;
        private PresentContract _presentContract;

        public void Init(INetworkProcessor networkProcessor)
        {
            _presentButton.onClick.AddListener(InternalClickOpen);
            _closeButton.onClick.AddListener(InternalClickCloseWindow);
            _networkProcessor = networkProcessor;

            gameObject.SetActive(false);
        }

        public void SetupWindow(PresentModel presentModel)
        {
            _presentContract = presentModel._presentContract;
            _presentImage.sprite = InternalGetSpriteWithPresentType(presentModel._presentContract.PresentType);
            _open.Play();

            DateTime dateTime = new DateTime(_presentContract.Time);
            if (dateTime.CompareTo(DateTime.UtcNow) == -1)
            {
                _presentCostOpenNow.text = $"Open free!";

                _goldText.text = "0";
                _silverText.text = "0";
                _copperText.text = "0";
            }
            else
            {
                _presentCostOpenNow.text = "Spend crowns to open right now!";

                int totalSeconds = (int)dateTime.Subtract(DateTime.UtcNow).TotalSeconds;
                int costOpentGiftNow = unchecked(totalSeconds * _presentContract.CostOfOneSecondGift);

                int[] goldSplit = Parser.SplitIntToMoney(costOpentGiftNow);

                _goldText.text = goldSplit[2].ToString();
                _silverText.text = goldSplit[1].ToString();
                _copperText.text = goldSplit[0].ToString();
            }
        }

        private Sprite InternalGetSpriteWithPresentType(int type)
        {
            return _presentsType[type];
        }

        private void InternalClickOpen()
        {
            _networkProcessor.SendPacketAsync(RequestUsePresent.ToPacket(_presentContract.Slot));
            gameObject.SetActive(false);
        }

        private void InternalClickCloseWindow()
        {
            gameObject.SetActive(false);
            _close.Play();
        }
    }
}