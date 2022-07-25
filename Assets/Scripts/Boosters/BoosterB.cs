using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterB", menuName = "ScriptableObjects/Boosters/B")]
public class BoosterB : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords, VirtualGridModel Model)
    {
        List<Vector2> coordsToCheck = new();
        coordsToCheck.AddRange(initialCoords.GetSplashCoords());
        
        foreach (var coords in coordsToCheck)
        {
            if (Model.virtualGrid.TryGetValue(coords, out GridCell cell) && cell.hasBlock)
            {
                Model.matchList.Add(cell);
            }
        }
    }
}
