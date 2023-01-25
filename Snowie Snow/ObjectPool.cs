using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Get object from pool.
    /// </summary>
    /// <param name="objectPool">Type of object pool.</param>
    /// <returns>Pooled object.</returns>
    public GameObject GetObject(List<GameObject> objectPool)
    {
        if (objectPool == null || objectPool.Count == 0)
        {
            print("Object Pool Ded");
        }
        int random = UnityEngine.Random.Range(0, objectPool.Count);
        GameObject pooledObject = objectPool[random]; 
        objectPool.Remove(pooledObject);

        pooledObject.SetActive(true);
        return pooledObject;
    }

    /// <summary>
    /// Returns object to pool
    /// </summary>
    /// <param name="obj">Object to return to pool.</param>
    /// <param name="objectPool">Object pool to return object to.</param>
    /// <returns>The object that will be returned.</returns>
    public GameObject ReturnObject(GameObject obj, List<GameObject> objectPool)
    {
        obj.SetActive(false);
        objectPool.Add(obj);
        return obj;
    }
}
