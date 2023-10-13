using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace QuanticCollapse
{
    public class GridController
    {
        private readonly AddScoreEventBus _addScoreEventBus;
        private readonly GenericEventBus _blockDestructionEventBus;

        private readonly UserInputManager _userInputManager;
        private readonly TurnManager _turnManager;

        private readonly GridModel _model;

        private readonly GridInteractableChecker _gridInteractableChecker;
        private readonly GridBlockCollapse _gridBlockCollapse;
        private readonly GridBlockLifeCycle _gridBlockLifeCycle;
        private readonly GenerateInitialGrid _initialGeneration;

        public GridController(GridModel model, AddScoreEventBus addScoreEventBus,
            GenericEventBus blockDestructionEventBus, PoolManager poolManager, UserInputManager userInputManager,
            TurnManager turnManager)
        {
            _addScoreEventBus = addScoreEventBus;
            _blockDestructionEventBus = blockDestructionEventBus;
            _userInputManager = userInputManager;
            _turnManager = turnManager;

            _model = model;

            _initialGeneration = new(_model, poolManager);
            _gridInteractableChecker = new(_model, poolManager);
            _gridBlockLifeCycle = new(_model, poolManager, _gridInteractableChecker, _addScoreEventBus);
            _gridBlockCollapse = new(_model);
        }

        public async Task Interact(Vector2Int inputCoords, bool boostedInput)
        {
            if (_model.GridData.TryGetValue(inputCoords, out GridCellModel gridCell))
            {
                if (gridCell.BlockModel == null)
                {
                    return;
                }

                if (!boostedInput)
                {
                    InteractionAtGridCell(gridCell);
                }
                else
                {
                    _gridBlockLifeCycle.DestroyBlockOnLaserBooster(gridCell);
                    await Task.Delay(250);

                    RegenerateGrid();
                }
            }
        }

        public void GenerateInitialGidCell(LevelModel levelModel) => _initialGeneration.Initialize(levelModel);


        private void InteractionAtGridCell(GridCellModel gridCell)
        {
            _userInputManager.BlockInputByGridInteraction(true);
            OpenCloseAutoClickSystem(gridCell).ManageTaskException();
        }

        private async Task OpenCloseAutoClickSystem(GridCellModel gridCell)
        {
            var autoInput = false;

            _model.MatchOpenList.Add(gridCell);
            while (_model.MatchOpenList.Count > 0)
            {
                var tappedGridCell = _model.MatchOpenList[0];
                _model.MatchOpenList.RemoveAt(0);
                InteractionCore(tappedGridCell, autoInput).ManageTaskException();

                _model.MatchClosedList.Clear();
                _gridBlockLifeCycle.BoosterGridCell = null;
                autoInput = true;
                await Task.Delay(500);
            }

            _model.MatchOpenList.Clear();
            _userInputManager.BlockInputByGridInteraction(false);
        }

        private async Task InteractionCore(GridCellModel gridCell, bool autoInput)
        {
            if (!CheckInteractionWith(gridCell))
            {
                return;
            }

            AddScoreOnInteractionSucceed();

            if (!autoInput)
            {
                _turnManager.InteractionUsed();
            }

            _gridBlockLifeCycle.DestroyBlocksOnActionSucceed();

            if (_gridBlockLifeCycle.BoosterGridCell == null)
            {
                _gridBlockLifeCycle.SpawnBooster(gridCell.AnchorCoords);
            }

            await Task.Delay(250);

            RegenerateGrid();
        }

        private void RegenerateGrid()
        {
            _gridBlockCollapse.CheckCollapseBoard();
            _gridBlockLifeCycle.GenerateBlocksOnEmptyCells();
            _gridInteractableChecker.CheckBoardPossible();
            _gridBlockLifeCycle.CheckTriggeredBoostersToInteract();
        }

        private bool CheckInteractionWith(GridCellModel gridCell)
        {
            var boosterMatchInteraction = false;

            if (gridCell.BlockModel.Booster == null)
            {
                OpenClosedListMatchCellsGetter(gridCell);
            }
            else
            {
                CheckActionOnBoosterBased(gridCell);
                boosterMatchInteraction = true;
            }

            return _model.MatchClosedList.Count >= 2 || boosterMatchInteraction;
        }

        private void CheckActionOnBoosterBased(GridCellModel gridCell)
        {
            _gridBlockLifeCycle.BoosterGridCell = gridCell;
            gridCell.BlockModel.Booster.OnInteraction(gridCell.BlockModel.Coords, _model);
        }

        private void OpenClosedListMatchCellsGetter(GridCellModel touchedGridCell)
        {
            var matchOpenList = new List<GridCellModel>();

            if (touchedGridCell.BlockModel == null)
            {
                return;
            }

            matchOpenList.Add(touchedGridCell);

            while (matchOpenList.Count > 0)
            {
                var selectedGridCell = matchOpenList[0];
                matchOpenList.RemoveAt(0);
                _model.MatchClosedList.Add(selectedGridCell);

                foreach (var coords in selectedGridCell.BlockModel.Coords.GetCrossCoords())
                {
                    if (_model.GridData.TryGetValue(coords, out var objectiveCell)
                        && objectiveCell.BlockModel != null &&
                        touchedGridCell.BlockModel.Id == objectiveCell.BlockModel.Id
                        && !matchOpenList.Contains(objectiveCell) && !_model.MatchClosedList.Contains(objectiveCell))
                    {
                        matchOpenList.Add(objectiveCell);
                    }
                }
            }
        }

        private void AddScoreOnInteractionSucceed()
        {
            var elementCount = 0;
            var cellId = _model.MatchClosedList[0].BlockModel.Id;

            foreach (var CellController in _model.MatchClosedList.Where(CellController =>
                         CellController.BlockModel.Booster == null))
            {
                cellId = CellController.BlockModel.Id;
                elementCount++;
            }

            _blockDestructionEventBus.NotifyEvent();

            if (_model.MatchClosedList[0].BlockModel.Booster == null)
            {
                _addScoreEventBus.NotifyEvent(cellId, elementCount);
            }
        }
    }
}