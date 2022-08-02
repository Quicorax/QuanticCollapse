using TMPro;

public class DamageEnemyBooster : ExternalBoosters
{
    public int lifeSubstractionAmount = 2;
    VirtualGridView _View;

    public DamageEnemyBooster(VirtualGridView view, TMP_Text text)
    {
        _View = view;
        textRef = text;

        textRef.text = usesLeft.ToString();
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
        textRef.text = usesLeft.ToString();
    }
}
