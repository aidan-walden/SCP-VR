using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingDestDetector : MonoBehaviour {
    private List<GameObject> collidingObj = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        collidingObj.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObj.Remove(other.gameObject);
    }

    public List<GameObject> getCollidingObj()
    {
        return collidingObj;
    }
}
