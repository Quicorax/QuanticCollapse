using UnityEngine;

public class StarshipManager : MonoBehaviour
{
    public CanvasDebugManager canvasDebugManager;
    public StarshipData starshipData;
    public TurnManager turnManager;

    public int[] kindScores = new int[4];
    private void Awake()
    {
        turnManager.OnInteraction += Interaction;
        turnManager.OnTurnEnded += StarshipActions;
    }

    private void OnDestroy()
    {
        turnManager.OnInteraction -= Interaction;
        turnManager.OnTurnEnded -= StarshipActions;

    }

    void Interaction(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);
    }

    void Start()
    {
        SetModulesPowerThreshold();
    }

    void SetModulesPowerThreshold()
    {
        for (int i = 0; i < starshipData.starshipModules.Length; i++)
            canvasDebugManager.SetMaxModuleSliderPower(i, starshipData.starshipModules[i].module.modulePowerThresholds[3]);
    }

    public void AddScoreOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;
        int resultPower = kindScores[kindIndex] + amount;

        ModifyStarShipModuleScore(kindIndex, resultPower);
    }

    public void StarshipActions()
    {
        starshipData.ActivateModules(kindScores);
        ResetModuleEnergy();
    }
    void ResetModuleEnergy()
    {
        for (int i = 0; i < kindScores.Length; i++)
            ModifyStarShipModuleScore(i, 0);
    }

    void ModifyStarShipModuleScore(int moduleKindIndex, int result) { kindScores[moduleKindIndex] = result; }

}
