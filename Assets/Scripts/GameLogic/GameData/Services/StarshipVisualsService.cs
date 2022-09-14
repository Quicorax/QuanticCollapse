using System.Collections.Generic;
using UnityEngine;

public class StarshipVisualsService : IService
{
    public Dictionary<string, DeSeializedStarshipColors> DeSerializedStarshipColors = new();
    public List<StarshipGeoModel> StarshipGeo = new();
    public void Initialize() 
    {
        LoadColorPacks();
        LoadStarshipGeos();
    }

    void LoadColorPacks()
    {
        StarshipColorsListModel ColorPacksModel = JsonUtility.FromJson<StarshipColorsListModel>(Resources.Load<TextAsset>("StarshipColors").text);

        foreach (var colorPack in ColorPacksModel.StarshipColors)
        {
            DeSerializedStarshipColors.Add(colorPack.SkinName, new(colorPack.SkinName, colorPack.SkinDescription,
                new Color().GenerateColorPackFromFormatedString(colorPack.ColorCode), colorPack.Price));
        }
    }
    void LoadStarshipGeos()
    {
        StarshipGeosListModel GeosModel = JsonUtility.FromJson<StarshipGeosListModel>(Resources.Load<TextAsset>("StarshipGeo").text);
        StarshipGeo = GeosModel.StarshipGeo;
    }
    public DeSeializedStarshipColors GetColorPackByName(string colorPackName)
    {
        DeSerializedStarshipColors.TryGetValue(colorPackName, out DeSeializedStarshipColors colorPack);
        return colorPack;
    }
    public void Clear() { }
}