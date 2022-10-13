using System.Collections.Generic;

namespace QuanticCollapse
{
    [System.Serializable]
    public class BaseBlockModel
    {
        public int Id;
        public string AdrsKey;

        public int SpawnThreshold;
    }

    [System.Serializable]
    public class GidBlocks
    {
        public List<BaseBlockModel> BaseBlocks;
        public List<BaseBlockModel> BoosterBlocks;
    }
}