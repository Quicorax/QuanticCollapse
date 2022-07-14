using UnityEngine;

public class StarshipData : ScriptableObject
{
    public bool isPlayerShip;

    public StarshipModuleData[] starshipModules = new StarshipModuleData[4];

    public void CheckModuleActivation(int[] energyThresholdGrid)
    {
        for (int i = 0; i < starshipModules.Length; i++)
        {
            starshipModules[i].CheckEnergy(energyThresholdGrid[i], isPlayerShip);
        }
    }
}
