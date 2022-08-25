using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterKindBased", menuName = "ScriptableObjects/Boosters/BoosterKindBased")]
public class BoosterKindBased : BaseBooster
{
    public override void OnInteraction(Vector2Int initialCoords, VirtualGridController Controller)
    {
        List<Vector2Int> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
                coordsToCheck.Add(new Vector2Int(x, y));
        }

        ElementKind kind = (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 3);

        foreach (var coords in coordsToCheck)
        {
            if (Controller.Model.virtualGrid.TryGetValue(coords, out GridCellController cell) && cell.CheckHasBlock() && cell.GetBlockKind() == kind)
                Controller.InteractionsController.MatchClosedList.Add(cell);
        }
    }
}
