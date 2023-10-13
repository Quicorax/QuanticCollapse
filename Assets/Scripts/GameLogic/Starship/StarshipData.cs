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
            for (var index = 0; index < starshipModules.Length; index++)
            {
                starshipModules[index].CheckEnergy(energyThresholdGrid[index], IsPlayerShip);
            }
        }
    }
}