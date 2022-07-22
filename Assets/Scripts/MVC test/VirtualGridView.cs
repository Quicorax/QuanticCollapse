using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGridView : MonoBehaviour
{
    [SerializeField]
    private TapOnCoordsEventBus _TapOnCoordsEventBus;

    private VirtualGridController Controller = new VirtualGridController();

    private void Awake()
    {
        _TapOnCoordsEventBus.Event += ListenInput;
    }
    private void OnDestroy()
    {
        _TapOnCoordsEventBus.Event -= ListenInput;
    }

    public void ListenInput(Vector2 inputCoords, bool isExternalBooster) //isExternalBooster must not be used (is here to be able to test the input system event bus already existing)
    {
        Controller.ProcessCommand(new UserInteractionCommant(inputCoords));
    }

    #region Generation
    public void GenerateGidCell(Vector2 cellCoords, GridCell cell)
    {
        Controller.ProcessCommand(new GenerateGridCellCommand(cellCoords, cell));
    }

    public void FillGidCell(Vector2 coordsToFill)
    {
        Controller.ProcessCommand(new FillGridCellCommand(coordsToFill));
    }
    public void AggrupateBlocks()
    {
        Controller.ProcessCommand(new AggrupateBlocksCommand());
    }
    #endregion
}
