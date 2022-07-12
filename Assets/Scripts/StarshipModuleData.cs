using UnityEngine;

[CreateAssetMenu(fileName = "StarshipModuleData", menuName = "ScriptableObjects/StarshipModuleData", order = 2)]
public class StarshipModuleData : ScriptableObject
{
    public ElementKind moduleKind;
    public int[] modulePowerThresholds = new int[4];

    public void CheckEnergyThreshhold(int energy)
    {

        for (int i = 3; i >= 0; i--)
        {
            if (energy >= modulePowerThresholds[i])
            {
                ActivateMouleOnThreshold(i, energy);
                break;
            }
        }
    }

    void ActivateMouleOnThreshold(int power, int energy)
    {
        Debug.Log("Used module: " + moduleKind + " With power: " + power + "; Energy: " + energy);
    }
}
