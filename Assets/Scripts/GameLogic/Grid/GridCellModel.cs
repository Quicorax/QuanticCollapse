using UnityEngine;

namespace QuanticCollapse
{
    public class GridCellModel
    {
        public Vector2Int AnchorCoords;
        public CellBlockModel BlockModel;

        public GridCellModel(Vector2Int anchorCoords)
        {
            AnchorCoords = anchorCoords;
        }
    }
}