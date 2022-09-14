
using System;
using UnityEngine;

public class ExternalBoosterSourceController : ScriptableObject
{
    public string boosterName;
    public virtual void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
    }
}