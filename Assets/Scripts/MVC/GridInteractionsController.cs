using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    private VirtualGridModel _Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [SerializeField] private BoostersLogic _boostersLogic;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;

    bool generationComplete;

    public void InteractionAtGridCell(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null)
    {
        if (_Model == null)
            _Model = Model;
        if (_View == null)
            _View = View;

        if (gridCell.blockInCell == null)
            return;

        StartCoroutine(InteractionCore(gridCell));

        _Model.matchList.Clear();
        _Model.boosterGridCell = null;
    }

    IEnumerator InteractionCore(GridCell gridCell)
    {
        //Check if interaction has result
        if (CheckInteractionWith(gridCell))
        {
            generationComplete = false;

            if(!gridCell.blockInCell.isTriggered)
                _turnManager.InteractionUsed();

            //Add Score
            AddScoreOnInteractionSucceed();

            //Destoy blocks
            DestroyBlocksOnActionSucceed();

            //Check if Add Booster
            CheckForBoosterSpawnOnInteractionSucceed(gridCell.blockAnchorCoords);

            yield return new WaitForSeconds(0.25f);

            //Collapse Board
            CheckCollapseBoard();

            //Regen Empty cells
            GenerateBlocksOnEmptyCells();

            //Use Hot Boosters
            StartCoroutine(CheckHotBoostersToInteract());
        }
    }

    bool CheckInteractionWith(GridCell gridCell)
    {
        bool boosterInteraction = false;

        if (!gridCell.blockInCell.isBooster)
        {
            CheckNeigtbourCoords(gridCell);
        }
        else
        {
            CheckActionOnBoosterBased(gridCell);
            boosterInteraction = true;
        }

        return _Model.matchList.Count >= 2 || boosterInteraction;
    }

    void CheckActionOnBoosterBased(GridCell gridCell)
    {
        _Model.boosterGridCell = gridCell;
        gridCell.blockInCell.booster.OnInteraction(gridCell.blockAnchorCoords, _Model);
    }

    void CheckNeigtbourCoords(GridCell gridCell)
    {
        Vector2[] neigtbourCoords = gridCell.blockAnchorCoords.GetCrossCoords();
        foreach (Vector2 coords in neigtbourCoords)
        {
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) && gridCell.blockInCell != null && objectiveCell.blockInCell != null)
            {
                TryGetMatch(gridCell, objectiveCell);
            }
        }
    }

    void TryGetMatch(GridCell cellA, GridCell cellB)
    {
        if (!_Model.matchList.Contains(cellA))
        {
            _Model.matchList.Add(cellA);
        }

        if (!_Model.matchList.Contains(cellB) && cellA.blockInCell.blockKind == cellB.blockInCell.blockKind)
        {
            CheckNeigtbourCoords(cellB);
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        foreach (var item in _Model.matchList)
        {
            if (!_Model.virtualGrid[item.blockAnchorCoords].blockInCell.isBooster)
            {
                _AddScoreEventBus.NotifyEvent(_Model.virtualGrid[item.blockAnchorCoords].blockInCell.blockKind, 1);
            }
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(_Model.boosterGridCell != null)
        {
            _Model.boosterGridCell.blockInCell.blockView.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _poolManager.DeSpawnBlockView(_Model.boosterGridCell.blockInCell.blockKind, _Model.boosterGridCell.blockInCell.blockView);
            });

            _Model.boosterGridCell.ResetGridCell();
        }

        foreach (var dynamicBlock in _Model.matchList)
        {
            if (dynamicBlock.blockInCell.isBooster)
            {
                dynamicBlock.blockInCell.isTriggered = true;
            }
            else
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.blockInCell.blockKind, dynamicBlock.blockInCell.blockView);
                dynamicBlock.ResetGridCell();
            }
        }
    }

    void CheckForBoosterSpawnOnInteractionSucceed(Vector2 coords)
    {
        if (_Model.boosterGridCell != null)
            return;

        if(_boostersLogic.CheckBaseBoosterSpawn(_Model.matchList.Count, out BaseBooster booster))
        {
            Transform newBooster = _poolManager.SpawnBlockView(booster.boosterKind, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            _View.FillGidCellWithBooster(coords, newBooster.gameObject, booster);
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

                element.Value.blockInCell.collapseSteps = cellCollapseSteps;
            }
        }

        CollapseBlocks();
    }

    void CollapseBlocks()
    {
        foreach (var gridCell in _Model.virtualGrid.Values)
        { 
            if(gridCell.hasBlock && gridCell.blockInCell.collapseSteps > 0)
            {
                Vector2 newCoords = gridCell.blockInCell.blockCoords + Vector2.down * gridCell.blockInCell.collapseSteps;
                DynamicBlock dynamicBlock = gridCell.blockInCell;

                dynamicBlock.blockCoords = newCoords;
                dynamicBlock.collapseSteps = 0;

                dynamicBlock.blockView.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                _Model.virtualGrid[newCoords].SetDynamicBlockOnCell(dynamicBlock);
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

        generationComplete = true;
    }

    IEnumerator CheckHotBoostersToInteract()
    {
        foreach (var item in _Model.virtualGrid)
        {
            if (item.Value.blockInCell != null && item.Value.blockInCell.isTriggered)
            {
                yield return new WaitUntil(()=> generationComplete);
                yield return new WaitForSeconds(0.5f);

                InteractionAtGridCell(item.Value);
            }
        }
    }
}
