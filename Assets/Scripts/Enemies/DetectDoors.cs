using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDoors : MonoBehaviour {
    public List<Transform> doorsInRange = new List<Transform>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            SlidingDoor doorScript = other.gameObject.GetComponent<SlidingDoor>();
            if (!doorScript.doorIsLocked && doorScript.EnemyCanOpen)
            {
                doorsInRange.Add(other.transform);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Door" && doorsInRange.Contains(other.transform))
        {
            doorsInRange.Remove(other.transform);
        }
    }
}
