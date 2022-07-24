using UnityEngine;

public class GridCell
{
    public bool hasBlock;

    public Vector2 blockAnchorCoords;

    public DynamicBlock blockInCell;

    public DynamicBlockV2 blockInCellV2; //For MVC test

    public GridCell(Vector2 anchorCoords)
    {
        hasBlock = false;
        blockAnchorCoords = anchorCoords;
    }

    public void SetDynamicBlockOnCell(DynamicBlock dynamicBlock)
    {
        hasBlock = true;
        blockInCell = dynamicBlock;
    }
    public void SetDynamicBlockOnCellV2(DynamicBlockV2 dynamicBlock) //For MVC test
    {
        hasBlock = true;
        blockInCellV2 = dynamicBlock;
    }
    public void ResetGridCell()
    {
        hasBlock = false;
        blockInCell = null;

        blockInCellV2 = null;
    }
}
