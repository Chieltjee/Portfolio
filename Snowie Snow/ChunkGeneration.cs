using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    public static ChunkGeneration instance;

    [SerializeField] private float chunkGenY;
    [SerializeField] private float chunkGenZ;

    private Vector3 newChunkPos;

    private ChunkObjectPool chunkObjectPool;
    private ObstacleObjectPool obstacleObjectPool;
    private PowerupObjectPool powerupObjectPool;

    private ChunkObjects objects;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        chunkObjectPool = FindObjectOfType<ChunkObjectPool>();
        obstacleObjectPool = FindObjectOfType<ObstacleObjectPool>();
        powerupObjectPool = FindObjectOfType<PowerupObjectPool>();

        StartCoroutine(StartGeneration(0f));
    }

    /// <summary>
    /// Starts the base generation of the map.
    /// </summary>
    /// <param name="sec">Time to wait before running rest of code.</param>
    /// <returns></returns>
    private IEnumerator StartGeneration(float sec)
    {
        yield return new WaitForSeconds(sec);
        NewChunk(2, false);
        NewChunk(8, true);
        yield return null;
    }

    /// <summary>
    /// Creates a new chunk
    /// </summary>
    /// <param name="amount">Amount of chunks to generate.</param>
    /// <param name="spawnObstacles">Bool that controls if there should be obstacles on the map chunk.</param>
    public void NewChunk(int amount, bool spawnObstacles)
    {
        for (int i = 0; i < amount; i++)
        {
            //Check if the amount of active map chunks is equal or higher than the max set in a variable.
            if (chunkObjectPool.mapChunks.Count >= chunkObjectPool.renderDistance)
            {
                //Return objects to objectpool
                obstacleObjectPool.ReturnObject(chunkObjectPool.mapChunks[0]);
                powerupObjectPool.ReturnObject(chunkObjectPool.mapChunks[0]);
                chunkObjectPool.ReturnObject(chunkObjectPool.mapChunks[0]);
            }

            //If there are any active mapchunks
            if (chunkObjectPool.mapChunks.Count > 0)
            {
                //Set pos to spawn chunk at
                newChunkPos = new Vector3(chunkObjectPool.mapChunks[chunkObjectPool.mapChunks.Count - 1].gameObject.transform.position.x, chunkObjectPool.mapChunks[chunkObjectPool.mapChunks.Count - 1].gameObject.transform.position.y - chunkGenY, chunkObjectPool.mapChunks[chunkObjectPool.mapChunks.Count - 1].gameObject.transform.position.z + chunkGenZ);
            }
            else
                newChunkPos = this.transform.position;

            //Set object to position
            chunkObjectPool.GetObject().transform.position = newChunkPos;

            //Get object script
            objects = chunkObjectPool.mapChunks[chunkObjectPool.mapChunks.Count - 1].GetComponent<ChunkObjects>();
            objects.SetArray();
            //int maxPowerups = 0;
            if (spawnObstacles)
            {
                if (objects != null)
                {
                    //Loop 3 times to spawn obstacles.
                    int unspawnableLane = UnityEngine.Random.Range(0, 3);
                    int maxPowerups = 0;
                    bool spawnObject = false;
                    for (int y = 0; y < 3; y++)
                    {
                        //Random chance to spawn obstacles
                        int chance = UnityEngine.Random.Range(0, 3);
                        int spawnChance = UnityEngine.Random.Range(0, 1);
                        if (spawnChance == 0)
                        {
                            //Make sure that the lane isnt the unspawnable lane
                            if (chance != unspawnableLane)
                            {
                                //Get last chunks script for the spawn points
                                ChunkObjects lastChunkObjects = chunkObjectPool.mapChunks[chunkObjectPool.mapChunks.Count - 2].GetComponent<ChunkObjects>();
                                //Check if the first obstacle point at next chunk is empty.
                                if (chance == 0 || chance == 2)
                                {
                                    if (lastChunkObjects.chunkobjects[1, 2].GetComponent<ChunkObject>().isObstacle == false)
                                    {
                                        spawnObject = true;
                                    }
                                }
                                else if (chance == 1)
                                {
                                    if (lastChunkObjects.chunkobjects[0, 2].GetComponent<ChunkObject>().isObstacle == false && lastChunkObjects.chunkobjects[2, 2].GetComponent<ChunkObject>().isObstacle == false)
                                    {
                                        spawnObject = true;
                                    }
                                }
                                if (spawnObject)
                                {
                                    //Get object and set position
                                    GameObject obj = obstacleObjectPool.GetObject();
                                    obj.transform.position = objects.chunkobjects[chance, y].transform.position;
                                    objects.chunkobjects[chance, y].GetComponent<ChunkObject>().isObstacle = true;
                                    //Add to list
                                    objects.obstacles.Add(obj);
                                }
                            }
                        }
                        //Check if there are not more than 1 powerup
                        if (maxPowerups < 1)
                        {
                            //Random chance for spawning and prefab
                            int powerupChance = UnityEngine.Random.Range(0, 20);
                            int powerupPrefab = UnityEngine.Random.Range(0, PowerupObjectPool.instance.powerupsPrefabs.Length);
                            if (powerupChance == 0)
                            {
                                if (objects.chunkobjects[powerupChance, y].GetComponent<ChunkObject>().isObstacle == false)
                                {
                                    //Get object and set position
                                    GameObject obj = powerupObjectPool.GetObject();
                                    obj.transform.position = objects.chunkobjects[powerupChance, y].transform.position;
                                    //Add to list
                                    objects.powerups.Add(obj);
                                    maxPowerups++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }        
}