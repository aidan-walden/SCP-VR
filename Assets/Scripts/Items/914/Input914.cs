using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input914 : MonoBehaviour {
    private List<GameObject> inputObjects = new List<GameObject>();
    // Use this for initialization
    void Start () {
        Debug.Log(transform.parent.transform.parent.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name + " is in 914");
        if(!inputObjects.Contains(collision.transform.root.gameObject) && collision.transform.root.name != "Environment")
        {
            inputObjects.Add(collision.transform.root.gameObject); //Adds objects in the input box to an array
        }
        foreach(GameObject item in inputObjects)
        {
            Debug.Log(item.name + "Is an item within 914");
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision.gameObject.name + " has left 914");
        inputObjects.Remove(collision.transform.root.gameObject);
        foreach (GameObject item in inputObjects)
        {
            Debug.Log(item.name);
        }
    }

    public List<GameObject> getInput()
    {
        return inputObjects;
    }
}
