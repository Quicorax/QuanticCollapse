
[System.Serializable]
public class ShopElementModel
{
    public TradeableItem Product;
    public TradeableItem Price;

    public string ProductImage;
    public string ProductBody;

}

[System.Serializable]
public class TradeableItem
{
    public string Id;
    public int Amount;
}