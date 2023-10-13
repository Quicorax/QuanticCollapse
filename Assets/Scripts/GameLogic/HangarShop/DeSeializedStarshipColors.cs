using UnityEngine;


namespace QuanticCollapse
{
    [System.Serializable]
    public class DeSeializedStarshipColors
    {
        public string SkinName;
        public string SkinDescription;
        public Color[] SkinColors;
        public int SkinPrice;

        public DeSeializedStarshipColors(string skinName, string skinDescription, Color[] skinColors, int skinPrice)
        {
            SkinName = skinName;
            SkinDescription = skinDescription;
            SkinColors = skinColors;
            SkinPrice = skinPrice;
        }
    }
}