using UnityEngine;

public class InitialModelState : MonoBehaviour
{
    public VirtualGridView View;

    public PlayerStarshipData playerData;
    public EnemyStarshipData enemyData;

    private void Start()
    {
        View.ModifyPlayerLife(playerData.starshipLife);
        View.ModifyEnemyLife(enemyData.starshipLife);
    }
}
