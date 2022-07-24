using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractionsController : MonoBehaviour
{
    private VirtualGridView _View;
    private VirtualGridModel _Model;

    [SerializeField] private AddScoreEventBus _AddScoreEventBus;
    [SerializeField] private BoosterActionEventBus _BoosterActionEventBus;
    [SerializeField] private BoosterActionEventBus _BoosterActionKindBasedEventBus;

    [SerializeField] private BoostersLogic _boostersLogic;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private TurnManager _turnManager;


    private List<GridCell> _matchList = new();
    private GridCell _boosterGridCell;

    bool reGenerationComplete;

    private void Awake()
    {
        _BoosterActionEventBus.Event += BoosterAction;
        _BoosterActionKindBasedEventBus.Event += BoosterActionKindBased;
    }

    private void OnDestroy()
    {
        _BoosterActionEventBus.Event -= BoosterAction;
        _BoosterActionKindBasedEventBus.Event -= BoosterActionKindBased;
    }

    public void InteractionAtGridCell(GridCell gridCell, VirtualGridView View = null, VirtualGridModel Model = null)
    {
        if (_Model == null)
            _Model = Model;
        if (_View == null)
            _View = View;

        if (gridCell.blockInCell == null)
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

        if (!gridCell.blockInCell.isBooster)
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
        gridCell.blockInCell.booster.OnInteraction(gridCell.blockAnchorCoords);
    }

    void BoosterAction(Vector2[] affectedBoosterCoords)
    {
        foreach (var coords in affectedBoosterCoords)
        {
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell cell) && cell.hasBlock)
            {
                _matchList.Add(cell);
            }
        }
    }

    void BoosterActionKindBased(Vector2[] affectedBoosterCoords)
    {
        ElementKind kind = (ElementKind)Random.Range(0, System.Enum.GetValues(typeof(ElementKind)).Length - 1);

        foreach (var coords in affectedBoosterCoords)
        {
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell cell) && cell.hasBlock && cell.blockInCell.blockKind == kind)
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
            if (_Model.virtualGrid.TryGetValue(coords, out GridCell objectiveCell) && gridCell.blockInCell != null && objectiveCell.blockInCell != null)
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

        if (!_matchList.Contains(cellB) && cellA.blockInCell.blockKind == cellB.blockInCell.blockKind)
        {
            CheckNeigtbourCoords(cellB);
        }
    }

    void AddScoreOnInteractionSucceed()
    {
        foreach (var item in _matchList)
        {
            if(!_Model.virtualGrid[item.blockAnchorCoords].blockInCell.isBooster)
                _AddScoreEventBus.NotifyEvent(_Model.virtualGrid[item.blockAnchorCoords].blockInCell.blockKind, 1);
        }
    }

    void DestroyBlocksOnActionSucceed()
    {
        if(_boosterGridCell != null)
        {
            Destroy(_boosterGridCell.blockInCell.blockView);
            _boosterGridCell.ResetGridCell();
        }

        foreach (var dynamicBlock in _matchList)
        {
            if (!dynamicBlock.blockInCell.isBooster)
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.blockInCell.blockKind, dynamicBlock.blockInCell.blockView);
                dynamicBlock.ResetGridCell();
            }
            else
            {
                //Turn Booster hot
                dynamicBlock.blockInCell.isHot = true;
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
            if (item.Value.blockInCell.isHot)
            {
                yield return new WaitUntil( ()=> reGenerationComplete);
                yield return new WaitForSeconds(0.5f);

                InteractionAtGridCell(item.Value);
            }
        }
    }
}
