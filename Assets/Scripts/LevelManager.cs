using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CanvasDebugManager canvasDebugManager;
    public StarshipData starshipData;

    int interactionsRemaining;

    public int[] kindScores = new int[4];

    void Start()
    {
        ResetTurn();
        SetModulesPowerThreshold();
    }

    void SetModulesPowerThreshold()
    {
        for (int i = 0; i < starshipData.starshipModules.Length; i++)
            canvasDebugManager.SetMaxModuleSliderPower(i, starshipData.starshipModules[i].module.modulePowerThresholds[3]);
    }

    void ResetTurn()
    {
        interactionsRemaining = starshipData.maxInteractions;

        CallCanvasTurnUpdate(interactionsRemaining);

        for (int i = 0; i < starshipData.starshipModules.Length; i++)
            canvasDebugManager.UpdateModuleSlider(i, 0);
    }

    public void InteractionUsed(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);

        interactionsRemaining--;
        CallCanvasTurnUpdate(interactionsRemaining);

        if (interactionsRemaining <= 0)
            TurnEnded();
    }
    void AddScoreOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;
        int resultPower = kindScores[kindIndex] + amount;

        ModifyStarShipModuleScore(kindIndex, resultPower);
        canvasDebugManager.UpdateModuleSlider(kindIndex, resultPower);
    }


    void TurnEnded()
    {
        StarshipActions();
        //Enemy Actions 

        ResetModuleEnergy();
        ResetTurn();
    }
    void StarshipActions()
    {
        starshipData.ActivateModules(kindScores);
    }
    void ResetModuleEnergy()
    {
        for (int i = 0; i < kindScores.Length; i++)
            ModifyStarShipModuleScore(i, 0);
    }

    void ModifyStarShipModuleScore(int moduleKindIndex, int result) { kindScores[moduleKindIndex] = result; }
    void CallCanvasTurnUpdate(int i) { canvasDebugManager.SetTurns(i.ToString()); }
}
