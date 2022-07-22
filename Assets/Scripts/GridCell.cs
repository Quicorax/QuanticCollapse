public class GridCell
{
    public bool hasBlock;
    public DynamicBlock blockInCell;

    public DynamicBlockV2 blockInCellV2;

    public void SetDynamicBlockOnCell(DynamicBlock dynamicBlock)
    {
        hasBlock = true;
        blockInCell = dynamicBlock;
    }
    public void ResetGridCell()
    {
        hasBlock = false;
        blockInCell = null;
    }
}
