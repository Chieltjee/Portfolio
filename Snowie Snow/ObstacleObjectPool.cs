using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleObjectPool : ObjectPool
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public List<GameObject> obstaclesObjectPool;
    public List<GameObject> obstacles;
    [SerializeField] private int poolSize = 25;

    private void Start()
    {
        //Create new pool.
        for (int i = 0; i < poolSize; i++)
        {
            int random = Random.Range(0, obstaclePrefabs.Length);
            ObjectPool.instance.ReturnObject(Instantiate(obstaclePrefabs[random]), obstaclesObjectPool);
        }
    }

    /// <summary>
    /// Get object from pool.
    /// </summary>
    /// <returns>Object from pool.</returns>
    public GameObject GetObject()
    {
        GameObject obj = base.GetObject(obstaclesObjectPool);
        obstacles.Add(obj);
        return obj;
    }
    /// <summary>
    /// Returns object and clears all old data.
    /// </summary>
    /// <param name="obj">Returned object.</param>
    public void ReturnObject(GameObject obj)
    {
        ChunkObjects objects = obj.GetComponent<ChunkObjects>();
        for (int i = 0; i < objects.obstacles.Count; i++)
        {
            GameObject returnedObject = base.ReturnObject(obstacles[0], obstaclesObjectPool);
            obstacles.Remove(returnedObject);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    objects.chunkobjects[x, y].GetComponent<ChunkObject>().isObstacle = false;
                }
            }
        }
        objects.obstacles.Clear();
    }
}
