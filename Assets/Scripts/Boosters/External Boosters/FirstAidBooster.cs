public class FirstAidBooster : ExternalBoosters
{
    public int lifeRegenAmount = 5;
    VirtualGridView _View;

    public FirstAidBooster(VirtualGridView view)
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
        _View.ModifyPlayerLife(lifeRegenAmount);
    }
}
