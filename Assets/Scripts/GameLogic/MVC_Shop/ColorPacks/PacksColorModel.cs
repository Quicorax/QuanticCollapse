using System.Collections.Generic;
using UnityEngine;

public class PacksColorModel
{
    public List<ColorPackModel> Colors = new();
}


public class PacksColorController
{
    public PacksColorModel ColorPackModel;

    public PacksColorController()
    {
        LoadColorPackModelData();
    }

    public void LoadColorPackModelData()
    {
        ColorPackModel = JsonUtility.FromJson<PacksColorModel>(Resources.Load<TextAsset>("Colors").text);
    }
}
