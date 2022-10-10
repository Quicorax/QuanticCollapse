using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    private int _poolSize = 63;

    [System.Serializable]
    public class BlockViewPool
    {
        public int poolCellId;
        public GameObject blockPrefab;
    }

    [SerializeField] private List<BlockViewPool> blockViewPoolList = new();
    [HideInInspector] public Dictionary<int, Queue<GameObject>> blockViewPoolsDictionary = new();

    void Awake()
    {
        InitializePool();
    }

    public void InitializePool()
    {
        foreach (BlockViewPool pool in blockViewPoolList)
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
