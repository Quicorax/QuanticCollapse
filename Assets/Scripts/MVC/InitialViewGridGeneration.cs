using System.Collections.Generic;
using UnityEngine;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };
public class InitialViewGridGeneration : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    private Vector2Int[] _cachedCoordsToCheck;
    private List<Vector2Int> _coordsToCheckList = new();

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;
    
    private LevelGridData _levelData;

    public VirtualGridController Controller;

    private void Awake()
    {
        _LevelInjected.Event += SetLevelData;
    }
    private void OnDisable()
    {
        _LevelInjected.Event -= SetLevelData;
    }
    private void Start()
    {
        InitialGeneration();

        Controller.ModifyPlayerLife(playerData.starshipLife);
        Controller.ModifyEnemyLife(_levelData.enemyStarshipMaxLife);
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
                Controller.GenerateGidCell(gridCellCoords, newGridCell);
            }
        }
        _cachedCoordsToCheck = _coordsToCheckList.ToArray();
    }
    public void FillGridCells()
   {
       foreach (Vector2Int cachedCoords in _cachedCoordsToCheck)
       {
            Controller.FillGidCellWithInitialDisposition(cachedCoords, _levelData);
       }
   }
}
