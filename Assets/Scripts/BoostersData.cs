using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Booster
{
    public int interactiosToSpawn;
    public string boosterName;
    public string boosterDescription;

    public GameObject boosterPrefab;

}
[CreateAssetMenu(fileName = "BoostersData", menuName = "ScriptableObjects/BoostersData", order = 3)]
public class BoostersData : ScriptableObject
{
    public List<Booster> boostersList = new();
}
