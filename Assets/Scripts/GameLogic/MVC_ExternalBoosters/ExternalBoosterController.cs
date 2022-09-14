using System;

public class ExternalBoosterController
{
    private GameProgressionService _gameProgression;
    private GridController _gridController;
    private Action<string> _boosterUsedVisualEffects;
    public ExternalBoosterController(GameProgressionService gameProgression, GridController gridController, Action<string> boosterUsedVisualEffects)
    {
        _gameProgression = gameProgression;
        _gridController = gridController;
        _boosterUsedVisualEffects = boosterUsedVisualEffects;
    }

    public void ExecuteBooster(ExternalBoosterSourceController elementBehaviour)
    {
        if (_gameProgression.CheckElement(elementBehaviour.boosterName) > 0)
            elementBehaviour.Execute(_gridController, ConfirmExecution);
    }

    void ConfirmExecution(string executedBoosterName, bool positive)
    {
        if (positive)
            _gameProgression.UpdateElement(executedBoosterName, -1);
        else
            _gameProgression.UpdateElement(executedBoosterName, 1);

        _boosterUsedVisualEffects?.Invoke(executedBoosterName);
    }
}
