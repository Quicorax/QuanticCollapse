using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    [SerializeField] private LevelGridData _levelData;
    public LevelGridData LevelData 
    { 
        get => _levelData; 
        set 
        { 
            _levelData = value;
            CallLevelDataInjected();
        }
    }

    void CallLevelDataInjected()
    {
        _LevelInjected.NotifyEvent(_levelData);
    }

}
