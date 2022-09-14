using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterBomb", menuName = "ScriptableObjects/Boosters/BoosterBomb")]
public class BoosterBomb : BaseBooster
{
    public override void OnInteraction(Vector2Int initialCoords, GridController Controller)
    {
        List<Vector2Int> coordsToCheck = new();
        coordsToCheck.AddRange(initialCoords.GetSplashCoords());
        
        foreach (var coords in coordsToCheck)
        {
            if (Controller.Model.VirtualGrid.TryGetValue(coords, out GridCellController cell) && cell.CheckHasBlock())
                Controller.InteractionsController.MatchClosedList.Add(cell);
        }
    }
}
