using TMPro;

public class BlockLaserBooster : ExternalBoosters
{
    private InputManager _inputManager;

    public BlockLaserBooster(InputManager inputManager, TMP_Text text)
    {
        _inputManager = inputManager;
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
        _inputManager.blockLaserBoosterInput = true;
        textRef.text = usesLeft.ToString();
    }
}