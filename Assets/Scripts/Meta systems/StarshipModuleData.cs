using UnityEngine;

[CreateAssetMenu(fileName = "StarshipModule", menuName = "ScriptableObjects/StarshipData/StarshipModule")]
public class StarshipModuleData : ScriptableObject
{
    [SerializeField]
    private StarshipModuleActivationEventBus _StarshipModuleActivationEventBus;

    public ElementKind moduleKind;
    public int[] moduleEnergyPowerThresholds = new int[4];

    public void CheckEnergy(int incomeEnergy, bool playerShip)
    {
        for (int thresholdPowerIndex = moduleEnergyPowerThresholds.Length - 1; thresholdPowerIndex >= 0; thresholdPowerIndex--)
        {
            if (incomeEnergy >= moduleEnergyPowerThresholds[thresholdPowerIndex])
            {
                ActivateModuleByEnergyPower(thresholdPowerIndex, playerShip);
                break;
            }
        }
    }

    public void ActivateModuleByEnergyPower(int energyPower, bool playerShip)
    {
        _StarshipModuleActivationEventBus.NotifyEvent(playerShip, moduleKind, energyPower);
    }
}
