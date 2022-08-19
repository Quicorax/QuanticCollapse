using UnityEngine;

public enum RewardKind { Dilithium, AlianceCredits }

[System.Serializable]
public struct Reward
{
    public RewardKind rewardKind;
    public int rewardAmount;
    [Range(0, 100)] public int rewardChance;
}
