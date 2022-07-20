using UnityEngine;

public class BoosterActionB : BoosterBaseAction
{
    public override void Execute(Vector2[] coords)
    {
        EventManager.Instance.BoosterInteraction(coords);
    }
}
