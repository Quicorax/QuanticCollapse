using UnityEngine;
public class ShopController
{
    public ShopModel Model;
    private MasterSceneManager _master;

    public ShopController(MasterSceneManager master)
    {
        _master = master;
    }

    public void Init() { LoadModelData(); }
    void LoadModelData()
    {
        Model = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopElements").text);
    }
    public void PurchaseElement(ShopElementModel elementModel, GenericEventBus purchaseEvent) 
    {
        _master.Inventory.AddElement(elementModel.ProductKind, elementModel.ProductAmount);
        _master.Inventory.RemoveElement(elementModel.PriceKind, elementModel.PriceAmount);

        purchaseEvent.NotifyEvent();

        _master.SaveAll();
    }
}
