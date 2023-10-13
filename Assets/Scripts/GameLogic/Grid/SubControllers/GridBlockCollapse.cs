using DG.Tweening;
using UnityEngine;

namespace QuanticCollapse
{
    public class GridBlockCollapse
    {
        private readonly GridModel _model;

        public GridBlockCollapse(GridModel model)
        {
            _model = model;
        }

        public void CheckCollapseBoard()
        {
            foreach (var element in _model.GridData)
            {
                if (element.Value.BlockModel != null)
                {
                    var cellCollapseSteps = 0;

                    for (var y = element.Key.y; y >= 0; y--)
                    {
                        if (_model.GridData.TryGetValue(new(element.Key.x, y), out GridCellModel gridCell)
                            && gridCell.BlockModel == null)
                        {
                            cellCollapseSteps++;
                        }
                    }

                    element.Value.BlockModel.CollapseSteps = cellCollapseSteps;
                }
            }

            CollapseBlocks();
        }

        private void CollapseBlocks()
        {
            foreach (var gridCell in _model.GridData.Values)
            {
                if (gridCell.BlockModel != null && gridCell.BlockModel.CollapseSteps > 0)
                {
                    var newCoords = gridCell.BlockModel.Coords + Vector2Int.down * gridCell.BlockModel.CollapseSteps;

                    var model = _model.GridData[newCoords];
                    model.BlockModel = gridCell.BlockModel;
                    model.BlockModel.Coords = newCoords;
                    model.BlockModel.CollapseSteps = 0;
                    gridCell.BlockModel = null;

                    var gridObject = _model.GridObjects[gridCell.AnchorCoords];
                    gridObject.transform.DOMoveY(newCoords.y, 0.4f).SetEase(Ease.OutBounce);

                    _model.GridObjects[newCoords] = gridObject;
                }
            }
        }
    }
}