public class AggrupateBlocksCommand : IGridCommand
{
        //TODO: Recursive-less alternative Open-Close list? O.o
    public void Do(VirtualGridModel Model)
    {
        Aggrupation _aggrupation = new();

        Model.aggrupationList.Add(_aggrupation);
    }
}

