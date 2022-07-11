using UnityEngine;

[System.Serializable]
public struct StarshipsModule
{
    public ElementKind moduleKind;
    public StarshipModuleData module;
}
[CreateAssetMenu(fileName = "StarshipData", menuName = "ScriptableObjects/StarshipData", order = 1)]
public class StarshipData : ScriptableObject
{
    public int maxInteractions;

    public StarshipsModule[] starshipModules = new StarshipsModule[4];

    public void ActivateModules(int[] energyGrid)
    {
        for (int i = 0; i < starshipModules.Length; i++)
            starshipModules[i].module.CheckEnergyThreshhold(energyGrid[i]);
    }
}
