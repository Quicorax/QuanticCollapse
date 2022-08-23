using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelGridData : ScriptableObject
{
    public int reputationToAcces;

    [HideInInspector] public Vector2 gridDimensions;
    public Texture2D gridInitialLayout;

    public int enemyStarshipMaxLife;
    [Range(0, 20)] public int enemydifficulty;

    public List<Reward> possibleRewards = new();

    public Color spaceColorA;
    public Color spaceColorB;
}
