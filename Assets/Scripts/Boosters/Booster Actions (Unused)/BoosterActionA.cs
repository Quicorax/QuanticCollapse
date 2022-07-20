using UnityEngine;

public class BoosterActionA : BoosterBaseAction
{
    public override void Execute(Vector2[] coords)
    {
        EventManager.Instance.BoosterInteraction(coords);
    }
}
