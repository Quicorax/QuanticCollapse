using System;

public class ExternalBoosterController
{
    private MasterSceneManager _master;
    private VirtualGridView _view;
    private Action<string> _boosterUsesChanged;
    public ExternalBoosterController(MasterSceneManager master, VirtualGridView view, Action<string> usesChanged)
    {
        _master = master;
        _view = view;
        _boosterUsesChanged = usesChanged;
    }

    public void ExecuteBooster(ExternalBoosterBase elementBehaviour)
    {
        //TODO: Visuals of the execution

        elementBehaviour.Execute(_view, ConfirmExecution);
    }

    public void ConfirmExecution(string executedBoosterName, bool positive)
    {
        if (positive)
            _master.Inventory.RemoveElement(executedBoosterName, 1);
        else
            _master.Inventory.AddElement(executedBoosterName, 1);

        _boosterUsesChanged?.Invoke(executedBoosterName);

    }
}
