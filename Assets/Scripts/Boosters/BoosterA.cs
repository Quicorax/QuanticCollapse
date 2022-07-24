using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterA", menuName = "ScriptableObjects/Boosters/A")]
public partial class BoosterA : BaseBooster 
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        bool vertical = Random.Range(0, 100) > 50;
        List<Vector2> coordsToCheck = new();

        if (vertical)
        {
            for (int i = 0; i < 7; i++)
            {
                coordsToCheck.Add(new Vector2(initialCoords.x, i));
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
               coordsToCheck.Add(new Vector2(i, initialCoords.y));
            }
        }
        coordsToCheck.Remove(initialCoords);

        _BoosterActionEventBus.NotifyEvent(coordsToCheck.ToArray());
    }
}
