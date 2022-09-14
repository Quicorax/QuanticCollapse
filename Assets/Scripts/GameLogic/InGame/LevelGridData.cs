using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelGridData : ScriptableObject
{
    public int ReputationToAcces;

    public Texture2D GridInitialLayout;

    public int EnemyStarshipMaxLife;
    [Range(0, 20)] public int Enemydifficulty;

    public List<LevelRewards> PossibleRewards = new();

    public Color SpaceGeneralColor;
}
