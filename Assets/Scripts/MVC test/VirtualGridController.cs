using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGridController 
{
    public VirtualGridModel Model = new VirtualGridModel();

    private List<IGridCommand> _commands = new();

    public void ProcessCommand(IGridCommand command)
    {
        command.Do(Model);
        _commands.Add(command);
    }
}
