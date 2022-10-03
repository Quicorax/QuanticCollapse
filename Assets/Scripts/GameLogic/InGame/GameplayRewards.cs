using System.Collections.Generic;
using UnityEngine;

public class GameplayRewards : MonoBehaviour
{
    [SerializeField] private LevelInjectedEventBus _LevelInjected;
    [SerializeField] private GenericEventBus _WinConditionEventBus;

    [SerializeField] private GameplayCanvasManager _canvas;
    [HideInInspector] public LevelModel LevelData;

    private GameProgressionService _gameProgression;

    private void Awake()
    {
        _WinConditionEventBus.Event += GiveRewards;
        _LevelInjected.Event += SetLevelData;

        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }
    private void OnDestroy()
    {
        _WinConditionEventBus.Event -= GiveRewards;
        _LevelInjected.Event -= SetLevelData;
    }

    void SetLevelData(LevelModel levelReference) => LevelData = levelReference;

    void GiveRewards()
    {
        int[] rewards = new int[3];

        if (!_gameProgression.CheckLevelWithIndexIsCompleted(LevelData.Level))
        {
            _gameProgression.UpdateElement("Reputation", 1);
            _gameProgression.SetLevelWithIndexCompleted(LevelData.Level);
            rewards[0] = 1;
        }

        List<LevelRewards> levelRewards = new()
        {
            GenerateReward(LevelData.RewardA),
            GenerateReward(LevelData.RewardB)
        };


        foreach (LevelRewards reward in levelRewards)
        {
            if (reward.RewardChance >= Random.Range(0, 100))
                _gameProgression.UpdateElement(reward.RewardId, reward.RewardAmount);
        }

        rewards[1] = levelRewards[0].RewardAmount;
        rewards[2] = levelRewards[1].RewardAmount;

        _canvas.PlayerWin(rewards);
    }

    LevelRewards GenerateReward(string rewardCode)
    {
        string[] RewardData = rewardCode.Split("_");

        string RewardId = RewardData[0];
        int RewardAmount = Random.Range(int.Parse(RewardData[1]), int.Parse(RewardData[2]));
        int RewardChance = int.Parse(RewardData[3]);

        return new LevelRewards(RewardId, RewardAmount, RewardChance);
    }

    public struct LevelRewards
    {
        public string RewardId;
        public int RewardAmount;
        public int RewardChance;

        public LevelRewards(string rewardKind, int rewardAmount, int rewardChance)
        {
            RewardId = rewardKind;
            RewardAmount = rewardAmount;
            RewardChance = rewardChance;
        }
    }
}
