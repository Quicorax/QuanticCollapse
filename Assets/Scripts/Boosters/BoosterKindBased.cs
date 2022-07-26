using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterKindBased", menuName = "ScriptableObjects/Boosters/C")]
public class BoosterKindBased : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords, VirtualGridModel Model)
    {
        List<Vector2> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                coordsToCheck.Add(new Vector2(x, y));
            }
        }

        ElementKind kind = (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 1);

        foreach (var coords in coordsToCheck)
        {
            if (Model.virtualGrid.TryGetValue(coords, out GridCell cell) && cell.hasBlock && cell.blockInCell.blockKind == kind)
            {
                Model.matchList.Add(cell);
            }
        }
    }
}
