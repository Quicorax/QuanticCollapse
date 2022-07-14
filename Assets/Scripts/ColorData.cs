using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/Debug/ColorData", order = 4)]
public class ColorData : ScriptableObject
{
    public Color[] colors = new Color[4];

    public bool CheckColorCoincidence(Color incomeColor, out int colorIndex)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (incomeColor == colors[i])
            {
                colorIndex = i;
                return true;
            }
        }
        colorIndex = 0;
        return false;
    }
}
