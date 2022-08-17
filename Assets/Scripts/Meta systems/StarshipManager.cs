using UnityEngine;

public class StarshipManager : MonoBehaviour
{
    [SerializeField] private GenericEventBus _TurnEndedEventBus;
    [SerializeField] private AddScoreEventBus _AddScoreEventBus;

    [Range(0, 20)]
    [SerializeField] private int AIdifficulty;

    [SerializeField] private StarshipData playerStarshipData;
    [SerializeField] private StarshipData enemyStarshipData;


    private int[] dynamicPlayerEnergyGrid = new int[4];

    private void Awake()
    {
        _AddScoreEventBus.Event += AddPowerOfKind;
        _TurnEndedEventBus.Event += StarshipActions;
    }

    private void OnDestroy()
    {
        _AddScoreEventBus.Event -= AddPowerOfKind;
        _TurnEndedEventBus.Event -= StarshipActions;
    }

    public void AddPowerOfKind(ElementKind kind, int amount)
    {
        int kindIndex = (int)kind;
        int resultPower = dynamicPlayerEnergyGrid[kindIndex] + amount;

        ModifyStarShipModuleScore(kindIndex, resultPower);
    }

    public void StarshipActions()
    {
        playerStarshipData.CheckModuleActivation(dynamicPlayerEnergyGrid);
        enemyStarshipData.CheckModuleActivation(GetEnemyEnergyGrid());

        ResetModuleEnergy();
    }
    void ResetModuleEnergy()
    {
        for (int i = 0; i < 4; i++)
        {
            ModifyStarShipModuleScore(i, 0);
        }
    }

    void ModifyStarShipModuleScore(int moduleKindIndex, int result) 
    {
        dynamicPlayerEnergyGrid[moduleKindIndex] = result; 
    }


    int[] GetEnemyEnergyGrid()
    {
        int[] powerGrid = new int[4];

        powerGrid[0] = Random.Range(0, 10) * (AIdifficulty / 10);
        powerGrid[1] = Random.Range(0, 10) * (AIdifficulty / 10);
        powerGrid[2] = Random.Range(0, 10) * (AIdifficulty / 10);
        powerGrid[3] = Random.Range(0, 10) * (AIdifficulty / 10);

        return powerGrid;
    }

}
