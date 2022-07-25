using UnityEngine;
using UnityEngine.SceneManagement;

public class ModulesCanvas : MonoBehaviour
{
    public GameObject inputManager;

    public CanvasDebugManager canvasDebugManager;

    int interactionsRemaining;

    [SerializeField]
    private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField]
    private GenericEventBus _TurnEndedEventBus;
    [SerializeField]
    private AddScoreEventBus _AddScoreEventBus;

    private void Awake()
    {
        _PlayerInteractionEventBus.Event += Interaction;
        _TurnEndedEventBus.Event += ResetModulesCanvas;
        _AddScoreEventBus.Event += AddScore;
    }

    private void OnDestroy()
    {
        _PlayerInteractionEventBus.Event -= Interaction;
        _TurnEndedEventBus.Event -= ResetModulesCanvas;
        _AddScoreEventBus.Event -= AddScore;
    }
    private void Start()
    {
        interactionsRemaining = 5;
        SetModulesPowerThreshold();
        SetMaxStarshipLife();
    }
    void Interaction()
    {
        interactionsRemaining--;
    }
    void AddScore(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);
        CallCanvasTurnUpdate(interactionsRemaining);
    }
    void CallCanvasTurnUpdate(int i) { canvasDebugManager.SetTurns(i.ToString()); }
    void AddScoreOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;

        canvasDebugManager.AddModuleSlider(kindIndex, amount);
    }
    void SetModulesPowerThreshold()
    {
        for (int i = 0; i < 4; i++)
            canvasDebugManager.SetMaxModuleSliderPower(i, 15);
    }
    void SetMaxStarshipLife()
    {
        canvasDebugManager.SetMaxLifeModuleSlider(10);
    }

    public void ModifyPlayerLife(int amount)
    {
        canvasDebugManager.ChangePlayerLife(amount);
    }
    public void ModifyEnemyLife(int amount)
    {
        canvasDebugManager.ChangeEnemyLife(amount);
    }

    void ResetModulesCanvas()
    {
        interactionsRemaining = 5;
        CallCanvasTurnUpdate(interactionsRemaining);

        for (int i = 0; i < 4; i++)
            canvasDebugManager.ResetModuleSlider(i);
    }

    public void PlayerWin()
    {
        inputManager.SetActive(false);
        canvasDebugManager.PlayerWin();
    }
    public void PlayerLose()
    {
        inputManager.SetActive(false);
        canvasDebugManager.PlayerLose();
    }
}
