using UnityEngine;

[System.Serializable]
public class DeSeializedStarshipColors 
{
    public string SkinName;
    public Color[] SkinColors;
    public int SkinPrice;
    public DeSeializedStarshipColors(string skinName, Color[] skinColors, int skinPrice)
    {
        SkinName = skinName;
        SkinColors = skinColors;
        SkinPrice = skinPrice;
    }
}
