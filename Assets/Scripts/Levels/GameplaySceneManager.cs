using UnityEngine;

public class GameplaySceneManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _WinConditionEventBus;
    [SerializeField] private CanvasDebugManager canvas;

    private MasterSceneManager _MasterSceneManager;
    private LevelGridData _LevelData;

    private void Awake()
    {
        _WinConditionEventBus.Event += GiveRewards;
        _MasterReference.Event += SetMasterReference;
        _LevelInjected.Event += SetLevelData;
    }
    private void OnDestroy()
    {
        _WinConditionEventBus.Event -= GiveRewards;
        _MasterReference.Event -= SetMasterReference; 
        _LevelInjected.Event -= SetLevelData;
    }

    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    void SetLevelData(LevelGridData levelReference) { _LevelData = levelReference; }

    void GiveRewards()
    {
        _MasterSceneManager.SaveFiles.progres.reputation ++;

        foreach (var reward in _LevelData.possibleRewards)
        {
            if (Random.Range(0, 100) <= reward.rewardChance)
            {
                switch (reward.rewardKind)
                {
                    case RewardKind.Dilithium:
                        _MasterSceneManager.SaveFiles.progres.dilithiumAmount += reward.rewardAmount;
                        break;
                    case RewardKind.AlianceCredits:
                         _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount += reward.rewardAmount;
                        break;
                    default:
                        break;
                }
                canvas.SetRewardTextToWinPanel(reward.rewardKind, reward.rewardAmount);
            }
        }
    }
}
