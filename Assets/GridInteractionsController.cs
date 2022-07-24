using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    private VirtualGridModel _Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private BoostersLogic _boostersLogic;


    private List<GridCell> _matchList = new();
    private GridCell _boosterGridCell;

    bool reGenerationComplete;

    public void InteractionAtGridCell(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null)
    {
        if (_Model == null)
            _Model = Model;
        if (_View == null)
            _View = View;

        if (gridCell.blockInCellV2 == null)
            return;

        //Check if interaction has result
        if (CheckInteractionWith(gridCell))
        {
            reGenerationComplete = false;

            _turnManager.InteractionUsed();

            //Add Score
            AddScoreOnInteractionSucceed();

            //Destoy blocks
            DestroyBlocksOnActionSucceed();

            //Check if Add Booster
            CheckForBoosterSpawnOnInteractionSucceed(gridCell.blockAnchorCoords);

            //Collapse Board
            CheckCollapseBoard();

            //Regen Empty cells
            GenerateBlocksOnEmptyCells();

            //Use Hot Boosters
            StartCoroutine(CheckHotBoostersToInteract());
        }

        _matchList.Clear();
        _boosterGridCell = null;
    }


    bool CheckInteractionWith(GridCell gridCell)
    {
        bool boosterInteraction = false;

        if (!gridCell.blockInCellV2.isBooster)
        {
            CheckNeigtbourCoords(gridCell);
        }
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterInteraction = true;
        }

        return _matchList.Count >= 2 || boosterInteraction;
    }

    void CheckActionOnBoosterBased(GridCell gridCell)
    {
        _boosterGridCell = gridCell;

        foreach (var affectedBoosterCoords in gridCell.blockInCellV2.booster.OnReturnCellsByInteraction(gridCell.blockAnchorCoords))
        {
            if(_Model.virtualGrid.TryGetValue(affectedBoosterCoords, out GridCell cell) && cell.hasBlock)
            {
                _matchList.Add(cell);
            }
        }
    }

    void CheckNeigtbourCoords(GridCell gridCell)
    {
        Vector2[] neigtbourCoords = gridCell.blockAnchorCoords.GetCrossCoords();
        foreach (Vector2 coords in neigtbourCoords)
        {
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) && gridCell.blockInCellV2 != null && objectiveCell.blockInCellV2 != null)
            {
                TryGetMatch(gridCell, objectiveCell);
            }
        }
    }

    void TryGetMatch(GridCell cellA, GridCell cellB)
    {
        if (!_matchList.Contains(cellA))
        {
            _matchList.Add(cellA);
        }

        if (!_matchList.Contains(cellB) && cellA.blockInCellV2.blockKind == cellB.blockInCellV2.blockKind)
        {
            CheckNeigtbourCoords(cellB);
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        foreach (var item in _matchList)
        {
            if(!_Model.virtualGrid[item.blockAnchorCoords].blockInCellV2.isBooster)
                _AddScoreEventBus.NotifyEvent(_Model.virtualGrid[item.blockAnchorCoords].blockInCellV2.blockKind, 1);
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(_boosterGridCell != null)
        {
            Destroy(_boosterGridCell.blockInCellV2.blockView);
            _boosterGridCell.ResetGridCell();
        }

        foreach (var dynamicBlock in _matchList)
        {
            if (!dynamicBlock.blockInCellV2.isBooster)
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.blockInCellV2.blockKind, dynamicBlock.blockInCellV2.blockView);
                dynamicBlock.ResetGridCell();
            }
            else
            {
                //Turn Booster hot
                dynamicBlock.blockInCellV2.isHot = true;
            }
        }
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2 coords)
    {
        if (_boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(_matchList.Count, out BaseBooster booster))
        {
            GameObject boosterObject = Instantiate(booster.boosterPrefab, coords, Quaternion.identity);
            _View.FillGidCellWithBooster(coords, boosterObject, booster);
        }
    }

    void CheckCollapseBoard()
    {
        foreach (var element in _Model.virtualGrid)
        {
            if (element.Value.hasBlock)
            {
               int cellCollapseSteps = 0;

                for (int y = (int)element.Key.y; y >= 0; y--)
                {
                    if (_Model.virtualGrid.TryGetValue(new Vector2(element.Key.x, y), out GridCell gridCell) && !gridCell.hasBlock)
                    {
                        cellCollapseSteps++;
                    }
                }

                element.Value.blockInCellV2.collapseSteps = cellCollapseSteps;
            }
        }

        CollapseBlocks();
    }

    void CollapseBlocks()
    {
        foreach (var gridCell in _Model.virtualGrid.Values)
        { 
            if(gridCell.hasBlock && gridCell.blockInCellV2.collapseSteps > 0)
            {
                Vector2 newCoords = gridCell.blockInCellV2.blockCoords + Vector2.down * gridCell.blockInCellV2.collapseSteps;
                DynamicBlockV2 dynamicBlock = gridCell.blockInCellV2;

                dynamicBlock.blockCoords = newCoords;
                dynamicBlock.collapseSteps = 0;

                dynamicBlock.blockView.transform.DOMoveY(newCoords.y, 0.3f);

                _Model.virtualGrid[newCoords].SetDynamicBlockOnCellV2(dynamicBlock);
                gridCell.ResetGridCell();
            }
        }
    }

    void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in _Model.virtualGrid)
        {
            if (!item.Value.hasBlock)
            {
                _View.FillGidCell(item.Key);
            }
        }

        reGenerationComplete = true;
    }

    IEnumerator CheckHotBoostersToInteract()
    {

        foreach (var item in _Model.virtualGrid)
        {
            if (item.Value.blockInCellV2.isHot)
            {
                yield return new WaitUntil( ()=> reGenerationComplete);
                yield return new WaitForSeconds(0.5f);

                InteractionAtGridCell(item.Value);
            }
        }
    }
}
