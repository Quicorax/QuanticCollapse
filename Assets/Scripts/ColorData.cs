using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/Debug/ColorData", order = 4)]
public class ColorData : ScriptableObject
{
    public Color[] colors = new Color[4];

}
