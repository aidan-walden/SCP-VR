using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElevatorMove : MonoBehaviour {

    public BoxCollider otherEle;
    public List<Transform> objectsInEle = new List<Transform>();
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y))
        {
            swapElevators();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name != "Environment")
        {
            Debug.Log(other.name + " has entered the elevator. Checking for validity...");
            if(other.transform.root == other.transform)
            {
                objectsInEle.Add(other.transform);
                Debug.Log(other.name + " has entered the elevator. Its root is: " + other.transform.root);
            }
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.name != "Environment")
        {
            foreach(Transform eleObject in objectsInEle)
            {
                if (eleObject.gameObject == other.gameObject)
                {
                    eleObject.position = other.transform.position;
                }
            }
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root == other.transform)
        {
            objectsInEle.Remove(other.transform);
        }
    }

    void swapElevators()
    {
        Debug.Log("swapping elevators");
        foreach(Transform passenger in objectsInEle)
        {
            Debug.Log(passenger.name + " is in the elevator");
            Debug.Log(passenger.name + "'s root is in the elevator. Proceeding...");
            Vector3 passPos = transform.position - passenger.position;
            Vector3 newPassPos = otherEle.transform.position - passPos;
            NavMeshAgent nav = passenger.gameObject.GetComponent<NavMeshAgent>();
            Debug.Log(passenger.name + ": " + passPos + ", " + newPassPos);
            if (nav != null)
            {
                nav.Warp(newPassPos);
            }
            else
            {
                passenger.position = newPassPos;
            }
        }
    }


}
