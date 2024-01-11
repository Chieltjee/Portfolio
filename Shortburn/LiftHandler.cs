using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class LiftHandler : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    [SerializeField] private Transform _originalParent;

    public GameObject heldObject;
    [SerializeField] private bool _isHolding = false;

    [SerializeField] private GameObject _objectHolder;
    [SerializeField] private float _raycastLength;

    [SerializeField] private Quaternion _defaultRotation;

    /*[HideInInspector] */public KeyCode key;

    private void Start()
    {
        _cam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        _defaultRotation = Quaternion.identity;
    }
    private void Update()
    {
        //Debug.DrawRay(_cam.transform.position, _cam.transform.forward * _raycastLength, Color.magenta);
        //Mouse Input
        if (Input.GetKeyDown(key))
        {
            //Send Raycast
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _raycastLength))
            {
                //Check tag of colliding object
                if (hit.collider.gameObject.CompareTag("Small Character"))//use compare tag
                {
                    //Set Held Object
                    heldObject = hit.collider.gameObject;
                    //Set parent for object and reset position
                    hit.transform.SetParent(_originalParent);
                    hit.transform.localPosition = Vector3.zero;
                    //Disable gravity
                    hit.collider.gameObject.GetComponent<SC_FPSController>().applyGravity = false;
                }
                else if (hit.collider.gameObject.GetComponent<PickUp>() != null)
                {
                    //Set Held Object
                    heldObject = hit.collider.gameObject;
                    //Set parent for object and reset position
                    hit.transform.SetParent(_originalParent);
                    hit.transform.localPosition = Vector3.zero;
                    //Disable gravity
                    heldObject.GetComponent<Rigidbody>().useGravity = false;

                    heldObject.AddComponent<FixedJoint>();
                    FixedJoint l_fj = heldObject.GetComponent<FixedJoint>();
                    l_fj.connectedBody = _objectHolder.GetComponent<Rigidbody>();
                    l_fj.enableCollision = true;
                }
                _isHolding = true;
            }
        }
        //Mouse Input
        if (Input.GetKeyUp(key))
        {
            //If holding a object
            if (heldObject != null)
            {
                //Clear Parent
                heldObject.transform.SetParent(null);
                //Enable gravity
                if (heldObject.GetComponent<SC_FPSController>() != null)
                {
                    heldObject.GetComponent<SC_FPSController>().applyGravity = true;
                    heldObject.transform.rotation = _defaultRotation;
                }
                else
                {
                    heldObject.GetComponent<Rigidbody>().useGravity = true;
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(heldObject.GetComponent<FixedJoint>());
                }

                //Reset held object
                heldObject = null;
                _isHolding = false;
            }
        }
    }
}
