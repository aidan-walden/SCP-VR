using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour {
    public Transform openPos;
    private Vector3 moveTo;
    private Vector3 origPos;
    public float speed = 0.43143504788f;
    public bool openDoor = true;
    public bool doorChanging = false;
    public bool doorStartsOpen = false;
    public AudioClip doorOpen, doorClose;
    [HideInInspector] public AudioSource doorSounds;

    // Use this for initialization
    void Start () {
        origPos = transform.position;
        moveTo = origPos;
        doorSounds = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if(doorChanging)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
            if(transform.position == moveTo)
            {
                doorChanging = false;
            }
        }
        
    }


    public void moveDoor(bool openDoor)
    {
        if (doorStartsOpen)
        {
            openDoor = !openDoor; //We do this so that you dont really have to think about creating doors, as there will be a lot of them.
        }
        if (openDoor)
        {
            doorSounds.clip = doorOpen;
            moveTo = openPos.position;
        }
        else
        {
            doorSounds.clip = doorClose;
            moveTo = origPos;
        }
        doorSounds.Play();
        doorChanging = true;
    }

}
