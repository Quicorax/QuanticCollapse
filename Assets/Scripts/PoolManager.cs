using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class BasePool
    {
        public ElementKind poolKind;
        public GameObject blockPrefab;
        public int poolSize;
    }

    public List<BasePool> basePoolList = new();
    public Dictionary<ElementKind, Queue<GameObject>> basePoolsDictionary = new();


    void Start()
    {
        foreach (BasePool pool in basePoolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.blockPrefab, transform);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            basePoolsDictionary.Add(pool.poolKind, objectPool);
        }
    }

    public GameObject SpawnObject(ElementKind kind, Vector2 coords)
    {
        GameObject objectToSpawn = basePoolsDictionary[kind].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = coords;

        return objectToSpawn;
    }

    public void DeSpawnObject(ElementKind kind, GameObject obj)
    {
        basePoolsDictionary[kind].Enqueue(obj);
        obj.SetActive(false);
    }

}
