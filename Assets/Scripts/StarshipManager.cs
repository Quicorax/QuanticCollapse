using UnityEngine;

public class StarshipManager : MonoBehaviour
{
    [Range(0, 20)]
    public int AIdifficulty;

    public StarshipData playerStarshipData;

    public StarshipData enemyStarshipData;

    public int[] playerEnergyGrid = new int[4];
    public int[] enemyEnergyGrid = new int[4];

    private void Awake()
    {
        EventManager.Instance.OnAddScore += Interaction;
        EventManager.Instance.OnTurnEnded += StarshipActions;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnAddScore -= Interaction;
        EventManager.Instance.OnTurnEnded -= StarshipActions;

    }

    void Interaction(ElementKind kind, int amount)
    {
        AddScoreOfKind(kind, amount);
    }

    public void AddScoreOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;
        int resultPower = playerEnergyGrid[kindIndex] + amount;

        ModifyStarShipModuleScore(kindIndex, resultPower);
    }

    public void StarshipActions()
    {
        playerStarshipData.CheckModuleActivation(playerEnergyGrid);

        DefineEnemyEnergyGrid();
        enemyStarshipData.CheckModuleActivation(enemyEnergyGrid);

        ResetModuleEnergy();
    }
    void ResetModuleEnergy()
    {
        for (int i = 0; i < playerEnergyGrid.Length; i++)
        {
            ModifyStarShipModuleScore(i, 0);
            enemyEnergyGrid[i] = 0;
        }
    }

    void ModifyStarShipModuleScore(int moduleKindIndex, int result) { playerEnergyGrid[moduleKindIndex] = result; }


    void DefineEnemyEnergyGrid()
    {
        enemyEnergyGrid[0] = Random.Range(0, 10) * (AIdifficulty/10);
        enemyEnergyGrid[1] = Random.Range(0, 10) * (AIdifficulty/10);
        enemyEnergyGrid[2] = Random.Range(0, 10) * (AIdifficulty/10);
        enemyEnergyGrid[3] = Random.Range(0, 10) * (AIdifficulty/10);
    }

}
