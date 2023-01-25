using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject[,] chunkobjects = new GameObject[3, 3];
    public List<GameObject> obstacles;
    public List<GameObject> powerups;

    /// <summary>
    /// Set array for object transforms.
    /// </summary>
    public void SetArray()
    {
        for (int x = 0; x < chunkobjects.GetLength(0); x++)
        {
            for (int y = 0; y < chunkobjects.GetLength(1); y++)
            {
                chunkobjects[x, y] = objects[x + y * chunkobjects.GetLength(0)];
            }
        }
    }
}
