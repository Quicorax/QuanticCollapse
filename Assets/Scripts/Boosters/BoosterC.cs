using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterC", menuName = "ScriptableObjects/Boosters/C")]
public class BoosterC : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        List<Vector2> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                coordsToCheck.Add(new Vector2(x, y));
            }
        }

        _BoosterActionEventBus.NotifyEvent(coordsToCheck.ToArray());
    }
}
