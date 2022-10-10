using System.Collections.Generic;
using UnityEngine;

public class BoosterBomb : BaseBooster
{
    public int BoosterKindId => 5;

    public void OnInteraction(Vector2Int initialCoords, GridController Controller)
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
