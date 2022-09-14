using UnityEngine;

public class HangarShopController
{
    public HangarStarshipGeoShopModel HangarStarshipGeoShopModel;

    public HangarShopController()
    {
        LoadStarshipModelsList();
    }

    void LoadStarshipModelsList()
    {
        HangarStarshipGeoShopModel = JsonUtility.FromJson<HangarStarshipGeoShopModel>(Resources.Load<TextAsset>("StarshipGeo").text);
    }
}
