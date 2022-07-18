using System.Collections.Generic;
using UnityEngine;

public class BlockPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class BaseBlockPool
    {
        public ElementKind poolKind;
        public GameObject blockPrefab;
        public int poolSize;
    }

    public List<BaseBlockPool> baseBlockPoolList = new();
    public Dictionary<ElementKind, Queue<GameObject>> baseBlocksPoolsDictionary = new();


    void Start()
    {
        foreach (BaseBlockPool pool in baseBlockPoolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.blockPrefab, transform);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            baseBlocksPoolsDictionary.Add(pool.poolKind, objectPool);
        }
    }

    public GameObject SpawnObject(ElementKind kind, Vector2 coords)
    {
        GameObject objectToSpawn = baseBlocksPoolsDictionary[kind].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = coords;

        return objectToSpawn;
    }

    public void DeSpawnObject(ElementKind kind, GameObject obj)
    {
        baseBlocksPoolsDictionary[kind].Enqueue(obj);
        obj.SetActive(false);
    }

}
