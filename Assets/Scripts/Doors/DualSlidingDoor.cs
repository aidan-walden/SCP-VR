using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DualSlidingDoor : MonoBehaviour
{
    bool enemyCanOpen;
    public bool doorIsOpen = false;
    public bool doorStartsOpen = false;
    private NavMeshAgent enemyNav;
    private SlidingDoor[] doors;
    private AudioSource doorSounds;
    [SerializeField] private AudioClip doorOpen, doorClose;
    public float speed = 1.5f;
    // Use this for initialization
    void Start()
    {
        doorIsOpen = doorStartsOpen;
        doors = GetComponentsInChildren<SlidingDoor>();
        enemyCanOpen = doors[0].enemyCanOpen;
        doorSounds = GetComponent<AudioSource>();
        changeSpeed(speed);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            moveDoor(!doorIsOpen);
        }
    }
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag != "Environment")
        {
            enemyNav = other.transform.root.GetComponent<NavMeshAgent>();
            Debug.Log(enemyNav.name + " is in the trigger");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (enemyNav != null && enemyCanOpen)
        {
            if (enemyNav.isOnOffMeshLink)
            {
                Debug.Log(enemyNav.name + " is on the off mesh link");
                if (!doorIsOpen)
                {
                    //speed *= 1.5f;
                    moveDoor(true);
                }

                if (!doors[0].doorChanging)
                {
                    //speed /= 1.5f;
                    enemyNav.CompleteOffMeshLink();

                }

            }
        }
    }

    public void changeDoorState()
    {
        moveDoor(!doorIsOpen);
    }

    public void moveDoor(bool openDoor)
    {
        if(doorStartsOpen)
        {
            openDoor = !openDoor;
        }
        if(!doors[0].doorChanging && !doors[1].doorChanging)
        {
            if (openDoor)
            {
                doorSounds.clip = doorOpen;
            }
            else
            {
                doorSounds.clip = doorClose;
            }
            doorSounds.Play();
            foreach (SlidingDoor door in doors)
            {
                door.moveDoor(openDoor);
            }
            doorIsOpen = openDoor;
        } 
    }

    void changeSpeed(float doorSpeed)
    {
        foreach(SlidingDoor door in doors)
        {
            door.speed = doorSpeed;
        }
    }
}