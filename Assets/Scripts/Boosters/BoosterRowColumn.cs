using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterRowColumn", menuName = "ScriptableObjects/Boosters/BoosterRowColumn")]
public partial class BoosterRowColumn : BaseBooster 
{
    public override void OnInteraction(Vector2Int initialCoords, GridInteractionsController Controller)
    {
        bool vertical = Random.Range(0, 100) > 50;
        List<Vector2Int> coordsToCheck = new();

        if (vertical)
        {
            for (int i = 0; i < 7; i++)
            {
                coordsToCheck.Add(new Vector2Int(initialCoords.x, i));
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
               coordsToCheck.Add(new Vector2Int(i, initialCoords.y));
            }
        }
        coordsToCheck.Remove(initialCoords);

        foreach (var coords in coordsToCheck)
        {
            if (Controller.Model.virtualGrid.TryGetValue(coords, out GridCellController cell) && cell.CheckHasBlock())
            {
                Controller.matchList.Add(cell);
            }
        }
    }
}
