using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Aggrupation
{
    public int index;
    public List<Vector2> memberCoords;
}

[RequireComponent(typeof(VirtualGridManager))]
public class AggrupationManager : MonoBehaviour
{
    [SerializeField]
    private GenericEventBus _ImpossibleGridEventBus;

    public List<Aggrupation> aggrupationList = new();
    Aggrupation emptyAggrupation = new();

    public int aggrupationIndexAmount;

    public void SetBlockCellOnAggrupation(int index, Vector2 coords)
    {
        bool aggrupationRegistered = false;
        List<Vector2> existingAggrupationList = null;

        if (GetAggrupationByItsIndex(index, out Aggrupation aggrupationContainer))
        {
            aggrupationRegistered = true;
            existingAggrupationList = aggrupationContainer.memberCoords;
        }

        if (aggrupationRegistered && !existingAggrupationList.Contains(coords))
        {
            existingAggrupationList.Add(coords);
        }
        else if (!aggrupationRegistered)
        {
            List<Vector2> newAggrupationList = new();
            newAggrupationList.Add(coords);

            Aggrupation newAggrupation = new Aggrupation();
            newAggrupation.index = index;
            newAggrupation.memberCoords = newAggrupationList;

            aggrupationList.Add(newAggrupation);
        }
    }

    public void RemoveElementFromAggrupation(DynamicBlock block)
    {
        if (GetAggrupationByItsIndex(block.aggrupationIndex, out Aggrupation aggrupation))
        {
            aggrupation.memberCoords.Remove(block.actualCoords);

            if (aggrupation.memberCoords.Count == 0)
                DeleteAggrupation(aggrupation);
        }
    }

    public void TryDeleteAggrupationEntry(int index)
    {
        if (GetAggrupationByItsIndex(index, out Aggrupation aggrupation))
        {
            DeleteAggrupation(aggrupation);

            if (aggrupationList.Count == 0)
            {
                _ImpossibleGridEventBus.NotifyEvent();
            }
        }
    }

    void DeleteAggrupation(Aggrupation aggrupation)
    { 
        aggrupationList.Remove(aggrupation);
    }


    public bool GetAggrupationByItsIndex(int index, out Aggrupation aggrupation)
    {
        foreach (var item in aggrupationList)
        {
            if (item.index == index)
            {
                aggrupation = item;
                return true;
            }
        }
        aggrupation = emptyAggrupation;
        return false;
    }
}
