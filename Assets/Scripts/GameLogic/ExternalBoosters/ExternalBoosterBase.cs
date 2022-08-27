
using System;
using UnityEngine;

public class ExternalBoosterBase : ScriptableObject
{
    public string boosterName;
    public virtual void Execute(VirtualGridView View, Action<string, bool> ConfirmExecution)
    {
    }
}