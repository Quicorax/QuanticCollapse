using UnityEngine;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };
public class InitialGridGeneration : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;

    [SerializeField] private VirtualGridView View;

    private LevelGridData _levelData;

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
        GenerateGridCells();

        View.Controller.ModifyPlayerLife(playerData.starshipLife);
        View.Controller.ModifyEnemyLife(_levelData.enemyStarshipMaxLife);
    }

    void SetLevelData(LevelGridData data) { _levelData = data; }
    void GenerateGridCells()
    {
        for (int x = 0; x < _levelData.gridDimensions.x; x++)
        {
            for (int y = 0; y < _levelData.gridDimensions.y; y++)
            {
                Vector2Int gridCellCoords = new(x, y);

                GridCellController newGridCell = new(gridCellCoords);
                View.Controller.GenerateInitialGidCell(gridCellCoords, _levelData, newGridCell);
            }
        }
    }
}
