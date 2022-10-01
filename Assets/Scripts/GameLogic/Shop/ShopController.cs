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
            case "LOBBY_SHOP_FIRSTAIDKIT_HEADER":
                resource = ResourcesType.FirstAidKit;
                break;
            case "LOBBY_SHOP_EASYTRIGGER_HEADER":
                resource = ResourcesType.EasyTrigger;
                break;
            case "LOBBY_SHOP_DEATHOMIZER_HEADER":
                resource = ResourcesType.DeAthomizer;
                break;
            case "LOBBY_SHOP_DILITHIUM_HEADER":
                resource = ResourcesType.Dilithium;
                break;
            case "AllianceCredits":
                resource = ResourcesType.AllianceCredits;
                break;
            case "Reputation":
                resource = ResourcesType.Reputation;
                break;
        }
        return resource;
    }
}
