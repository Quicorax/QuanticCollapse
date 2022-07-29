using UnityEngine;

public class InitialModelState : MonoBehaviour
{
    private VirtualGridView _View;

    public PlayerStarshipData playerData;
    public EnemyStarshipData enemyData;
    private void Awake()
    {
        _View = GetComponent<VirtualGridView>();
    }

    private void Start()
    {
        _View.ModifyPlayerLife(playerData.starshipLife);
        _View.ModifyEnemyLife(enemyData.starshipLife);
    }
}
