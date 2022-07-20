using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boster
{
    public int interactions;
    public BaseBooster boosterLogic;
}
public class BoostersLogic : MonoBehaviour //OPEN-CLOSE Priniple:
                                           //Open to extend: You can add as many boosters that inherit from BaseBooster as you want).
                                           //Close to modification: There is no need to modify the BooosterLogic or the BaseBooster to be able to add or activate the new boosts).
{
    public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster)
    {
        return GetBooster(blockCountOnAggrupation, out booster);
    }

    public List<Boster> boostrsList;

    public bool GetBooster(int index, out BaseBooster booster)
    {
        for (int i = 0; i < boostrsList.Count; i++)
        {         
            if (index >= boostrsList[i].interactions)
            {
                booster = boostrsList[i].boosterLogic;
                return true;
            }
        }

        booster = null;
        return false;
    }
}
