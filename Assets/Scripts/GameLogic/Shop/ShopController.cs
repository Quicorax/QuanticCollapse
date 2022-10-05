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
        _gameProgression.UpdateElement(elementModel.Product.Id, elementModel.Product.Amount);
        _gameProgression.UpdateElement(elementModel.Price.Id, -elementModel.Price.Amount);

        purchaseEvent?.Invoke();
    }
}
