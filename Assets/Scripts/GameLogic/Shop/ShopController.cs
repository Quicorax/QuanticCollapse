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
        _gameProgression.UpdateElement(GetResourceTypeFromString(elementModel.ProductName), elementModel.ProductAmount);
        _gameProgression.UpdateElement(GetResourceTypeFromString(elementModel.PriceKind), -elementModel.PriceAmount);

        purchaseEvent?.Invoke();
    }

    ResourcesType GetResourceTypeFromString(string name)
    {
        ResourcesType resource;

        switch (name)
        {
            default:
            case "FirstAidKit":
                resource = ResourcesType.FirstAidKit;
                break;
            case "EasyTrigger":
                resource = ResourcesType.EasyTrigger;
                break;
            case "DeAthomizer":
                resource = ResourcesType.DeAthomizer;
                break;
            case "AllianceCredits":
                resource = ResourcesType.AllianceCredits;
                break;
            case "Dilithium":
                resource = ResourcesType.Dilithium;
                break;
            case "Reputation":
                resource = ResourcesType.Reputation;
                break;
        }
        return resource;
    }
}
