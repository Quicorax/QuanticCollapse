using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelGridData : ScriptableObject
{
    public Vector2 gridDimensions;
    public Texture2D gridInitialLayout;
}
