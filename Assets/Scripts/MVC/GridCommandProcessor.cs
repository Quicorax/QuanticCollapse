using System.Collections.Generic;

public class GridCommandProcessor
{
    public VirtualGridModel Model;

    private List<IGridCommand> _commands = new();

    public GridCommandProcessor(VirtualGridModel model)
    {
        Model = model;
    }

    public void ProcessCommand(IGridCommand command)
    {
        command.Do(Model);
        _commands.Add(command);
    }
}
