
public class DamageEnemyExternalBoosterLogic : ExternalBoosters
{
    public int lifeSubstractionAmount = 2;

    public void Execute()
    {
        Used();
        View.ModifyEnemyLife(-lifeSubstractionAmount);
    }
}
