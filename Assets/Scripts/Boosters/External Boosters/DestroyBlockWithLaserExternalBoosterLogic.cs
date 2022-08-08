
public class DestroyBlockWithLaserExternalBoosterLogic : ExternalBoosters
{
    public  UserInputManager _inputManager;

    public void Execute()
    {
        Used();
        _inputManager.blockLaserBoosterInput = true;
    }
}
