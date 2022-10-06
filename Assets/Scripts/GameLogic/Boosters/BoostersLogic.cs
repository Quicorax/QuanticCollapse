using System.Collections.Generic;

[System.Serializable]
public struct Boster
{
    public int interactions;
    public BaseBooster boosterLogic;
}

public class BoostersLogic 
{
    public Dictionary<int, BaseBooster> boosters = new()
    {
        { 9, new BoosterKindBased()},
        { 7, new BoosterBomb()},
        { 5, new BoosterRowColumn()}
    };

    public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster) 
        => GetBooster(blockCountOnAggrupation, out booster);
    public bool GetBooster(int index, out BaseBooster booster)
    {
        foreach (var item in boosters)
        {
            if (index >= item.Key)
            {
                booster = item.Value;
                return true;
            }
        }
        
        booster = null;
        return false;
    }
}
