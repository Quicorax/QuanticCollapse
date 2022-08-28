using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _WinConditionEventBus;
    [SerializeField] private CanvasDebugManager canvas;

    private MasterSceneManager _MasterSceneManager;
    [HideInInspector] public LevelModel LevelData;

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

    void SetMasterReference(MasterSceneManager masterReference) => _MasterSceneManager = masterReference;
    void SetLevelData(LevelModel levelReference) => LevelData = levelReference;

    void GiveRewards()
    {
        _MasterSceneManager.SaveFiles.progres.reputation++;

        List<LevelRewards> levelRewards = new()
        {
            GenerateReward(LevelData.RewardA),
            GenerateReward(LevelData.RewardB)
        };


        foreach (LevelRewards reward in levelRewards)
        {
            if (reward.RewardChance >= Random.Range(0, 100))
            {
                _MasterSceneManager.Inventory.AddElement(reward.RewardKind, reward.RewardAmount);
                canvas.SetRewardTextToWinPanel(reward.RewardKind, reward.RewardAmount);
            }
        }
    }

    LevelRewards GenerateReward(string rewardCode)
    {
        string[] RewardData = rewardCode.Split("_");

        string RewardKind = RewardData[0];
        int RewardAmount = Random.Range(int.Parse(RewardData[1]), int.Parse(RewardData[2]));
        int RewardChance = int.Parse(RewardData[3]);

        return new LevelRewards(RewardKind, RewardAmount, RewardChance);
    }
}
public enum RewardKind { Dilithium, AlianceCredits }
public struct LevelRewards
{
    public string RewardKind;
    public int RewardAmount;
    public int RewardChance;

    public LevelRewards(string rewardKind, int rewardAmount, int rewardChance)
    {
        RewardKind = rewardKind;
        RewardAmount = rewardAmount;
        RewardChance = rewardChance;
    }
}
