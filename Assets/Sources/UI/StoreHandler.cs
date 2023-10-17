using UnityEngine;
using UnityEngine.Purchasing;
using Assets.Sources.Network;
using Assets.Sources.UI.Models;
using Assets.Sources.Interfaces;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.MechanicUI.Models;

namespace Assets.Sources.UI
{
    public sealed class StoreHandler : MonoBehaviour
    {
        [SerializeField] private PurchaseWindow _purchaseWindow;
        [SerializeField] private AudioSource _open;
        [SerializeField] private AudioSource _close;

        public static StoreHandler Instance;

        private INetworkProcessor _networkProcessor;
        private bool _statusWindow = false;

        private void Awake()
        {
            _networkProcessor = ClientProcessor.Instance;
            Instance = this;
            _purchaseWindow.InitButton();

            gameObject.SetActive(false);
            _purchaseWindow.gameObject.SetActive(false);
        }

        public void ActiveStorePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);

            if (!_statusWindow)
                _close.Play();
            else
                _open.Play();
        }

        public void OnPurchaseCompleted(Product product)
        {
            _networkProcessor.SendPacketAsync(BuyItemFromStore.ToPacket(product.definition.id));
        }

        public void OnInternalPurchaseComplete(string id)
        {
            _purchaseWindow.SetItem(id, InternalOnPurchaseComplete);
            _purchaseWindow.Show();
        }

        private void InternalOnPurchaseComplete(string id)
        {
            _networkProcessor.SendPacketAsync(BuyItemFromStore.ToPacket(id));
        }

        public void OnEnoughtSoulCrowns()
        {
            _purchaseWindow.ShowNoEnoughCrowns();
        }
    }
}