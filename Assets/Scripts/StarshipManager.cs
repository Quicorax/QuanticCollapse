using UnityEngine;

public class StarshipManager : MonoBehaviour
{
    public StarshipData starshipData;

    public int[] kindScores = new int[4];
    private void Awake()
    {
        EventManager.Instance.OnInteraction += Interaction;
        EventManager.Instance.OnTurnEnded += StarshipActions;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnInteraction -= Interaction;
        EventManager.Instance.OnTurnEnded -= StarshipActions;

    }

    void Interaction(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);
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
