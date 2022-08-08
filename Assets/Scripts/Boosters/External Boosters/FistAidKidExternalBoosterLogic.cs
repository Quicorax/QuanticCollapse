
public class FistAidKidExternalBoosterLogic : ExternalBoosters
{
    public int lifeRegenAmount = 5;

    public void Execute()
    {
        Used();
        View.ModifyPlayerLife(lifeRegenAmount);
    }
}
