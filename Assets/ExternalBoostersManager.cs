using UnityEngine;
using UnityEngine.UI;

public class ExternalBoostersManager : MonoBehaviour
{
    private VirtualGridView _View;

    private void Awake()
    {
        _View = GetComponent<VirtualGridView>();
    }

    //Called on Button Event at UI "ExternalBoosters"
    public void ExecuteFirsAidBooster()
    {
        FirstAidBooster booster = new FirstAidBooster();
        booster.TryUse(_View);
    }

    //Called on Button Event at UI "ExternalBoosters"
    public void ExecuteDamageEnemyBooster()
    {
        DamageEnemyBooster booster = new DamageEnemyBooster();
        booster.TryUse(_View);
    }
}
