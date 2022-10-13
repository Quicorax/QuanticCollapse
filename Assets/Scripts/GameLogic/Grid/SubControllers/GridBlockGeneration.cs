using DG.Tweening;
using System.Linq;
using UnityEngine;

public class GridBlockGeneration
{
    private GridModel _model;
    private PoolManager _poolManager;
    private GameConfigService _config;
    private BoostersLogic _boostersLogic = new();
    private GridInteractableChecker _gridInteractableChecker;
    public GridBlockGeneration(GridModel model, PoolManager poolManager, GridInteractableChecker gridInteractableChecker)
    {
        _model = model;
        _poolManager = poolManager;
        _gridInteractableChecker = gridInteractableChecker;
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
}