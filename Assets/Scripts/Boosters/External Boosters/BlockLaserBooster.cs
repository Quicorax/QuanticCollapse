public class BlockLaserBooster : ExternalBoosters
{
    private InputManager _inputManager;

    public BlockLaserBooster(InputManager inputManager)
    {
        _inputManager = inputManager;
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
        _inputManager.blockLaserBoosterInput = true;
    }
}