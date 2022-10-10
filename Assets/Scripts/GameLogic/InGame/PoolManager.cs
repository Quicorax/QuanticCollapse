using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GenericEventBus _PoolLoaded;

    private int _poolSize = 63;

    [System.Serializable]
    public class BlockViewPool
    {
        public int poolCellId;
        public GameObject blockPrefab;

        public BlockViewPool(int poolCellId, GameObject blockPrefab)
        {
            this.poolCellId = poolCellId;
            this.blockPrefab = blockPrefab;
        }
    }

    [SerializeField] private List<BlockViewPool> _internalBlockViewPoolList = new();
    [HideInInspector] public Dictionary<int, Queue<GameObject>> blockViewPoolsDictionary = new();

    async void Awake()
    {
        await LoadAddressables();
        InitializePool();
    }
    private async Task LoadAddressables()
    {
        foreach (var item in ServiceLocator.GetService<GameConfigService>().GridBlocks)
        {
            await ServiceLocator.GetService<AddressablesService>().SpawnAddressablePoolObject(item.AdrsKey, x => 
            {
                _internalBlockViewPoolList.Add(new(item.Id, x));
            });
        }
    }
    private void InitializePool()
    {
        foreach (BlockViewPool pool in _internalBlockViewPoolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject blockView = Instantiate(pool.blockPrefab, transform);
                blockView.transform.localScale = Vector2.zero;
                blockView.SetActive(false);
    
                objectPool.Enqueue(blockView);
            }
    
            blockViewPoolsDictionary.Add(pool.poolCellId, objectPool);
        }

        _PoolLoaded.NotifyEvent();
    }
    public GameObject SpawnBlockView(int id, Vector2 coords)
    {
        GameObject blockView = blockViewPoolsDictionary[id].Dequeue();
        
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
            blockViewPoolsDictionary[id].Enqueue(blockView);
        });
    }
}
