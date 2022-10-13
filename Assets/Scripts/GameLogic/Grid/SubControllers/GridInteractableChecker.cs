using UnityEngine;

namespace QuanticCollapse
{
    public class GridInteractableChecker
    {
        private GridModel _model;
        private PoolManager _poolManager;

        public int BoostersInGrid;

        public GridInteractableChecker(GridModel model, PoolManager poolManager)
        {
            _model = model;
            _poolManager = poolManager;
        }

        public void CheckBoardPossible()
        {
            if (BoostersInGrid > 0)
                return;

            foreach (var item in _model.GridData)
            {
                if (SimulateCheckInteractionWith(item.Value))
                    return;
            }

            NonInteractableBoard();
        }

        private bool SimulateCheckInteractionWith(GridCellModel gridCell)
        {
            foreach (Vector2Int coords in gridCell.BlockModel.Coords.GetCrossCoords())
            {
                if (_model.GridData.TryGetValue(coords, out GridCellModel objectiveCell) &&
                    gridCell.BlockModel != null && objectiveCell.BlockModel != null &&
                    gridCell.BlockModel.Id == objectiveCell.BlockModel.Id)
                {
                    return true;
                }
            }
            return false;
        }
        private void NonInteractableBoard()
        {
            Vector2Int randomCoords = new(Random.Range(0, 9), Random.Range(0, 7));

            if (_model.GridData.TryGetValue(randomCoords, out GridCellModel cell))
            {
                _poolManager.DeSpawnBlockView(cell.BlockModel.Id, _model.GridObjects[cell.AnchorCoords]);
                cell.BlockModel = null;
            }

            BaseBooster boosterLogic = new BoosterRowColumn(100);
            GameObject boosterObject = _poolManager.SpawnBlockView(boosterLogic.BoosterKindId, cell.BlockModel.Coords);

            _model.GridData[cell.BlockModel.Coords].BlockModel = new(boosterLogic.BoosterKindId, cell.BlockModel.Coords, boosterLogic);
            _model.GridObjects[cell.BlockModel.Coords] = boosterObject;
        }
    }
}