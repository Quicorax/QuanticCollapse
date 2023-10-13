using System;

namespace QuanticCollapse
{
    public class ShopController
    {
        private GameProgressionService _gameProgress;

        public ShopController()
        {
            _gameProgress = ServiceLocator.GetService<GameProgressionService>();
        }

        public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent)
        {
            _gameProgress.UpdateElement(elementModel.Product.Id, elementModel.Product.Amount);
            _gameProgress.UpdateElement(elementModel.Price.Id, -elementModel.Price.Amount);

            purchaseEvent?.Invoke();
        }

        public void PurchaseElementWithIAP(IAPBundle elementModel, Action purchaseEvent)
        {
            _gameProgress.UpdateElement(elementModel.Product.Id, elementModel.Product.Amount);

            purchaseEvent?.Invoke();
        }

        public void PurchaseElementWithRewardedAdd(int amount, Action purchaseEvent)
        {
            _gameProgress.UpdateElement("AllianceCredits", amount);

            purchaseEvent?.Invoke();
        }
    }
}