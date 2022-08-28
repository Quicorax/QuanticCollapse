using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum ElementKind { Attack, Defense, Intel, Speed, BoosterRowColumn, BoosterBomb, BoosterKindBased };

[System.Serializable]
public struct ControllerElements
{
    public GenericEventBus _LoseConditionEventBus;
    public GenericEventBus _WinConditionEventBus;
    public GenericIntEventBus _enemyDamagedEventBus;
    public GenericIntEventBus _playerDamagedEventBus;

    public GridInteractionSubController _interactionsController;
    public PoolManager _poolManager;
}

public class VirtualGridView : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [SerializeField] private PlayerStarshipData playerData;
    [SerializeField] private EnemyStarshipData enemyData;


    [SerializeField] private Slider enemyLifeSlider;
    [SerializeField] private Slider playerLifeSlider;

    [SerializeField] private GenericIntEventBus _enemyDamagedEventBus;
    [SerializeField] private GenericIntEventBus _playerDamagedEventBus;

    public ControllerElements controllerElements;

    public VirtualGridController Controller;
    private LevelModel _levelData;

    private Texture2D _gridInitialLayout;

    private void Awake()
    {
        Controller = new(controllerElements);

        _enemyDamagedEventBus.Event += EnemyDamaged;
        _playerDamagedEventBus.Event += PlayerDamaged;
        _LevelInjected.Event += SetLevelData;

    }
    private void OnDestroy()
    {
        _enemyDamagedEventBus.Event -= EnemyDamaged;
        _playerDamagedEventBus.Event -= PlayerDamaged;
        _LevelInjected.Event -= SetLevelData;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        GenerateGridCells();

        Controller.ModifyPlayerLife(playerData.starshipLife);
        Controller.ModifyEnemyLife(20);
    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) { Controller.ListenInput(inputCoords, boostedInput); }
    void PlayerDamaged(int amount) => playerLifeSlider.value += amount; 
    void EnemyDamaged(int amount) => enemyLifeSlider.value += amount; 
    void SetLevelData(LevelModel data)
    {
        _levelData = data;
        LoadInitialGridTexture(data.Level.ToString());
    } 
    void LoadInitialGridTexture(string levelIndex)
    {
        Texture2D expectedInitialDisposition = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/LevelDispositionData/Level_" + levelIndex + ".psd", typeof(Texture2D));

        if (expectedInitialDisposition != null)
            _gridInitialLayout = expectedInitialDisposition;
        else
            _gridInitialLayout = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/LevelDispositionData/Level_999.psd", typeof(Texture2D));
    }

    void GenerateGridCells()
    {
        for (int x = 0; x < _gridInitialLayout.width; x++)
        {
            for (int y = 0; y < _gridInitialLayout.height; y++)
            {
                Vector2Int gridCellCoords = new(x, y);

                GridCellController newGridCell = new(gridCellCoords);
                Controller.GenerateInitialGidCell(gridCellCoords, _gridInitialLayout, newGridCell);
            }
        }
    }
}
