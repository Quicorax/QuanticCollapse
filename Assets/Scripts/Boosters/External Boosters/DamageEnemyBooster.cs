public class DamageEnemyBooster : ExternalBoosters
{
    public int lifeSubstractionAmount = 2;
    public override void TryUse(VirtualGridView View)
    {
        if (!CheckUses(usesLeft))
            return;
        usesLeft--;

        ExecuteExternalBooster(View);
    }

    void ExecuteExternalBooster(VirtualGridView View)
    {
        View.ModifyEnemyLife(-lifeSubstractionAmount);
    }
}