using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStarshipData", menuName = "ScriptableObjects/StarshipData/Enemy")]
public class EnemyStarshipData : StarshipData
{
    private int[] energyGrid = new int[4]; //Fill based on AI difficulty

    private void Awake()
    {
        isPlayerShip = false;
    }
    public void Init()
    {
        CheckModuleActivation(energyGrid);
    }

}
