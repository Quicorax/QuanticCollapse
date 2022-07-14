using UnityEngine;

[CreateAssetMenu(fileName = "AttackModuleData", menuName = "ScriptableObjects/StarshipModuleData/Attack")]
public class AttackModule : StarshipModuleData
{
    public override void ActivateModuleByEnergyPower(int energyPower)
    {
        Debug.Log("Used module ATTACK with power: " + energyPower);
    }
}
