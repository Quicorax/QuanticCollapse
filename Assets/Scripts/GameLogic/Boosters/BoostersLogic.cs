using System.Collections.Generic;

namespace QuanticCollapse
{
    [System.Serializable]
    public class Boster
    {
        public int Id;
        public int SpawnThreshold;
        public BaseBooster BoosterLogic;

        public Boster(int id, int spawnThreshold, BaseBooster boosterLogic)
        {
            Id = id;
            SpawnThreshold = spawnThreshold;
            BoosterLogic = boosterLogic;
        }
    }

    public class BoostersLogic
    {
        private List<Boster> _finalBoosters = new();

        private List<BaseBooster> _boosterLogicList = new()
        {
            new BoosterKindBased(102),
            new BoosterBomb(101),
            new BoosterRowColumn(100)
            //Add new Boosters Specific Logic Here
        };

        public BoostersLogic()
        {
            var config = ServiceLocator.GetService<GameConfigService>();
            Initialize(config);
        }

        private void Initialize(GameConfigService config)
        {
            foreach (var boosterToSpawn in config.GridBlocks.BoosterBlocks)
            {
                BaseBooster logic = _boosterLogicList.Find(x => x.BoosterKindId == boosterToSpawn.Id);
                if (logic != null)
                    _finalBoosters.Add(new(boosterToSpawn.Id, boosterToSpawn.SpawnThreshold, logic));
            }
        }
        public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster)
            => GetBooster(blockCountOnAggrupation, out booster);
        public bool GetBooster(int threshold, out BaseBooster booster)
        {
            foreach (var item in _finalBoosters)
            {
                if (threshold > item.SpawnThreshold)
                {
                    booster = item.BoosterLogic;
                    return true;
                }
            }

            booster = null;
            return false;
        }
    }
}