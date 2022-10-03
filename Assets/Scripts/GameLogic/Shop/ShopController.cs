using System;
public class ShopController
{
    private GameProgressionService _gameProgression;

    public ShopController()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }
    public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent) 
    {
        _gameProgression.UpdateElement(elementModel.ProductId, elementModel.ProductAmount);
        _gameProgression.UpdateElement(elementModel.PriceId, -elementModel.PriceAmount);

        purchaseEvent?.Invoke();
    }
}
