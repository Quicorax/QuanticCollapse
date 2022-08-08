
public class DestroyBlockWithLaserExternalBoosterLogic : ExternalBoosters
{
    public  UserInputManager _inputManager;
    public void Execute()
    {
        if (_inputManager.blockLaserBoosterInput)
        {
            usesLeft++;
            textRef.text = usesLeft.ToString();
            _inputManager.blockLaserBoosterInput = false;
            return;
        }

        Used();
        _inputManager.blockLaserBoosterInput = true;
    }
}
