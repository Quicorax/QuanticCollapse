using UnityEngine;

public class UserInteractionCommant : IGridCommand
{
    private Vector2 _inputCoords;
    public UserInteractionCommant(Vector2 coords) => _inputCoords = coords;

    public void Do(VirtualGridModel Model)
    {
        Debug.Log("Input at: " + _inputCoords);

        foreach (Aggrupation aggrupation in Model.aggrupationList)
        {
            if (aggrupation.memberCoords.Contains(_inputCoords))
            {
                Debug.Log("Pointed at aggrupation: " + aggrupation.index);
            }
        }
    }
}


