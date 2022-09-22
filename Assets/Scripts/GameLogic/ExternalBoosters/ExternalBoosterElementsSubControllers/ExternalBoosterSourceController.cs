
using System;
using UnityEngine;

public class ExternalBoosterSourceController : ScriptableObject
{
    public ResourcesType boosterType;
    public virtual void Execute(GridController Controller, Action<ResourcesType, bool> ConfirmExecution)
    {
    }
}