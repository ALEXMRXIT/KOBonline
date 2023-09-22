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

        public static StoreHandler Instance;

        private INetworkProcessor _networkProcessor;

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
            gameObject.SetActive(true);
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