using UnityEngine;

//Called on Button Event at UI "ExternalBoosters"
public class ExternalBoostersManager : MonoBehaviour
{
    private VirtualGridView _View;

    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        _View = GetComponent<VirtualGridView>();
    }

    public void ExecuteFirsAidBooster()
    {
        FirstAidBooster booster = new FirstAidBooster(_View);
        booster.TryUse();
    }

    public void ExecuteDamageEnemyBooster()
    {
        DamageEnemyBooster booster = new DamageEnemyBooster(_View);
        booster.TryUse();
    }

    public void ExecuteBlockLaserBooster()
    {
        BlockLaserBooster booster = new BlockLaserBooster(inputManager);
        booster.TryUse();
    }
}
