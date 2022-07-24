using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterB", menuName = "ScriptableObjects/Boosters/B")]
public class BoosterB : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        List<Vector2> coordsToCheck = new();
        coordsToCheck.AddRange(initialCoords.GetSplashCoords());
        
        _BoosterActionEventBus.NotifyEvent(coordsToCheck.ToArray());
    }

    public override List<Vector2> OnReturnCellsByInteraction(Vector2 initialCoords)
    {
        List<Vector2> coordsToCheck = new();

        coordsToCheck.AddRange(initialCoords.GetSplashCoords());

        return coordsToCheck;
    }
}
