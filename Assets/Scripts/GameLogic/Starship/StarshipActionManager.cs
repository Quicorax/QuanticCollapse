using UnityEngine;
namespace QuanticCollapse
{
    public class StarshipActionManager : MonoBehaviour
    {
        [SerializeField]
        private StarshipModuleActivationEventBus _StarshipModuleActivationEventBus;
        [SerializeField]
        private GenericEventBus _playerHitEventBus;

        [SerializeField]
        private GridView View;

        [SerializeField]
        private ParticleSystem AttackParticles;
        [SerializeField]
        private ParticleSystem EnemyAttackParticles;

        private int[] finalPlayerEnergyGrid = new int[4];
        private int[] finalEnemyEnergyGrid = new int[4];

        private int playerActionsFilledAmount = 0;
        private int enemyActionsFilledAmount = 0;

        private bool playerActionsFilled;
        private bool enemyActionsFilled;
        private bool turnCompared;

        private void Awake()
        {
            _StarshipModuleActivationEventBus.Event += ReceivePowerCall;
        }

        private void OnDisable()
        {
            _StarshipModuleActivationEventBus.Event -= ReceivePowerCall;
        }

        private void ReceivePowerCall(bool player, int kindId, int force)
        {
            if (player)
                AddPlayerAction(kindId, force);
            else
                AddEnemyAction(kindId, force);
        }
        private void AddPlayerAction(int kindId, int force)
        {
            finalPlayerEnergyGrid[kindId] = force;
            playerActionsFilledAmount++;

            if (playerActionsFilledAmount < 4)
                return;

            playerActionsFilled = true;

            if (enemyActionsFilled && !turnCompared)
                CompareActionForces();
        }
        private void AddEnemyAction(int kindId, int force)
        {
            finalEnemyEnergyGrid[kindId] = force;
            enemyActionsFilledAmount++;

            if (enemyActionsFilledAmount < 4)
                return;

            enemyActionsFilled = true;

            if (playerActionsFilled && !turnCompared)
                CompareActionForces();
        }

        private void CompareActionForces()
        {
            turnCompared = true;
            Comparison();

            ResetAction();
        }

        private void Comparison()
        {
            bool playerFirst = finalPlayerEnergyGrid[3] >= finalEnemyEnergyGrid[3];

            if (playerFirst)
            {
                DamageEnemy();
                DamagePlayer();
            }
            else
            {
                DamagePlayer();
                DamageEnemy();
            }
        }

        private void DamagePlayer()
        {
            int playerDeltaDamage = 1 + finalEnemyEnergyGrid[0] - finalPlayerEnergyGrid[1];
            Debug.Log("Damage to player: " + playerDeltaDamage);
            if (playerDeltaDamage > 0)
            {
                int finalDamage = playerDeltaDamage + 1 * finalEnemyEnergyGrid[2];
                Invoke(nameof(PlayerHitVisuals), 0.5f);
                ModifyPlayerHealth.Do(-finalDamage);
            }
        }
        private void PlayerHitVisuals()
        {
            EnemyAttackParticles.Play();
            _playerHitEventBus.NotifyEvent();
        }

        private void DamageEnemy()
        {
            int enemyDeltaDamage = finalPlayerEnergyGrid[0] - finalEnemyEnergyGrid[1];
            if (enemyDeltaDamage > 0)
            {
                int finalDamage = enemyDeltaDamage * 1 + finalPlayerEnergyGrid[2];
                EnemyHitVisuals();
                ModifyEnemyHealth.Do(-finalDamage);
            }
        }
        private void EnemyHitVisuals() => AttackParticles.Play();

        private void ResetAction()
        {
            turnCompared = false;
            playerActionsFilled = false;
            enemyActionsFilled = false;

            playerActionsFilledAmount = 0;
            enemyActionsFilledAmount = 0;

            for (int i = 0; i < 4; i++)
            {
                finalPlayerEnergyGrid[i] = 0;
                finalEnemyEnergyGrid[i] = 0;
            }
        }
    }
}