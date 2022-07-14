using UnityEngine;

[CreateAssetMenu(fileName = "SpeedModuleData", menuName = "ScriptableObjects/StarshipModuleData/Speed")]
public class SpeedModule : StarshipModuleData
{
    public override void ActivateModuleByEnergyPower(int energyPower)
    {
        Debug.Log("Used module: SPEED" + " With power: " + energyPower);
    }
}
