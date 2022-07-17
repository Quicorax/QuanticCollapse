using System.Collections.Generic;
using UnityEngine;

public class BaseBooster
{
    public int interactiosToSpawn;
    public string boosterName;
    public string boosterDescription;

    public GameObject boosterPrefab;

    public virtual void OnInteraction(out int boosterActionIndex)
    {
       
        boosterActionIndex = 0;
    }
}
[System.Serializable]
public class BoosterA : BaseBooster
{
    public override void OnInteraction(out int boosterActionIndex)
    {
        boosterActionIndex = 1;
    }
}
[System.Serializable]
public class BoosterB : BaseBooster
{
    public override void OnInteraction(out int boosterActionIndex)
    {
        boosterActionIndex = 2;
    }
}
[System.Serializable]
public class BoosterC : BaseBooster
{
    public override void OnInteraction(out int boosterActionIndex)
    {
        boosterActionIndex = 3;
    }
}


[CreateAssetMenu(fileName = "BoostersData", menuName = "ScriptableObjects/BoostersData", order = 3)]
public class BoostersData : ScriptableObject
{
    public BoosterA boosterA;
    public BoosterB boosterB;
    public BoosterC boosterC;

    public bool GetBooster(int index, out BaseBooster booster)
    {
        if(index >= boosterC.interactiosToSpawn)
        {
            booster = boosterC;
            return true;
        }
        else if (index >= boosterB.interactiosToSpawn)
        {
            booster = boosterB;
            return true;
        }
        else if (index >= boosterA.interactiosToSpawn)
        {
            booster = boosterA;
            return true;
        }

        booster = null;
        return false;
    }
}
