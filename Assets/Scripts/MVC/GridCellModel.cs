using UnityEngine;

public class GridCellController
{
    public Vector2Int AnchorCoords;
    public BlockModel BlockModel;

    public GridCellController(Vector2Int anchorCoords) 
    {
        AnchorCoords = anchorCoords; 
    }

    public void RemoveBlock() { BlockModel = null; }
    public void CallBoosterInteraction(Vector2Int coords, VirtualGridController controller) { BlockModel.Booster.OnInteraction(coords, controller); }

    public void SetDynamicBlockOnCell(BlockModel dynamicBlock) { BlockModel = dynamicBlock; }
    public void SetIsTriggered(bool triggered) { BlockModel.IsTriggered = triggered; }
    public void SetCollapseSteps(int steps) { BlockModel.CollapseSteps = steps; }
    public void SetCoords(Vector2Int coords) { BlockModel.Coords = coords; }

    public int CheckCollapseSteps() { return BlockModel.CollapseSteps; }
    public bool CheckHasBlock() { return BlockModel != null; }
    public bool CheckIsBooster() { return BlockModel.Booster != null; }
    public bool CheckIsTriggered() { return BlockModel.IsTriggered; }

    public ElementKind GetBlockKind() { return BlockModel.Kind; }
    public Vector2Int GetBlockCoords() { return BlockModel.Coords; }
    public GameObject GetViewReference() { return BlockModel.View; }
    public BlockModel GetModel() { return BlockModel; }
}