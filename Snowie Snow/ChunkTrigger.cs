using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    /// <summary>
    /// Spawn new chunk when trigger is activated
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        ChunkGeneration.instance.NewChunk(1, true);
    }
}