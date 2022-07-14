using UnityEngine;

[CreateAssetMenu(fileName = "IntelModuleData", menuName = "ScriptableObjects/StarshipModuleData/Intel")]
public class IntelModule : StarshipModuleData
{
    public override void ActivateModuleByEnergyPower(int energyPower)
    {
        Debug.Log("Used module: INTEL" + " With power: " + energyPower);
    }
}
