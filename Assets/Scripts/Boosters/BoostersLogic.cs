using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boster
{
    public int interactions;
    public BaseBooster boosterLogic;
}

[CreateAssetMenu]
public class BoostersLogic : ScriptableObject
{
    public List<Boster> boostersList;

    public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster)
    {
        return GetBooster(blockCountOnAggrupation, out booster);
    }
    public bool GetBooster(int index, out BaseBooster booster)
    {
        for (int i = 0; i < boostersList.Count; i++)
        {         
            if (index >= boostersList[i].interactions)
            {
                booster = boostersList[i].boosterLogic;
                return true;
            }
        }

        booster = null;
        return false;
    }
}
