using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjectPool : ObjectPool
{
    [SerializeField] private GameObject[] mapPrefabs;
    public List<GameObject> chunksObjectPool;
    public List<GameObject> mapChunks;
    public int renderDistance;
    [SerializeField] private int poolSize = 20;

    private void Awake()
    {
        //Create new pool.
        for (int i = 0; i < poolSize; i++)
        {
            int random = Random.Range(0, mapPrefabs.Length);
            ObjectPool.instance.ReturnObject(Instantiate(mapPrefabs[random]), chunksObjectPool);
        }
    }
    /// <summary>
    /// Get object from pool.
    /// </summary>
    /// <returns>Object pulled from pool.</returns>
    public GameObject GetObject()
    {
        GameObject obj = base.GetObject(chunksObjectPool);
        mapChunks.Add(obj);
        return obj;
    }
    /// <summary>
    /// Return object to pool.
    /// </summary>
    /// <param name="obj">Object that is being returned.</param>
    public void ReturnObject(GameObject obj)
    {
        GameObject returnedObject = base.ReturnObject(obj, chunksObjectPool);
        mapChunks.Remove(returnedObject);
    }
}
