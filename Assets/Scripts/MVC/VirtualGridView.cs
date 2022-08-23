using UnityEngine;
using UnityEngine.UI;

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
    public Slider enemyLifeSlider;
    public Slider playerLifeSlider;

    public GenericIntEventBus _enemyDamagedEventBus;
    public GenericIntEventBus _playerDamagedEventBus;

    public ControllerElements controllerElements;

    public VirtualGridController Controller;

    private void Awake()
    {
        Controller = new(controllerElements);

        _enemyDamagedEventBus.Event += EnemyDamaged;
        _playerDamagedEventBus.Event += PlayerDamaged;
    }
    private void OnDestroy()
    {
        _enemyDamagedEventBus.Event -= EnemyDamaged;
        _playerDamagedEventBus.Event -= PlayerDamaged;
    }
    public void ProcessInput(Vector2Int inputCoords, bool boostedInput) { Controller.ListenInput(inputCoords, boostedInput); }

    void PlayerDamaged(int amount) { playerLifeSlider.value += amount; }
    void EnemyDamaged(int amount) { enemyLifeSlider.value += amount; }
}
