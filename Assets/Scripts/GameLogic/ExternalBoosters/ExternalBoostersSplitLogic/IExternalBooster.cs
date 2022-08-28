using System;

public interface IExternalBooster
{
    void Execute(VirtualGridView View, Action<string, bool> confirmExecution);
}
