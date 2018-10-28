using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElevatorMove : MonoBehaviour {

    public BoxCollider otherEle;
    List<Transform> objectsInEle = new List<Transform>();
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
        bool isValid = true;
        bool rootIsIn = false;
        if (other.transform.root.name != "Environment")
        {
            foreach(Transform passenger in objectsInEle)
            {
                if(other.transform.root.gameObject == passenger.gameObject) //Check for duplicate entries
                {
                    isValid = false;
                    break;
                }
                if (other.transform.root.gameObject == passenger.gameObject)
                {
                    rootIsIn = false;
                }
            }
            if (isValid && rootIsIn)
            {
                objectsInEle.Add(other.transform.root);
                Debug.Log(other.name + " has entered the elevator. Its root is: " + other.transform.root);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.name != "Environment")
        {
            for (int i = 0; i < objectsInEle.Count; i++)
            {
                if (objectsInEle[i].gameObject == other.gameObject)
                {
                    objectsInEle[i].position = other.transform.position;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool isValid = true;

        if (other.transform.root.name != "Environment")
        {
            foreach (Transform passenger in objectsInEle)
            {
                if (other.transform.root.gameObject != passenger.gameObject)
                {
                    isValid = false;
                    break;
                    
                }
            }
            if(isValid)
            {
                Debug.Log(other.name + " has entered the elevator. Its root is: " + other.transform.root);
                objectsInEle.Remove(other.transform.root);
            }
        }
    }

    void swapElevators()
    {
        foreach(Transform passenger in objectsInEle)
        {
            
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
