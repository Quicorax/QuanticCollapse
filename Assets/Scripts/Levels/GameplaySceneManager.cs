using UnityEngine;

public class GameplaySceneManager : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _WinConditionEventBus;
    [SerializeField] private CanvasDebugManager canvas;

    private MasterSceneManager _MasterSceneManager;

    private LevelGridData _levelData;
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
        _MasterSceneManager.runtimeSaveFiles.progres.reputation ++;

        foreach (var reward in _levelData.possibleRewards)
        {
            if (Random.Range(0, 100) <= reward.rewardChance)
            {
                switch (reward.rewardKind)
                {
                    case RewardKind.Dilithium:
                        _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount += reward.rewardAmount;
                        break;
                    case RewardKind.AlianceCredits:
                         _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount += reward.rewardAmount;
                        break;
                    default:
                        break;
                }
                canvas.SetRewardTextToWinPanel(reward.rewardKind, reward.rewardAmount);
            }
        }
    }
}
