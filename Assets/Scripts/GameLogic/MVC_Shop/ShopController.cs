using System;
using UnityEngine;
public class ShopController
{
    public ShopModel ShopModel;

    public ShopController()
    {
        LoadShopModelData();
    }

    void LoadShopModelData() => ShopModel = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopElements").text);
    public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent) 
    {
        ServiceLocator.GetService<GameProgressionService>().UpdateElement(elementModel.ProductName, elementModel.ProductAmount);
        ServiceLocator.GetService<GameProgressionService>().UpdateElement(elementModel.PriceKind, -elementModel.PriceAmount);

        purchaseEvent?.Invoke();
    }
}
