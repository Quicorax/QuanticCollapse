using System;
using UnityEngine;
public class ShopController
{
    public ShopModel ShopModel;
    private MasterSceneManager _master;

    public ShopController(MasterSceneManager master)
    {
        _master = master;
        LoadShopModelData();
    }

    void LoadShopModelData() => ShopModel = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopElements").text);
    public void PurchaseElement(ShopElementModel elementModel, Action purchaseEvent) 
    {
        _master.Inventory.AddElement(elementModel.ProductName, elementModel.ProductAmount);
        _master.Inventory.RemoveElement(elementModel.PriceKind, elementModel.PriceAmount);

        purchaseEvent?.Invoke();

        _master.SaveAll();
    }
}
