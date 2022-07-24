using UnityEngine;

public class GridCell
{
    public bool hasBlock;

    public Vector2 blockAnchorCoords;

    public DynamicBlock blockInCell; 

    public GridCell(Vector2 anchorCoords)
    {
        hasBlock = false;
        blockAnchorCoords = anchorCoords;
    }

    public void SetDynamicBlockOnCellV2(DynamicBlock dynamicBlock) //For MVC test
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
