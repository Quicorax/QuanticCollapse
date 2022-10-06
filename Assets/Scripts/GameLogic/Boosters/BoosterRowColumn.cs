using System.Collections.Generic;
using UnityEngine;

public partial class BoosterRowColumn : BaseBooster 
{
    public ElementKind BoosterKind => ElementKind.BoosterRowColumn;

    public void OnInteraction(Vector2Int initialCoords, GridController Controller)
    {
        bool vertical = Random.Range(0, 100) > 50;
        List<Vector2Int> coordsToCheck = new();

        if (vertical)
        {
            for (int i = 0; i < 7; i++)
                coordsToCheck.Add(new Vector2Int(initialCoords.x, i));
        }
        else
        {
            for (int i = 0; i < 9; i++)
               coordsToCheck.Add(new Vector2Int(i, initialCoords.y));
        }
        coordsToCheck.Remove(initialCoords);

        foreach (var coords in coordsToCheck)
        {
            if (Controller.Model.VirtualGrid.TryGetValue(coords, out GridCellController cell) && cell.CheckHasBlock())
                Controller.InteractionsController.MatchClosedList.Add(cell);
        }
    }
}
