using UnityEngine;

public class BoostersLogic : MonoBehaviour
{
    public BoostersData boostersData;
    public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster)
    {
        return boostersData.GetBooster(blockCountOnAggrupation, out booster);
    }
}
