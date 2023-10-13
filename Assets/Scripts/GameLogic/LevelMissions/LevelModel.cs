namespace QuanticCollapse
{
    public struct Reward
    {
        public readonly string RewardId;
        public readonly int RewardAmount;

        public Reward(string rewardId, int rewardAmount)
        {
            RewardId = rewardId;
            RewardAmount = rewardAmount;
        }
    }

    [System.Serializable]
    public class LevelModel
    {
        public int Sector;
        public int Level;
        public string Color;
        public int ReputationCap;
        public int EnemyLevel;

        public LevelRewards[] Reward;

        public int LevelWidth;
        public int LevelHeight;
        public int[] LevelDisposition;
    }


    [System.Serializable]
    public class LevelRewards
    {
        public string RewardId;
        public int RewardMaxAmount;
        public int RewardMinAmount;
        public int RewardChance;
    }
}