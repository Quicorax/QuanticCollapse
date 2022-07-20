using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public partial class BoosterA : BaseBooster 
{
    public override void OnInteraction(Vector2 initialCoords)
    {
        //If exist on virtual Grid
        //If block in cell no null
        //If block in cell no booster
        //If block in cell coords no initialcoords
        //If block in cell coords x (vertial) or y (horizontal) same as initalcoods x or y
            //Add Score
            //Call upper cells must collapse
            //DeSpawn PoolObjec
            //Call cell must be deleted

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

        EventManager.Instance.BoosterInteraction(coordsToCheck.ToArray());
        
        //BoosterActionA actionA = new BoosterActionA();
        //actionA.Execute(coordsToCheck.ToArray());
    }
}
