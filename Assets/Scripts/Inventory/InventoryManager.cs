using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public InventoryAddable invScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay(Collision collision)
    {
        //TODO: Also check for Interactable script once unity answers forums come back online
        if (collision.gameObject.transform.parent == null)
        {
            //Add item to inventory, execute special conditions
            if(invScript != null)
            {
                invScript.onAddedToInventory();
            }
            

        }
        else
        {
            //This else statement might not even need to exist, but it will to possibly serve a purpose in the future. Play an "invalid" sound, maybe?
        }
    }
}
