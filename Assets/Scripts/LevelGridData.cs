using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelGridData : ScriptableObject
{
    public Texture2D gridInitialLayout;
    public Vector2 gridDimensions;
}
