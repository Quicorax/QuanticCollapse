using UnityEngine;
public class StarshipActionManager : MonoBehaviour
{
    [SerializeField] private VirtualGridView View;
    [SerializeField] private ModulesCanvas modulesCanvas;

    [SerializeField] private StarshipModuleActivationEventBus _StarshipModuleActivationEventBus;
    [SerializeField] private GenericEventBus _playerHitEventBus;

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

    void ReceivePowerCall(bool player, ElementKind kind, int force)
    {
        if (player)
            AddPlayerAction(kind, force);
        else
            AddEnemyAction(kind, force);
    }
    void AddPlayerAction(ElementKind kind, int force)
    {
        finalPlayerEnergyGrid[(int)kind] = force;
        playerActionsFilledAmount++;

        if (playerActionsFilledAmount < 4)
            return;

        playerActionsFilled = true;

        if (enemyActionsFilled && !turnCompared)
            CompareActionForces();
    }
    void AddEnemyAction(ElementKind kind, int force)
    {
        finalEnemyEnergyGrid[(int)kind] = force;
        enemyActionsFilledAmount++;

        if (enemyActionsFilledAmount < 4)
            return;

        enemyActionsFilled = true;

        if (playerActionsFilled && !turnCompared)
            CompareActionForces();
    }
   
    void CompareActionForces()
    {
        turnCompared = true;
        Comparison();

        ResetAction();
    }

    void Comparison()
    {
        bool playerFirst = finalPlayerEnergyGrid[3] >= finalEnemyEnergyGrid[3];

        if (playerFirst)
        {
            if (DamageEnemy())
                return;

            DamagePlayer();
        }
        else
        {
            if (DamagePlayer())
                return;

            DamageEnemy();
        }
    }

    bool DamagePlayer()
    {
        int playerDeltaDamage = finalEnemyEnergyGrid[0] - finalPlayerEnergyGrid[1];
        if (playerDeltaDamage > 0)
        {
            int finalDamage = playerDeltaDamage * 1 + finalEnemyEnergyGrid[2];

            View.ModifyPlayerLife(-finalDamage);

            _playerHitEventBus.NotifyEvent();
        }
        return View.Controller.Model.PlayerLife <= 0;
    }
    bool DamageEnemy()
    {
        int enemyDeltaDamage = finalPlayerEnergyGrid[0] - finalEnemyEnergyGrid[1];
        if (enemyDeltaDamage > 0)
        {
            int finalDamage = enemyDeltaDamage * 1 + finalPlayerEnergyGrid[2];

            View.ModifyEnemyLife(-finalDamage);
        }
        return View.Controller.Model.EnemyLife <= 0;
    }

    void ResetAction()
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
