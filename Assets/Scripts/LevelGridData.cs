using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelGridData : ScriptableObject
{
    //Comunicate to InitialViewGridGeneration
    public Vector2 gridDimensions;
    public Texture2D gridInitialLayout;

    //Comunicate to StarShipManager
    public int enemyStarshipMaxLife;
    [Range(0, 20)] public int enemydifficulty;

    public List<Reward> possibleRewards = new();


    //Comunicate to SpaceShaderController
    public Color spaceColorA;
    public Color spaceColorB;
}
