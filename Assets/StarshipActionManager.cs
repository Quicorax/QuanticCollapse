using UnityEngine;

public class StarshipActionManager : MonoBehaviour
{
    [Header("Player starship")]
    public int playerLife;
    public int playerSpeedForce;
    public int playerDefenseForce;
    public int playerAttackForce;
    public int playerIntelForce;

    [Header("Enemy starship")]
    public int enemyLife;
    public int enemySpeedForce;
    public int enemyDefenseForce;
    public int enemyAttackForce;
    public int enemyIntelForce;

    int playerActionsFilledAmount = 0;
    int enemyActionsFilledAmount = 0;

    bool playerActionsFilled;
    bool enemyActionsFilled;

    bool turnCompared;

    private void Awake()
    {
        EventManager.Instance.starshipActivateModule += Action;
    }

    private void OnDestroy()
    {
        EventManager.Instance.starshipActivateModule += Action;
    }

    void Action(bool player, ElementKind kind, int force)
    {
        if (player)
            AddPlayerAction(kind, force);
        else
            AddEnemyAction(kind, force);
    }
    void AddPlayerAction(ElementKind kind, int force)
    {

        switch (kind)
        {
            case ElementKind.Attack:
                playerAttackForce = force;
                playerActionsFilledAmount++;
                break;
            case ElementKind.Defense:
                playerDefenseForce = force;
                playerActionsFilledAmount++;
                break;
            case ElementKind.Intel:
                playerIntelForce = force;
                playerActionsFilledAmount++;
                break;
            case ElementKind.Speed:
                playerSpeedForce = force;
                playerActionsFilledAmount++;
                break;
        }

        if (playerActionsFilledAmount >= 4)
        {
            playerActionsFilled = true;

            if (enemyActionsFilled && playerActionsFilled && !turnCompared)
                CompareActionForces();
        }
    }
    void AddEnemyAction(ElementKind kind, int force)
    {
        switch (kind)
        {
            case ElementKind.Attack:
                enemyAttackForce = force;
                enemyActionsFilledAmount++;
                break;
            case ElementKind.Defense:
                enemyDefenseForce = force;
                enemyActionsFilledAmount++;
                break;
            case ElementKind.Intel:
                enemyIntelForce = force;
                enemyActionsFilledAmount++;
                break;
            case ElementKind.Speed:
                enemySpeedForce = force;
                enemyActionsFilledAmount++;
                break;
        }

        if (enemyActionsFilledAmount >= 4)
        {
            enemyActionsFilled = true;

            if (enemyActionsFilled && playerActionsFilled && !turnCompared)
                CompareActionForces();
        }
    }
    void ResetAction()
    {
        turnCompared = false;
        playerActionsFilled = false;
        enemyActionsFilled = false;

        playerActionsFilledAmount = 0;
        enemyActionsFilledAmount = 0;

        playerSpeedForce = 0;
        playerDefenseForce = 0;
        playerAttackForce = 0;
        playerIntelForce = 0;

        enemySpeedForce = 0;
        enemyDefenseForce = 0;
        enemyAttackForce = 0;
        enemyIntelForce = 0;
    }
    void CompareActionForces()
    {
        turnCompared = true;
        Comparison();

        //ResetAction();
    }

    void Comparison()
    {
        if (playerSpeedForce >= enemySpeedForce)
        {
            DamageEnemy(out bool enemyDestoyed);
            if (enemyDestoyed)
                return;

            DamagePlayer(out bool playerDestoyed);
        }
        else
        {
            DamagePlayer(out bool playerDestoyed);
            if (playerDestoyed)
                return;

            DamageEnemy(out bool enemyDestoyed);
        }
    }

    void DamagePlayer(out bool isDestroyed)
    {
        isDestroyed = false;
        int playerDeltaDamage = enemyAttackForce - playerDefenseForce;
        if (playerDeltaDamage > 0)
        {
            int damageResult = playerDeltaDamage * 1 + enemyIntelForce;
            playerLife -= damageResult;
            Debug.Log("Damaged Player with " + damageResult + " life points");
            if (playerLife <= 0)
            {
                Debug.Log("Player ship destroyed, Player LOOSE");
                isDestroyed = true;
            }
        }
    }
    void DamageEnemy(out bool isDestroyed)
    {
        isDestroyed = false;
        int enemyDeltaDamage = playerAttackForce - enemyDefenseForce;
        if (enemyDeltaDamage > 0)
        {
            Debug.Log("Damaged Enemie with " + enemyDeltaDamage + " life points");
            enemyLife -= enemyDeltaDamage * 1 + playerIntelForce;

            if (enemyLife <= 0)
            {
                Debug.Log("Enemy ship destroyed, Player WIN");
                isDestroyed = true;
            }
        }
    }
}
