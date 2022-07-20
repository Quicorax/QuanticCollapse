using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoosterC : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        //If exist on virtual Grid
        //If block in cell no null
            //Coose random kind
        //If block in cell kind is same as random choseen kind
            //Add Score
            //DeSpawn PoolObject
            //Call cell must be deleted

        List<Vector2> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                coordsToCheck.Add(new Vector2(x, y));
            }
        }

        EventManager.Instance.BoosterSameKindCheckInteraction(coordsToCheck.ToArray());

        //BoosterActionC actionB = new BoosterActionC();
        //actionB.Execute(coordsToCheck.ToArray());
    }
}
