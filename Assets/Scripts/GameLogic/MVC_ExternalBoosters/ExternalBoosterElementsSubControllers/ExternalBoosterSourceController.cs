
using System;
using UnityEngine;

public class ExternalBoosterSourceController : ScriptableObject
{
    public string boosterName;
    public virtual void Execute(VirtualGridController Controller, Action<string, bool> ConfirmExecution)
    {
    }
}