using UnityEngine;

namespace QuanticCollapse
{
    public class StarshipManager : MonoBehaviour
    {
        [SerializeField] private GenericEventBus _TurnEndedEventBus;
        [SerializeField] private AddScoreEventBus _AddScoreEventBus;
        [SerializeField] private LevelInjectedEventBus _LevelInjected;

        [SerializeField] private StarshipData playerStarshipData;
        [SerializeField] private StarshipData enemyStarshipData;

        [HideInInspector] public int[] dynamicPlayerEnergyGrid = new int[4];

        private int AIdifficulty;

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

        void SetLevelData(LevelModel data) => AIdifficulty = data.EnemyLevel;
        private void AddPowerOfKind(int kindId, int amount)
            => ModifyStarShipModuleScore(kindId, dynamicPlayerEnergyGrid[kindId] + amount);

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

        void ModifyStarShipModuleScore(int moduleKindIndex, int result) => dynamicPlayerEnergyGrid[moduleKindIndex] = result;

        int[] GetEnemyEnergyGrid()
        {
            int[] powerGrid = new int[4];

            powerGrid[0] = 1 + Random.Range(0, 10) * (AIdifficulty / 10);
            powerGrid[1] = Random.Range(0, 10) * (AIdifficulty / 10);
            powerGrid[2] = Random.Range(0, 10) * (AIdifficulty / 10);
            powerGrid[3] = Random.Range(0, 10) * (AIdifficulty / 10);

            return powerGrid;
        }

    }
}