using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileMazeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Check tag
        if (other.gameObject.name == "Small Character")
        {
            //Allow throwing rocks
            other.gameObject.GetComponent<RockThrowing>().allowThrowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Check tag
        if (other.gameObject.name == "Small Character")
        {
            //Deny throwing rocks
            other.gameObject.GetComponent<RockThrowing>().allowThrowing = false;
        }
    }
}
