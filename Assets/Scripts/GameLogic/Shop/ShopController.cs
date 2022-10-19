using System;
namespace QuanticCollapse
{
    public class ShopController
    {
        private GameProgressionService _gameProgression;

        public ShopController()
        {
            _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        }
        public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent)
        {
            _gameProgression.UpdateElement(elementModel.Product.Id, elementModel.Product.Amount);
            _gameProgression.UpdateElement(elementModel.Price.Id, -elementModel.Price.Amount);

            purchaseEvent?.Invoke();
        }

        public void PurchaseElementWithIAP(IAPBundle elementModel, Action purchaseEvent)
        {
            _gameProgression.UpdateElement(elementModel.Product.Id, elementModel.Product.Amount);

            purchaseEvent?.Invoke();
        }

        public void PurchaseElementWithRewardedAdd(int amount, Action purchaseEvent)
        {
            _gameProgression.UpdateElement("AllianceCredits", amount);

            purchaseEvent?.Invoke();
        }
    }
}