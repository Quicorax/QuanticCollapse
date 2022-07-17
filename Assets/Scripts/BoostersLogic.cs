using UnityEngine;

public class BoostersLogic : MonoBehaviour
{
    public BoostersData boostersData;
    public bool CheckBoosterSpawn(int blockCountOnAggrupation, out Booster booster)
    {
        for (int i = boostersData.boostersList.Count - 1; i >= 0; i--)
        {
            if (blockCountOnAggrupation >= boostersData.boostersList[i].interactiosToSpawn)
            {
                booster = boostersData.boostersList[i];
                return true;
            }
        }

        booster = null;
        return false;
    }
}
