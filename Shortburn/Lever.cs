using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RetractableChandelier chandelier;

    [SerializeField] private TMP_Text _pressKeyText;

    private bool _inTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SC_FPSController>() != null)
        {
            _pressKeyText.enabled = true;
            _inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<SC_FPSController>() != null)
        {
            _pressKeyText.enabled = false;
            _inTrigger = false;
        }
    }

    private void Update()
    {
        if (_inTrigger)
        {
            //Get Key
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Run LeverAnimation coroutine
                StartCoroutine(LeverAnimation(1f));
            }
        }
    }
    /// <summary>
    /// Wait ... amount of time
    /// </summary>
    /// <param name="waitTime">How much time to wait</param>
    private IEnumerator LeverAnimation(float waitTime)
    {
        //Activate Lever Animation Bool
        animator.SetBool("Activate", true);
        //Enable chandelier movement
        chandelier.SetRequiredHeight();
        yield return new WaitForSeconds(waitTime);
        //Deactivate Lever Animation Bool
        animator.SetBool("Activate", false);
        yield return null;
    }
}
