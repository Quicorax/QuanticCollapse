using System;

public class ExternalBoosterController
{
    private MasterSceneManager _master;
    private VirtualGridController _gridController;
    private Action<string> _boosterUsedVisualEffects;
    public ExternalBoosterController(MasterSceneManager master, VirtualGridController gridController, Action<string> boosterUsedVisualEffects)
    {
        _master = master;
        _gridController = gridController;
        _boosterUsedVisualEffects = boosterUsedVisualEffects;
    }

    public void ExecuteBooster(ExternalBoosterSourceController elementBehaviour)
    {
        if (_master.Inventory.CheckElementAmount(elementBehaviour.boosterName) > 0)
            elementBehaviour.Execute(_gridController, ConfirmExecution);
    }

    void ConfirmExecution(string executedBoosterName, bool positive)
    {
        if (positive)
            _master.Inventory.RemoveElement(executedBoosterName, 1);
        else
            _master.Inventory.AddElement(executedBoosterName, 1);

        _boosterUsedVisualEffects?.Invoke(executedBoosterName);
    }
}
