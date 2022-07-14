using UnityEngine;

public interface IStarshipModule
{
    public void CheckEnergy(int incomeEnergy);
    public void ActivateModuleByEnergyPower(int energyPower);
}

public abstract class StarshipModuleData : ScriptableObject, IStarshipModule
{
    public ElementKind moduleKind;
    public int[] moduleEnergyPowerThresholds = new int[4];

    public void CheckEnergy(int incomeEnergy)
    {
        for (int thresholdPowerIndex = moduleEnergyPowerThresholds.Length - 1; thresholdPowerIndex >= 0; thresholdPowerIndex--)
        {
            if (incomeEnergy >= moduleEnergyPowerThresholds[thresholdPowerIndex])
            {
                ActivateModuleByEnergyPower(thresholdPowerIndex);
                break;
            }
        }
    }

    public virtual void ActivateModuleByEnergyPower(int energyPower)
    {
        Debug.Log("Module Activated");
    }
}
