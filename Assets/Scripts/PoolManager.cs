using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class BlockViewPool
    {
        public ElementKind poolKind;
        public GameObject blockPrefab;
        public int poolSize;
    }

    public List<BlockViewPool> blockViewPoolList = new();
    public Dictionary<ElementKind, Queue<GameObject>> blockViewPoolsDictionary = new();

    void Start()
    {
        foreach (BlockViewPool pool in blockViewPoolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.blockPrefab, transform);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            blockViewPoolsDictionary.Add(pool.poolKind, objectPool);
        }
    }

    public GameObject SpawnBlockView(ElementKind kind, Vector2 coords)
    {
        GameObject blockView = blockViewPoolsDictionary[kind].Dequeue();

        blockView.SetActive(true);
        blockView.transform.position = coords;
        return blockView;
    }

    public void DeSpawnBlockView(ElementKind kind, GameObject obj)
    {
        obj.SetActive(false);
        blockViewPoolsDictionary[kind].Enqueue(obj);
    }
}
