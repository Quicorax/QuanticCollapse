using System.Collections.Generic;
using System.Linq;

namespace QuanticCollapse
{
    [System.Serializable]
    public class Booster
    {
        public int Id;
        public int SpawnThreshold;
        public BaseBooster BoosterLogic;

        public Booster(int id, int spawnThreshold, BaseBooster boosterLogic)
        {
            Id = id;
            SpawnThreshold = spawnThreshold;
            BoosterLogic = boosterLogic;
        }
    }

    public class BoostersLogic
    {
        private readonly List<Booster> _finalBoosters = new();

        private readonly List<BaseBooster> _boosterLogicList = new()
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
                var logic = _boosterLogicList.Find(x => x.BoosterKindId == boosterToSpawn.Id);
                if (logic != null)
                {
                    _finalBoosters.Add(new(boosterToSpawn.Id, boosterToSpawn.SpawnThreshold, logic));
                }
            }
        }

        public bool CheckBaseBoosterSpawn(int blockCountOnAggrupation, out BaseBooster booster) =>
            GetBooster(blockCountOnAggrupation, out booster);

        private bool GetBooster(int threshold, out BaseBooster booster)
        {
            foreach (var item in _finalBoosters.Where(item => threshold > item.SpawnThreshold))
            {
                booster = item.BoosterLogic;
                return true;
            }

            booster = null;
            return false;
        }
    }
}