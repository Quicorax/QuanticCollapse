using DG.Tweening;
using System.Linq;
using UnityEngine;

public class GridBlockLifeCycle
{
    public GridCellModel BoosterGridCell;

    private GridModel _model;
    private PoolManager _poolManager;
    private GameConfigService _config;
    private BoostersLogic _boostersLogic = new();
    private GridInteractableChecker _gridInteractableChecker;
    private AddScoreEventBus _AddScoreEventBus;
    public GridBlockLifeCycle(GridModel model, PoolManager poolManager, GridInteractableChecker gridInteractableChecker, AddScoreEventBus addScoreEventBus)
    {
        _model = model;
        _poolManager = poolManager;
        _gridInteractableChecker = gridInteractableChecker;
        _AddScoreEventBus = addScoreEventBus;
        _config = ServiceLocator.GetService<GameConfigService>();
    }

    public void GenerateBlocksOnEmptyCells()
    {
        foreach (var item in _model.GridData)
        {
            if (item.Value.BlockModel == null)
            {
                int _blockId = _config.GridBlocks.BaseBlocks[Random.Range(0, _config.GridBlocks.BaseBlocks.Count())].Id;
                GameObject newBlockView = _poolManager.SpawnBlockView(_blockId, new Vector2Int(item.Key.x, 8));
                newBlockView.transform.DOMoveY(item.Key.y, 0.4f).SetEase(Ease.OutBounce);

                _model.GridData[item.Key].BlockModel = new(_blockId, item.Key);
                _model.GridObjects[item.Key] = newBlockView;
            }
        }
    }
    public void SpawnBooster(Vector2Int coords)
    {
        if (_boostersLogic.CheckBaseBoosterSpawn(_model.MatchClosedList.Count, out BaseBooster booster))
        {
            _gridInteractableChecker.BoostersInGrid++;

            Transform newBooster = _poolManager.SpawnBlockView(booster.BoosterKindId, coords).transform;

            newBooster.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            newBooster.DOPunchRotation(Vector3.forward * 120, 0.3f);

            _model.GridData[coords].BlockModel = new(booster.BoosterKindId, coords, booster);
            _model.GridObjects[coords] = newBooster.gameObject;
        }
    }

    public void CheckTriggeredBoostersToInteract()
    {
        foreach (var gridCell in _model.GridData.Values)
        {
            if (gridCell.BlockModel != null && gridCell.BlockModel.IsTriggered && !_model.MatchOpenList.Contains(gridCell))
                _model.MatchOpenList.Add(gridCell);
        }
    }
    public void DestroyBlocksOnActionSucceed()
    {
        if (BoosterGridCell != null)
        {
            _gridInteractableChecker.BoostersInGrid--;

            _poolManager.DeSpawnBlockView(BoosterGridCell.BlockModel.Id, _model.GridObjects[BoosterGridCell.AnchorCoords]);
            BoosterGridCell.BlockModel = null;
        }

        foreach (GridCellModel dynamicBlock in _model.MatchClosedList)
        {
            if (dynamicBlock.BlockModel.Booster != null)
                dynamicBlock.BlockModel.IsTriggered = true;
            else
            {
                _poolManager.DeSpawnBlockView(dynamicBlock.BlockModel.Id, _model.GridObjects[dynamicBlock.AnchorCoords]);
                dynamicBlock.BlockModel = null;
            }
        }
    }

    public void DestroyBlockOnLaserBooster(GridCellModel gridCell)
    {
        if (gridCell.BlockModel.Booster == null)
            _AddScoreEventBus.NotifyEvent(gridCell.BlockModel.Id, 1);

        _poolManager.DeSpawnBlockView(gridCell.BlockModel.Id, _model.GridObjects[gridCell.AnchorCoords]);
        gridCell.BlockModel = null;
    }
}