using UnityEngine;

public class GamePlayLevelManager : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _WinConditionEventBus;

    private MasterSceneManager _MasterSceneManager;

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

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
        _WinConditionEventBus.Event += GiveRewards;
    }

    private void OnDestroy()
    {
        _WinConditionEventBus.Event -= GiveRewards;
    }
    void CallLevelDataInjected()
    {
        _LevelInjected.NotifyEvent(_levelData);
    }
    void GiveRewards()
    {
        foreach (var reward in _levelData.possibleRewards)
        {
            switch (reward.rewardKind)
            {
                case RewardKind.Reputation:
                    _MasterSceneManager.runtimeSaveFiles.progres.reputation += reward.rewardAmount;
                    break;
                case RewardKind.Dilithium:
                    _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount += reward.rewardAmount;
                    break;
                case RewardKind.AlianceCredits:
                    _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount += reward.rewardAmount;
                    break;
            }
        }
    }
}
