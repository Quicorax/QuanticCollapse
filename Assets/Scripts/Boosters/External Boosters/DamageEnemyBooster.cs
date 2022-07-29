public class DamageEnemyBooster : ExternalBoosters
{
    public int lifeSubstractionAmount = 2;
    VirtualGridView _View;
    public DamageEnemyBooster(VirtualGridView view)
    {
        _View = view;
    }

    public override void TryUse()
    {
        if (!CheckUses(usesLeft))
            return;
        usesLeft--;

        ExecuteExternalBooster();
    }

    void ExecuteExternalBooster()
    {
        _View.ModifyEnemyLife(-lifeSubstractionAmount);
    }
}
