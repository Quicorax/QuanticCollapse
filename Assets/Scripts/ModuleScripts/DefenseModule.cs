using UnityEngine;

[CreateAssetMenu(fileName = "DefenseModuleData", menuName = "ScriptableObjects/StarshipModuleData/Defense")]
public class DefenseModule : StarshipModuleData
{
    public override void ActivateModuleByEnergyPower(int energyPower)
    {
        Debug.Log("Used module: DEFENSE" + " With power: " + energyPower);
    }
}
