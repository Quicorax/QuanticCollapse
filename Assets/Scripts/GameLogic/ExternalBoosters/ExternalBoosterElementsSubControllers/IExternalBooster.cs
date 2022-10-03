using System;

public interface IExternalBooster
{
    public string BoosterId { get; }
    public void Execute(GridController Controller, Action<string, bool> ConfirmExecution);
}