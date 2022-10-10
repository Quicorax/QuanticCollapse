using UnityEngine;

public class GridCellController //Remove this
{
    public Vector2Int AnchorCoords;
    public CellBlockModel BlockModel;

    public GridCellController(Vector2Int anchorCoords) 
    {
        AnchorCoords = anchorCoords; 
    }

    public void RemoveBlock() => BlockModel = null; 
    public void CallBoosterInteraction(Vector2Int coords, GridController controller) => BlockModel.Booster.OnInteraction(coords, controller);

    public void SetDynamicBlockOnCell(CellBlockModel dynamicBlock) => BlockModel = dynamicBlock;
    public void SetIsTriggered(bool triggered) => BlockModel.IsTriggered = triggered;
    public void SetCollapseSteps(int steps) => BlockModel.CollapseSteps = steps;
    public void SetCoords(Vector2Int coords) => BlockModel.Coords = coords;

    public int CheckCollapseSteps() => BlockModel.CollapseSteps;
    public bool CheckHasBlock() => BlockModel != null;
    public bool CheckIsBooster() => BlockModel.Booster != null;
    public bool CheckIsTriggered() => BlockModel.IsTriggered;

    public int GetBlockId() => BlockModel.Id;
    public Vector2Int GetBlockCoords() => BlockModel.Coords;
    public CellBlockModel GetModel() => BlockModel;
    public GameObject GetViewReference()=> BlockModel.RefObject;
}