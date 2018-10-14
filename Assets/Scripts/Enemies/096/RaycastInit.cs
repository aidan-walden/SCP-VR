using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInit : MonoBehaviour {
    public float viewArea = 5f;
    public float viewDistance = Mathf.Infinity;
    public GameObject headCollider;
    private ShyGuyTrigger triggerScript;
	// Use this for initialization
	void Start () {
        triggerScript = GetComponentInParent<ShyGuyTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "HeadCollider")
        {
            headCollider = other.gameObject;
            RaycastHit viewFace;
            /*
            if(Physics.SphereCast(other.gameObject.transform.position, viewArea, other.gameObject.transform.TransformDirection(Vector3.forward), out viewFace, viewDistance))
            {
                Debug.DrawRay(other.gameObject.transform.position, viewFace.collider.transform.position);
                Debug.Log("Player is viewing an object: " + viewFace.collider.name);
                if (viewFace.collider.name == "ShyGuyFace")
                {
                    Debug.Log("Player viewed 096!");
                    triggerScript.rageMode();
                }
            }
            */
            if(Physics.SphereCast(other.gameObject.transform.position, viewArea, other.gameObject.transform.TransformDirection(Vector3.forward), out viewFace, viewDistance))
            {
                Debug.Log("Player is viewing an object: " + viewFace.collider.name);
                if (viewFace.collider.name == "ShyGuyFace")
                {
                    Debug.Log("Player viewed 096!");
                    triggerScript.rageMode();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "HeadCollider")
        {
            Debug.Log("Player has entered raycast draw range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HeadCollider")
        {
            Debug.Log("Player has left raycast draw range");
        }
    }
}
