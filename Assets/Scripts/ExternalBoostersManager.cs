using TMPro;
using UnityEngine;

//Called on Button Event at UI "ExternalBoosters"
public class ExternalBoostersManager : MonoBehaviour
{
    public TMP_Text firstAidBoosterText;
    public TMP_Text damageEnemyBoosterText;
    public TMP_Text blockLaserBoosterText;

    FirstAidBooster firstAidBooster;
    DamageEnemyBooster damageEnemyBooster;
    BlockLaserBooster blockLaserBooster;


    private VirtualGridView _View;

    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        _View = GetComponent<VirtualGridView>();
    }
    private void Start()
    {
        InitBoosters();
    }

    private void InitBoosters()
    {
        firstAidBooster = new FirstAidBooster(_View, firstAidBoosterText);
        damageEnemyBooster = new DamageEnemyBooster(_View, damageEnemyBoosterText);
        blockLaserBooster = new BlockLaserBooster(inputManager, blockLaserBoosterText);
    }

    public void ExecuteFirsAidBooster()
    {
        firstAidBooster.TryUse();
    }

    public void ExecuteDamageEnemyBooster()
    {
        damageEnemyBooster.TryUse();
    }

    public void ExecuteBlockLaserBooster()
    {
        blockLaserBooster.TryUse();
    }
}
