using UnityEngine;

public class ModulesCanvas : MonoBehaviour
{
    public CanvasDebugManager canvasDebugManager;

    int interactionsRemaining;

    private void Awake()
    {
        EventManager.Instance.OnInteraction += Interaction;
        EventManager.Instance.OnTurnEnded += ResetModulesCanvas;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnInteraction -= Interaction;
        EventManager.Instance.OnTurnEnded -= ResetModulesCanvas;
    }
    private void Start()
    {
        interactionsRemaining = 5;
        SetModulesPowerThreshold();
    }
    void Interaction(ElementKind kind, int amount)
    {
        interactionsRemaining--;
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


    void ResetModulesCanvas()
    {
        interactionsRemaining = 5;
        CallCanvasTurnUpdate(interactionsRemaining);

        for (int i = 0; i < 4; i++)
            canvasDebugManager.ResetModuleSlider(i);
    }
}
