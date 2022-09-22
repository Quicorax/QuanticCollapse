using System.Collections.Generic;

public class GridCommandProcessor
{
    public GridModel Model;

    private List<IGridCommand> _commands = new();

    public GridCommandProcessor(GridModel model)
    {
        Model = model;
    }

    public void ProcessCommand(IGridCommand command)
    {
        command.Do(Model);
        _commands.Add(command);
    }
}
