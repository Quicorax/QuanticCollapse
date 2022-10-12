using System.Collections.Generic;
using UnityEngine;

public class BoosterBomb : BaseBooster
{
    private int _id;

    public BoosterBomb(int id)
    {
        _id = id;
    }

    public int BoosterKindId => _id;

    public void OnInteraction(Vector2Int initialCoords, GridModel Model)
    {
        List<Vector2Int> coordsToCheck = new();
        coordsToCheck.AddRange(initialCoords.GetSplashCoords());
        
        foreach (var coords in coordsToCheck)
        {
            if (Model.GridData.TryGetValue(coords, out GridCellController cell) && cell.CheckHasBlock())
                Model.MatchClosedList.Add(cell);
        }
    }
}
