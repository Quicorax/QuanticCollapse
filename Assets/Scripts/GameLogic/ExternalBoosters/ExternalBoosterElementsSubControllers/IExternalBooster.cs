using System;

public interface IExternalBooster
{
    public ResourcesType BoosterType { get; }
    public void Execute(GridController Controller, Action<ResourcesType, bool> ConfirmExecution);
}