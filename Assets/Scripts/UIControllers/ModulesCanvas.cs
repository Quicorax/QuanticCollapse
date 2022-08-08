using UnityEngine;
using UnityEngine.SceneManagement;

public class ModulesCanvas : MonoBehaviour
{
    public GameObject inputManager;

    public CanvasDebugManager canvasDebugManager;

    int interactionsRemaining;

    [SerializeField] private GenericEventBus _WinConditionEventBus;
    [SerializeField] private GenericEventBus _LoseConditionEventBus;
    [SerializeField] private GenericEventBus _PlayerInteractionEventBus;
    [SerializeField] private GenericEventBus _TurnEndedEventBus;
    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    private void Awake()
    {
        _LoseConditionEventBus.Event += PlayerLose;
        _WinConditionEventBus.Event += PlayerWin;
        _PlayerInteractionEventBus.Event += Interaction;
        _TurnEndedEventBus.Event += ResetModulesCanvas;
        _AddScoreEventBus.Event += AddScore;
    }

    private void OnDestroy()
    {
        _LoseConditionEventBus.Event -= PlayerLose;
        _WinConditionEventBus.Event -= PlayerWin;
        _PlayerInteractionEventBus.Event -= Interaction;
        _TurnEndedEventBus.Event -= ResetModulesCanvas;
        _AddScoreEventBus.Event -= AddScore;
    }
    private void Start()
    {
        interactionsRemaining = 5;
        SetModulesPowerThreshold();
    }
    void Interaction()
    {
        interactionsRemaining--;
        CallCanvasTurnUpdate(interactionsRemaining);

    }
    void AddScore(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);
    }
    void CallCanvasTurnUpdate(int i) { canvasDebugManager.SetTurns(i); }
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

    void ResetModulesCanvas()
    {
        interactionsRemaining = 5;
        CallCanvasTurnUpdate(interactionsRemaining);

        for (int i = 0; i < 4; i++)
            canvasDebugManager.ResetModuleSlider(i);
    }

    void PlayerWin()
    {
        inputManager.SetActive(false);
        canvasDebugManager.PlayerWin();
    }
    void PlayerLose()
    {
        inputManager.SetActive(false);
        canvasDebugManager.PlayerLose();
    }
}
