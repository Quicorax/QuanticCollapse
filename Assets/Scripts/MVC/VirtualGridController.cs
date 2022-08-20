using System.Collections.Generic;

public class VirtualGridController 
{
    public VirtualGridModel Model = new(); 
    private List<IGridCommand> _commands = new();
    public void ProcessCommand(IGridCommand command)
    {
        command.Do(Model);
        _commands.Add(command);
    }
}
