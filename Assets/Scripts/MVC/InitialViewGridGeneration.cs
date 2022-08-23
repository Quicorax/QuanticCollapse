using System.Collections.Generic;
using UnityEngine;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };
public class InitialViewGridGeneration : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    private VirtualGridView _virtualGridView;

    private Vector2Int[] _cachedCoordsToCheck;
    private List<Vector2Int> _coordsToCheckList = new();

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;
    
    private LevelGridData _levelData;

    private void Awake()
    {
        _LevelInjected.Event += SetLevelData;
        _virtualGridView = GetComponent<VirtualGridView>();
    }
    private void OnDisable()
    {
        _LevelInjected.Event -= SetLevelData;
    }
    private void Start()
    {
        InitialGeneration();

        _virtualGridView.ModifyPlayerLife(playerData.starshipLife);
        _virtualGridView.ModifyEnemyLife(_levelData.enemyStarshipMaxLife);
    }

    void SetLevelData(LevelGridData data) { _levelData = data; }
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
                Vector2Int gridCellCoords = new(x, y);
                _coordsToCheckList.Add(gridCellCoords);

                GridCellController newGridCell = new(gridCellCoords);
                _virtualGridView.GenerateGidCell(gridCellCoords, newGridCell);
            }
        }
        _cachedCoordsToCheck = _coordsToCheckList.ToArray();
    }
    public void FillGridCells()
   {
       foreach (Vector2Int cachedCoords in _cachedCoordsToCheck)
       {
           _virtualGridView.FillGidCellWithInitialDisposition(cachedCoords, _levelData);
       }
   }
}
