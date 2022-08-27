using System;

public class ExternalBoosterController
{
    private MasterSceneManager _master;
    private VirtualGridView _view;
    private Action<string> _boosterUsedEffects;
    public ExternalBoosterController(MasterSceneManager master, VirtualGridView view, Action<string> usesChanged)
    {
        _master = master;
        _view = view;
        _boosterUsedEffects = usesChanged;
    }

    public void ExecuteBooster(ExternalBoosterBase elementBehaviour)
    {
        elementBehaviour.Execute(_view, ConfirmExecution);
    }

    public void ConfirmExecution(string executedBoosterName, bool positive)
    {
        if (positive)
            _master.Inventory.RemoveElement(executedBoosterName, 1);
        else
            _master.Inventory.AddElement(executedBoosterName, 1);

        _boosterUsedEffects?.Invoke(executedBoosterName);

    }
}
