using UnityEngine;

namespace QuanticCollapse
{
    public class StarshipManager : MonoBehaviour
    {
        [HideInInspector] public int[] dynamicPlayerEnergyGrid = new int[4];

        [SerializeField] private GenericEventBus _TurnEndedEventBus;
        [SerializeField] private AddScoreEventBus _AddScoreEventBus;
        [SerializeField] private LevelInjectedEventBus _LevelInjected;

        [SerializeField] private StarshipData playerStarshipData;
        [SerializeField] private StarshipData enemyStarshipData;

        private int _botDifficulty;

        private void Awake()
        {
            _AddScoreEventBus.Event += AddPowerOfKind;
            _TurnEndedEventBus.Event += StarshipActions;
            _LevelInjected.Event += SetLevelData;
        }

        private void OnDisable()
        {
            _AddScoreEventBus.Event -= AddPowerOfKind;
            _TurnEndedEventBus.Event -= StarshipActions;
            _LevelInjected.Event -= SetLevelData;
        }

        private void SetLevelData(LevelModel data) => _botDifficulty = data.EnemyLevel;

        private void AddPowerOfKind(int kindId, int amount) =>
            ModifyStarShipModuleScore(kindId, dynamicPlayerEnergyGrid[kindId] + amount);

        private void StarshipActions()
        {
            playerStarshipData.CheckModuleActivation(dynamicPlayerEnergyGrid);
            enemyStarshipData.CheckModuleActivation(GetEnemyEnergyGrid());

            ResetModuleEnergy();
        }

        private void ResetModuleEnergy()
        {
            for (var index = 0; index < 4; index++)
            {
                ModifyStarShipModuleScore(index, 0);
            }
        }

        private void ModifyStarShipModuleScore(int moduleKindIndex, int result) =>
            dynamicPlayerEnergyGrid[moduleKindIndex] = result;

        private int[] GetEnemyEnergyGrid()
        {
            var powerGrid = new int[4];

            powerGrid[0] = 1 + Random.Range(0, 10) * (_botDifficulty / 10);
            powerGrid[1] = Random.Range(0, 10) * (_botDifficulty / 10);
            powerGrid[2] = Random.Range(0, 10) * (_botDifficulty / 10);
            powerGrid[3] = Random.Range(0, 10) * (_botDifficulty / 10);

            return powerGrid;
        }
    }
}