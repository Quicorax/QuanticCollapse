using System.Collections.Generic;
using UnityEngine;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };
public class InitialViewGridGeneration : MonoBehaviour
{
    private VirtualGridView _virtualGridView;

    private Vector2[] _cachedCoordsToCheck;
    private List<Vector2> _coordsToCheckList = new();

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;
    [SerializeField] private LevelGridData _levelData;

    private void Awake()
    {
        _virtualGridView = GetComponent<VirtualGridView>();
    }
    private void Start()
    {
        InitialGeneration();

        _virtualGridView.ModifyPlayerLife(playerData.starshipLife);
        _virtualGridView.ModifyEnemyLife(enemyData.starshipLife);
    }

    void InitialGeneration()
    {
        GenerateGridCells();
        FillGridCells();
    }
    void GenerateGridCells()
    {
        for (int x = 0; x < _levelData.gridDimensions.x; x++)
        {
            for (int y = 0; y < _levelData.gridDimensions.y; y++)
            {
                Vector2 gridCellCoords = new(x, y);
                _coordsToCheckList.Add(gridCellCoords);

                GridCell newGridCell = new(gridCellCoords);
                _virtualGridView.GenerateGidCell(gridCellCoords, newGridCell);
            }
        }
        _cachedCoordsToCheck = _coordsToCheckList.ToArray();
    }
    public void FillGridCells()
   {
       foreach (Vector2 cachedCoords in _cachedCoordsToCheck)
       {
           _virtualGridView.FillGidCellWithInitialDisposition(cachedCoords, _levelData);
       }
   }
}
