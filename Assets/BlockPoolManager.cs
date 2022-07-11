using System.Collections.Generic;
using UnityEngine;

public class BlockPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ElementKind poolKind;
        public GameObject kindPrefab;
        public int poolSize;
    }

    public Transform blockParent;
    public List<Pool> poolList = new();
    public Dictionary<ElementKind, Queue<GameObject>> poolsDictionary = new();


    void Start()
    {
        foreach (Pool pool in poolList)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.kindPrefab, transform);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            poolsDictionary.Add(pool.poolKind, objectPool);
        }
    }

    public GameObject SpawnFromPool(ElementKind kind, Vector2 coords)
    {
        GameObject objectToSpawn = poolsDictionary[kind].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = coords;

        return objectToSpawn;
    }

    public void DeSpawnObject(ElementKind kind, GameObject obj)
    {
        poolsDictionary[kind].Enqueue(obj);
        obj.SetActive(false);
    }

}
