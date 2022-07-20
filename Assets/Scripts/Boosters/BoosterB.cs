using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoosterB : BaseBooster
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        //If exist on virtual Grid
        //If block in cell no null
        //If block in cell no booster
        //If block in cell coords no initialcoords
        //If block in cell coords are part os SlashCoords
            //Add Score
            //Call upper cells must collapse
            //DeSpawn PoolObject
            //Call cell must be deleted


        List<Vector2> coordsToCheck = new();
        coordsToCheck.AddRange(initialCoords.GetSplashCoords());

        EventManager.Instance.BoosterInteraction(coordsToCheck.ToArray());

        //BoosterActionB actionB = new BoosterActionB();
        //actionB.Execute(coordsToCheck.ToArray());
    }
}
