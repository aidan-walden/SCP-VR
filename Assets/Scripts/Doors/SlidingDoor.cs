using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlidingDoor : MonoBehaviour {
    public Transform openPos;
    private Vector3 moveTo, origPos;
    private NavMeshAgent enemyNav;
    public float speed = 0.43143504788f;
    public bool enemyCanOpen = true;
    public bool doorChanging, doorStartsOpen, isDependent = false;
    public AudioClip doorOpen, doorClose;
    private bool doorIsOpen = false;
    [HideInInspector] public AudioSource doorSounds;

    // Use this for initialization
    void Start () {
        origPos = transform.position;
        moveTo = origPos;
        doorSounds = GetComponent<AudioSource>();
        doorStartsOpen = doorIsOpen;
        if(!enemyCanOpen && !isDependent)
        {
            GetComponent<OffMeshLink>().activated = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(doorChanging)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
            if(transform.position == moveTo)
            {
                doorChanging = false;
                doorIsOpen = !doorIsOpen;
            }
        }
        
    }


    public void moveDoor(bool openDoor)
    {
        if (doorStartsOpen)
        {
            openDoor = !openDoor; //Invert open door variable because the door is meant to start closed.
        }
        if (openDoor)
        {
            if(!isDependent)
            {
                doorSounds.clip = doorOpen;
            }
            moveTo = openPos.position;
        }
        else
        {
            if(!isDependent)
            {
                doorSounds.clip = doorClose;
            }
            moveTo = origPos;
        }
        if(!isDependent)
        {
            doorSounds.Play();
        }
        doorChanging = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isDependent)
        {
            if (other.transform.root.gameObject.tag != "Environment")
            {
                enemyNav = other.transform.root.GetComponent<NavMeshAgent>();
            }
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(!isDependent)
        {
            if (enemyNav != null && enemyCanOpen)
            {
                if (enemyNav.isOnOffMeshLink)
                {
                    if (!doorIsOpen)
                    {
                        //speed *= 1.5f;
                        moveDoor(true);
                    }

                    if (!doorChanging)
                    {
                        //speed /= 1.5f;
                        enemyNav.CompleteOffMeshLink();

                    }

                }
            }
        }
    }
}