using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAggrupationCommand: IGidCommand
{
    public void Do(VirtualGridManager model)
    {
        
    }
}

public class ActionCommands : MonoBehaviour
{
    public VirtualGridManager VirtualGridManager;

    private List<IGidCommand> _commands = new();

    public void ProcessCommand(IGidCommand command)
    {
        command.Do(VirtualGridManager);

        _commands.Add(command);
    }
}
