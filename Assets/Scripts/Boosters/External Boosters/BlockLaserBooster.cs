using TMPro;

public class BlockLaserBooster : ExternalBoosters
{
    private InputManager _inputManager;
    VirtualGridView _View;

    public BlockLaserBooster(VirtualGridView view, InputManager inputManager, TMP_Text text)
    {
        _View = view;
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