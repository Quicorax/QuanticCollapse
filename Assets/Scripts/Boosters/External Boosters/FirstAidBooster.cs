using TMPro;

public class FirstAidBooster : ExternalBoosters
{
    public int lifeRegenAmount = 5; 
    VirtualGridView _View;

    public FirstAidBooster(VirtualGridView view, TMP_Text text)
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
        _View.ModifyPlayerLife(lifeRegenAmount);
        textRef.text = usesLeft.ToString();
    }
}
