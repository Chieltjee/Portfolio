using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupObjectPool : ObjectPool
{
    public static PowerupObjectPool instance;
    public GameObject[] powerupsPrefabs;
    public List<GameObject> powerupsObjectPool;
    public List<GameObject> powerups;
    [SerializeField] private int poolSize = 20;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //Create new pool.
        for (int i = 0; i < poolSize; i++)
        {
            int random = Random.Range(0, powerupsPrefabs.Length);
            ObjectPool.instance.ReturnObject(Instantiate(powerupsPrefabs[random]), powerupsObjectPool);
        }
    }
    /// <summary>
    /// Get object from pool.
    /// </summary>
    /// <returns>Object from pool.</returns>
    public GameObject GetObject()
    {
        GameObject obj = base.GetObject(powerupsObjectPool);
        powerups.Add(obj);
        return obj;
    }
    /// <summary>
    /// Return object to pool and delete data.
    /// </summary>
    /// <param name="obj">Returned object</param>
    public void ReturnObject(GameObject obj)
    {
        ChunkObjects objects = obj.GetComponent<ChunkObjects>();
        for (int i = 0; i < objects.powerups.Count; i++)
        {
            GameObject returnedObject = base.ReturnObject(powerups[0], powerupsObjectPool);
            powerups.Remove(returnedObject);
        }
        objects.powerups.Clear();
    }
}
