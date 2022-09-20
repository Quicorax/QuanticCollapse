using System;
using UnityEngine;
public class ShopController
{
    public ShopModel ShopModel;
    private GameProgressionService _gameProgression;
    public ShopController()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        LoadShopModelData();
    }

    void LoadShopModelData() => ShopModel = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>(Constants.ShopElements).text);
    public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent) 
    {
        _gameProgression.UpdateElement(elementModel.ProductName, elementModel.ProductAmount);
        _gameProgression.UpdateElement(elementModel.PriceKind, -elementModel.PriceAmount);

        purchaseEvent?.Invoke();
    }
}
