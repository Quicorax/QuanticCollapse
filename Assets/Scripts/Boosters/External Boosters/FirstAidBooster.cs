using UnityEngine.UI;

public class FirstAidBooster : ExternalBoosters
{
    public int lifeRegenAmount = 5;

    public override void TryUse(VirtualGridView View)
    {
        if (!CheckUses(usesLeft))
            return;
        usesLeft--;

        ExecuteExternalBooster(View);
    }

    void ExecuteExternalBooster(VirtualGridView View)
    {
        View.ModifyPlayerLife(lifeRegenAmount);
    }
}
