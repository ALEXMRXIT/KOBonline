using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.Models.Base;
using Assets.Sources.Models.Characters.Tools;
using Assets.Sources.Network.OutPacket;

namespace Assets.Sources.UI
{
    public sealed class PresentView : MonoBehaviour
    {
        [SerializeField] private Image _presentImage;
        [SerializeField] private Text _presentCostOpenNow;
        [SerializeField] private Button _presentButton;
        [SerializeField] private Text _goldText;
        [SerializeField] private Text _silverText;
        [SerializeField] private Text _copperText;
        [SerializeField] private Sprite[] _presentsType;

        private INetworkProcessor _networkProcessor;
        private PresentContract _presentContract;

        public void Init(INetworkProcessor networkProcessor)
        {
            _presentButton.onClick.AddListener(InternalClickOpen);
            _networkProcessor = networkProcessor;

            gameObject.SetActive(false);
        }

        public void SetupWindow(PresentModel presentModel)
        {
            _presentContract = presentModel._presentContract;
            _presentImage.sprite = InternalGetSpriteWithPresentType(presentModel._presentContract.PresentType);

            DateTime dateTime = new DateTime(_presentContract.Time);
            if (dateTime.CompareTo(DateTime.Now) == -1)
            {
                _presentCostOpenNow.text = $"Open free!";
            }
            else
            {
                _presentCostOpenNow.text = "Spend crowns to open right now!";

                int totalSeconds = (int)dateTime.Subtract(DateTime.Now).TotalSeconds;
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
    }
}