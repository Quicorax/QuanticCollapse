using UnityEngine;

namespace QuanticCollapse
{
    public class StarshipData : ScriptableObject
    {
        public bool IsPlayerShip;
        public int StarshipHealth = 20;

        [SerializeField] private StarshipModuleData[] starshipModules = new StarshipModuleData[4];

        public void CheckModuleActivation(int[] energyThresholdGrid)
        {
            for (int i = 0; i < starshipModules.Length; i++)
                starshipModules[i].CheckEnergy(energyThresholdGrid[i], IsPlayerShip);
        }
    }
}