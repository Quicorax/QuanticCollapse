using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace QuanticCollapse
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class BlockViewPool
        {
            public int PoolCellId;
            public GameObject BlockPrefab;

            public BlockViewPool(int poolCellId, GameObject blockPrefab)
            {
                PoolCellId = poolCellId;
                BlockPrefab = blockPrefab;
            }
        }

        [SerializeField] private GenericEventBus _PoolLoaded;

        private GameConfigService _config;

        private readonly List<BlockViewPool> _internalBlockViewPoolList = new();
        private readonly Dictionary<int, Queue<GameObject>> _blockViewPoolsDictionary = new();

        private readonly int _poolSize = 63;

        public GameObject SpawnBlockView(int id, Vector2 coords)
        {
            var blockView = _blockViewPoolsDictionary[id].Dequeue();

            blockView.transform.DOScale(1, 0.2f);

            blockView.SetActive(true);
            blockView.transform.position = coords;
            return blockView;
        }

        public void DeSpawnBlockView(int id, GameObject blockView)
        {
            blockView.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                blockView.SetActive(false);
                _blockViewPoolsDictionary[id].Enqueue(blockView);
            });
        }

        private async void Awake()
        {
            _config = ServiceLocator.GetService<GameConfigService>();

            await LoadRemoteBlockInstances();
            InitializePool();
        }

        private async Task LoadRemoteBlockInstances()
        {
            await Task.WhenAll(
                LoadBlockObject(_config.GridBlocks.BaseBlocks),
                LoadBlockObject(_config.GridBlocks.BoosterBlocks));
        }

        private async Task LoadBlockObject(List<BaseBlockModel> blocks)
        {
            foreach (var item in blocks)
            {
                await ServiceLocator.GetService<AddressablesService>().LoadAddrsPoolObject(item.AdrsKey, x =>
                    _internalBlockViewPoolList.Add(new(item.Id, x)));
            }
        }

        private void InitializePool()
        {
            foreach (var pool in _internalBlockViewPoolList)
            {
                var objectPool = new Queue<GameObject>();
                for (var i = 0; i < _poolSize; i++)
                {
                    var blockView = Instantiate(pool.BlockPrefab, transform);
                    blockView.transform.localScale = Vector2.zero;
                    blockView.SetActive(false);

                    objectPool.Enqueue(blockView);
                }

                _blockViewPoolsDictionary.Add(pool.PoolCellId, objectPool);
            }

            _PoolLoaded.NotifyEvent();
        }
    }
}