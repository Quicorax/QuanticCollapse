using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class BlockViewPool
    {
        public ElementKind poolKind;
        public GameObject blockPrefab;
        public int poolSize;
    }

    [SerializeField] private List<BlockViewPool> blockViewPoolList = new();
    [SerializeField] private Dictionary<ElementKind, Queue<GameObject>> blockViewPoolsDictionary = new();

    void Awake()
    {
        InitializePool();
    }

    void InitializePool()
    {
        foreach (BlockViewPool pool in blockViewPoolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject blockView = Instantiate(pool.blockPrefab, transform);
                blockView.transform.localScale = Vector2.zero;
                blockView.SetActive(false);

                objectPool.Enqueue(blockView);
            }

            blockViewPoolsDictionary.Add(pool.poolKind, objectPool);
        }
    }
    public GameObject SpawnBlockView(ElementKind kind, Vector2 coords)
    {
        GameObject blockView = blockViewPoolsDictionary[kind].Dequeue();
        
        blockView.transform.DOScale(1, 0.2f);

        blockView.SetActive(true);
        blockView.transform.position = coords;
        return blockView;
    }

    public void DeSpawnBlockView(ElementKind kind, GameObject blockView)
    {
        blockView.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            blockView.SetActive(false);
            blockViewPoolsDictionary[kind].Enqueue(blockView);
        });
    }
}
